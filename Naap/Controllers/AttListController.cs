using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Naap.Models;
using PagedList;

namespace Naap.Controllers
{
    public class AttListController : Controller        
    {
        SharpDbEntities db = new SharpDbEntities();
        [HttpGet]
        [LoginAuthorize(RoleList = "Member,Admin")]
        // GET: AttList
        //public ActionResult Index()
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var datas = db.attcheck.OrderBy(m => m.adate).ToList();
            //return View(db.attcheck.OrderBy(m=>m.adate).ToList());
            return View(datas.ToPagedList(page, pageSize));
        }
        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Delete(int id)
        {
            
                var data = db.attcheck.Where(m => m.sno == id).FirstOrDefault();
                if (data != null)
                {
                    db.attcheck.Remove(data);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");            
        }
    }
}