using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Naap.Models;
using PagedList;
using System.Web.Configuration;
using DevStudio.Securitys;
using DevStudio.Line;

namespace Naap.Controllers
{
    public partial class MemberController : Controller
    {
        SharpDbEntities db = new SharpDbEntities();
        // GET: User
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            MemberAccount.Logout();
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            using (Cryptographys cryp = new Cryptographys())
            {
                string str_password = cryp.SHA256Encode(model.Password);
                using (SharpDbEntities db = new SharpDbEntities())
                {
                    var data = db.member
                        .Where(m => m.mno == model.MemberNo)
                        .Where(m => m.password == str_password)
                        .Where(m => m.is_valid == 1)
                        .FirstOrDefault();
                    if (data == null)
                    {
                        ModelState.AddModelError("UserNo", "登入帳號或密碼輸入錯誤!!");
                        return View(model);
                    }

                    MemberAccount.MemberNo = model.MemberNo;
                    MemberAccount.Login();
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "Member,Admin")]
        public ActionResult List(int page = 1, int pageSize = 5)
        {
            bas_member model = new bas_member();
            var role = Convert.ToString(MemberAccount.Role);
            var check = "Member";
            if (string.Compare(check, role) == 0)
            {
                var datas = db.member.Where(m => m.mname == MemberAccount.MemberName).ToList();
                //var datas = db.member.Where(m => m.code_role == "M").OrderBy(m => m.mno).ToList();
                return View(datas.ToPagedList(page, pageSize));
            }
            else
            {
                var datas = db.member.OrderBy(m => m.mno).ToList();
                return View(datas.ToPagedList(page, pageSize));
            }
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "Member,Admin")]
        public ActionResult Edit(int id)
        {
            var data = db.member.Where(m => m.rowid == id).FirstOrDefault();
            if (data == null) return RedirectToAction("List");
            return View(data);

        }

        [HttpPost]
        [LoginAuthorize(RoleList = "Member,Admin")]
        public ActionResult Edit(member model)
        {
            //if (!ModelState.IsValid) return View(model);

            var data = db.member.Where(m => m.mno == model.mno).FirstOrDefault();
            if (data == null) return View(model);
            data.mno = model.mno;
            data.mname = model.mname;
            data.date_birth = model.date_birth;
            data.email_member = model.email_member;
            data.line_id = model.line_id;
            data.remark = model.remark;
            db.SaveChanges();

            //Line Notify
            using (LineNotify line = new LineNotify())
            {
                //string str_token = WebConfigurationManager.AppSettings["LineToken"].ToString();
                string str_token = data.remark;
                string msg = "會員："+data.mname + "修改個人資料!";
                line.SendTextMessage(str_token, msg);
            }

            return RedirectToAction("List");            
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Create()
        {
            bas_member model = new bas_member();
            model.code_gender = "M";
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Create(bas_member model)
        {
            //if (!ModelState.IsValid) return View(model);

            //自定義檢查
            bool bln_error = false;
            var check = db.member.Where(m => m.mno == model.mno).FirstOrDefault();
            if (check != null) { ModelState.AddModelError("", "帳號重覆註冊!"); bln_error = true; }
            check = db.member.Where(m => m.email_member == model.email_member).FirstOrDefault();
            if (check != null) { ModelState.AddModelError("", "電子信箱重覆註冊!"); bln_error = true; }
            if (bln_error) return View(model);
            //密碼加密
            using (Cryptographys cryp = new Cryptographys())
            {
                model.password = cryp.SHA256Encode(model.password);
                model.ConfirmPassword = model.password;
            }
            member user = new member();
            user.mno = model.mno;
            user.mname = model.mname;
            user.password = model.password;
            user.code_gender = model.code_gender;
            user.email_member = model.email_member;
            user.date_birth = (DateTime)model.date_birth;
            user.line_id = model.line_id;
            user.remark = model.remark;
            user.code_role = "M";  //設定角色代號為 User
            user.code_valid = UserAccount.GetNewVarifyCode(); //產生驗證碼
            user.is_valid = 0;
            user.date_create = DateTime.Now.ToString("G");

            //寫入資料庫
            try
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                db.member.Add(user);
                db.SaveChanges();
                db.Configuration.ValidateOnSaveEnabled = true;
            }
            catch (Exception ex)
            {
                string str_message = ex.Message;
            }

            //Line Notify
            using (LineNotify line = new LineNotify())
            {
                string str_token = WebConfigurationManager.AppSettings["LineToken"].ToString();
                string msg = "新會員：" + user.mname + "申請加入!";
                line.SendTextMessage(str_token, msg);
            }

            //寄出驗證信
            SendVerifyMail(model.email_member, user.code_valid);
            return RedirectToAction("SendEmailResult");

        }

        private string SendVerifyMail(string userEmail, string varifyCode)
        {
            string str_app_name = "網路活動管理平台";
            var str_url = string.Format("/Member/VerifyEmail/{0}", varifyCode);
            var str_link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, str_url);
            string str_subject = str_app_name + " - 帳號成功建立通知!!";
            string str_body = "<br/><br/>";
            str_body += "很高興告訴您，您的 " + str_app_name + " 帳戶已經成功建立. <br/>";
            str_body += "請按下下方連結完成驗證您的帳號程序!!<br/><br/>";
            str_body += "<a href='" + str_link + "'>" + str_link + "</a> ";
            str_body += "<br/><br/>";
            str_body += "本信件由電腦系統自動寄出,請勿回信!!<br/><br/>";
            str_body += string.Format("{0} 系統開發團隊敬上", str_app_name);

            using (GmailService gmail = new GmailService())
            {
                gmail.ReceiveEmail = userEmail;
                gmail.Subject = str_subject;
                gmail.Body = str_body;
                gmail.Send();
                return gmail.MessageText;
            }
        }
        public ActionResult SendEmailResult()
        {
            return View();
        }

        public ActionResult VerifyEmail(string id)
        {
            bool Status = false;
            db.Configuration.ValidateOnSaveEnabled = false;
            var user = db.member.Where(m => m.code_valid == id).FirstOrDefault();
            if (user == null)
            { ViewBag.Message = "驗證碼錯誤!!"; }
            else
            {
                if (user.is_valid == 1)
                { ViewBag.Message = "此電子信箱已完成驗證, 請勿重覆執行!!"; }
                else
                {
                    user.is_valid = 1;
                    db.SaveChanges();
                    Status = true;
                }
            }
            ViewBag.Status = Status;
            return View();
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Delete(int id)
        {
            using (SharpDbEntities db = new SharpDbEntities())
            {
                var data = db.member.Where(m => m.rowid == id).FirstOrDefault();
                if (data != null)
                {
                    db.member.Remove(data);
                    db.SaveChanges();
                }
                return RedirectToAction("List");
            }
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "Member,Admin")]
        public ActionResult ResetPassword()
        {
            ResetPasswordViewModel model = new ResetPasswordViewModel();
            return View(model);
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "Member,Admin")]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            using (Cryptographys cryp = new Cryptographys())
            {
                string str_password = cryp.SHA256Encode(model.NewPassword);
                using (SharpDbEntities db = new SharpDbEntities())
                {
                    var data = db.member.Where(m => m.mno == MemberAccount.MemberNo).FirstOrDefault();
                    if (data != null)
                    {
                        data.password = str_password;
                        db.SaveChanges();
                        TempData["MessageText"] = "密碼已更新,下次登入請使用新的密碼!!";
                    }
                    else
                    {
                        TempData["MessageText"] = "帳號不存在 , 密碼未更新!!";
                    }
                    return RedirectToAction("ResetPasswordReport");
                }
            }
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "Member,Admin")]
        public ActionResult ResetPasswordReport()
        {
            ViewBag.MessageText = TempData["MessageText"].ToString();
            return View();
        }


        public ActionResult Test()
        {
            return View();
        }
    }
    
}