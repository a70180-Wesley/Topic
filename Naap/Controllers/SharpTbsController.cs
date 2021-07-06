using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Naap.Models;
using PagedList;


namespace Naap.Controllers
{
    public class SharpTbsController : Controller
    {
        private SharpDbEntities db = new SharpDbEntities();

        // GET: SharpTbs
        public ActionResult Index(int page = 1, int pageSize = 20)
        {
            var datas = db.SharpTb.OrderByDescending(m => m.DataTime).ToList().Take(100);
            return View(datas.ToPagedList(page, pageSize));
            //return View(datas);
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult List(int page = 1, int pageSize = 20)
        {

            NetWorkActiveViewModel model = new NetWorkActiveViewModel();

            if (AppService.StartDate == DateTime.MinValue)
            {
                model.StartDate = DateTime.Now;
                model.EndDate = DateTime.Now;
            }
            else
            {
                model.StartDate = AppService.StartDate;
                model.EndDate = AppService.EndDate;
            }
            model.ProtocolList = AppService.GetProtocolList();
            model.Protocol = AppService.ProtocolGen;
            model.searchText = AppService.SearText;
            if (string.IsNullOrEmpty(model.searchText))
            {
                model.SharpTbList = db.SharpTb
                .Where(m => m.DataTime >= model.StartDate)
                .Where(m => m.DataTime <= model.EndDate)
                .Where(m => m.Protocol == model.Protocol)
                .OrderByDescending(m => m.DataTime)
                .ToPagedList(page, pageSize);
            }
            else
            {
                model.SharpTbList = db.SharpTb
                .Where(m => m.DataTime >= model.StartDate)
                .Where(m => m.DataTime <= model.EndDate)
                .Where(m => m.Protocol == model.Protocol)
                .Where(m => m.SourceIP.Contains(model.searchText) || m.DestIP.Contains(model.searchText) || m.SourcePort.Contains(model.searchText) || m.DestPort.ToString().Contains(model.searchText))
                .OrderByDescending(m => m.DataTime)
                .ToPagedList(page, pageSize);
            }

            return View(model);

        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult List(NetWorkActiveViewModel model)
        {
            AppService.StartDate = model.StartDate;
            AppService.EndDate = model.EndDate;
            AppService.ProtocolGen = model.Protocol;
            AppService.SearText = model.searchText;
            return RedirectToAction("List");

        }

        public ActionResult TestList()  //下拉式選單測試網頁
        {
            var data = db.SharpTb.DistinctBy(m => m.Protocol);
            List<SelectListItem> mySelectItemList = new List<SelectListItem>();
            foreach (var item in data)
            {
                mySelectItemList.Add(new SelectListItem()
                {
                    Text = item.Protocol,
                    Value = item.Protocol,
                    Selected = false
                });
            }

            NetWorkActiveViewModel model = new NetWorkActiveViewModel()
            {
                ProtocolList = mySelectItemList
            };

            return View(model);
        }

        // GET: SharpTbs/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: SharpTbs/Create
        //// 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        //// 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "seq,DataTime,SourceIP,SourceMac,SourcePort,DestIP,DestMac,DestPort,FromNet,Protocol")] SharpTb sharpTb)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.SharpTb.Add(sharpTb);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(sharpTb);
        //}

        // GET: SharpTbs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SharpTb sharpTb = db.SharpTb.Find(id);
            if (sharpTb == null)
            {
                return HttpNotFound();
            }
            return View(sharpTb);
        }

        // POST: SharpTbs/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "seq,DataTime,SourceIP,SourceMac,SourcePort,DestIP,DestMac,DestPort,FromNet,Protocol")] SharpTb sharpTb)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sharpTb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sharpTb);
        }

        // GET: SharpTbs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SharpTb sharpTb = db.SharpTb.Find(id);
            if (sharpTb == null)
            {
                return HttpNotFound();
            }
            return View(sharpTb);
        }

        // POST: SharpTbs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SharpTb sharpTb = db.SharpTb.Find(id);
            db.SharpTb.Remove(sharpTb);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //頁面重置按鈕        
        public ActionResult Reset(NetWorkActiveViewModel model)
        {
            AppService.StartDate = DateTime.Now;
            AppService.EndDate = DateTime.Now;
            AppService.ProtocolGen = model.Protocol;
            AppService.SearText = model.searchText;
            return RedirectToAction("List");

        }
    }
}
