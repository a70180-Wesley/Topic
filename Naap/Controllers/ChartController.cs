using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Naap.Models;
using Newtonsoft.Json;

namespace Naap.Controllers
{
    public class ChartController : Controller
    {
        // GET: Chart
        public ActionResult BarChart01()
        {
            using (SharpDbEntities db = new SharpDbEntities())
            {
                //來源10大
                List<string> labelList = new List<string>();
                List<int> dataList = new List<int>();               
                var datas = db.SharpTb.GroupBy(m => m.SourceIP)
                    .Select(c => new
                    {
                        SourceIP = c.Key,
                        IPCount = c.Count()
                    })
                    .OrderByDescending(m => m.IPCount)
                    .Take(10)
                    .ToList();

                foreach(var item in datas)
                {
                    labelList.Add(item.SourceIP);
                    dataList.Add(item.IPCount);
                }
                ViewBag.LabelList = JsonConvert.SerializeObject(labelList); //利用JSON轉成序列化
                ViewBag.DataList = JsonConvert.SerializeObject(dataList);

                //目的10大
                List<string> destList = new List<string>();
                List<int> destData = new List<int>();
                var destdatas = db.SharpTb.GroupBy(m => m.DestIP)
                    .Select(d => new
                    {
                        DestIP = d.Key,
                        IPCount1 = d.Count()
                    })
                    .OrderByDescending(m => m.IPCount1)
                    .Take(10)
                    .ToList();

                foreach (var item in destdatas)
                {
                    destList.Add(item.DestIP);
                    destData.Add(item.IPCount1);
                }
                ViewBag.destList = JsonConvert.SerializeObject(destList); //利用JSON轉成序列化
                ViewBag.destData = JsonConvert.SerializeObject(destData);

                //使用服務埠10大
                List<int> PortList = new List<int>();
                List<int> ServiceList = new List<int>();
                var datas2 = db.SharpTb.GroupBy(m => m.DestPort)
                    .Select(e => new
                    {
                        DestPort = e.Key,
                        Count = e.Count()
                    })
                    .OrderByDescending(m => m.Count)
                    .Take(10)
                    .ToList();

                foreach (var item in datas2)
                {
                    PortList.Add((int)item.DestPort);
                    ServiceList.Add(item.Count);
                }
                ViewBag.PortList = JsonConvert.SerializeObject(PortList); //利用JSON轉成序列化
                ViewBag.ServiceList = JsonConvert.SerializeObject(ServiceList);


                //使用服務10大
                List<string> PortList1 = new List<string>();
                List<int> ServiceList1 = new List<int>();
                var datas3 = db.SharpTb.Join(db.PortList,c => c.DestPort,cd => cd.port,(c, cd) => new{c,cd})
                    .GroupBy(m=>m.cd.description)
                    .Select(e => new
                    {
                        DestService = e.Key,
                        Count = e.Count()
                    })
                    .OrderByDescending(m => m.Count)
                    .Take(10)
                    .ToList();

                foreach (var item in datas3)
                {
                    PortList1.Add(item.DestService);
                    ServiceList1.Add(item.Count);
                }
                ViewBag.PortList1 = JsonConvert.SerializeObject(PortList1); //利用JSON轉成序列化
                ViewBag.ServiceList1 = JsonConvert.SerializeObject(ServiceList1);

                //10大威脅
                List<string> AttList = new List<string>();
                List<int> AttDate = new List<int>();
                var Attdatas = db.attcheck.GroupBy(m => m.attType)
                    .Select(e => new
                    {
                        attType = e.Key,
                        Count = e.Count()
                    })
                    .OrderByDescending(m => m.Count)
                    .Take(10)
                    .ToList();

                foreach (var item in Attdatas)
                {
                    AttList.Add(item.attType);
                    AttDate.Add(item.Count);
                }
                ViewBag.AttList = JsonConvert.SerializeObject(AttList); //利用JSON轉成序列化
                ViewBag.AttDate = JsonConvert.SerializeObject(AttDate);

                return View();
            }
        }

        public ActionResult BarChart()
        {
            using (SharpDbEntities db = new SharpDbEntities())
            {
                //來源10大
                List<string> labelList = new List<string>();
                List<int> dataList = new List<int>();
                var datas = db.SharpTb.GroupBy(m => m.SourceIP)
                    .Select(c => new
                    {
                        SourceIP = c.Key,
                        IPCount = c.Count()
                    })
                    .OrderByDescending(m => m.IPCount)
                    .Take(10)
                    .ToList();

                foreach (var item in datas)
                {
                    labelList.Add(item.SourceIP);
                    dataList.Add(item.IPCount);
                }
                ViewBag.LabelList = JsonConvert.SerializeObject(labelList); //利用JSON轉成序列化
                ViewBag.DataList = JsonConvert.SerializeObject(dataList);

                //目的10大
                List<string> destList = new List<string>();
                List<int> destData = new List<int>();
                var destdatas = db.SharpTb.GroupBy(m => m.DestIP)
                    .Select(d => new
                    {
                        DestIP = d.Key,
                        IPCount1 = d.Count()
                    })
                    .OrderByDescending(m => m.IPCount1)
                    .Take(10)
                    .ToList();

                foreach (var item in destdatas)
                {
                    destList.Add(item.DestIP);
                    destData.Add(item.IPCount1);
                }
                ViewBag.destList = JsonConvert.SerializeObject(destList); //利用JSON轉成序列化
                ViewBag.destData = JsonConvert.SerializeObject(destData);

                //服務10大Port
                List<int> PortList = new List<int>();
                List<int> ServiceList = new List<int>();
                var datas2 = db.SharpTb.GroupBy(m => m.DestPort)
                    .Select(e => new
                    {
                        DestPort = e.Key,
                        Count = e.Count()
                    })
                    .OrderByDescending(m => m.Count)
                    .Take(10)
                    .ToList();

                foreach (var item in datas2)
                {
                    PortList.Add((int)item.DestPort);
                    ServiceList.Add(item.Count);
                }
                ViewBag.PortList = JsonConvert.SerializeObject(PortList); //利用JSON轉成序列化
                ViewBag.ServiceList = JsonConvert.SerializeObject(ServiceList);

                //使用服務10大
                List<string> PortList1 = new List<string>();
                List<int> ServiceList1 = new List<int>();
                var datas3 = db.SharpTb.Join(db.PortList, c => c.DestPort, cd => cd.port, (c, cd) => new { c, cd })
                    .GroupBy(m => m.cd.description)
                    .Select(e => new
                    {
                        DestService = e.Key,
                        Count = e.Count()
                    })
                    .OrderByDescending(m => m.Count)
                    .Take(10)
                    .ToList();

                foreach (var item in datas3)
                {
                    PortList1.Add(item.DestService);
                    ServiceList1.Add(item.Count);
                }
                ViewBag.PortList1 = JsonConvert.SerializeObject(PortList1); //利用JSON轉成序列化
                ViewBag.ServiceList1 = JsonConvert.SerializeObject(ServiceList1);

                //10大威脅
                List<string> AttList = new List<string>();
                List<int> AttDate = new List<int>();
                var Attdatas = db.attcheck.GroupBy(m => m.attType)
                    .Select(e => new
                    {
                        attType = e.Key,
                        Count = e.Count()
                    })
                    .OrderByDescending(m => m.Count)
                    .Take(10)
                    .ToList();

                foreach (var item in Attdatas)
                {
                    AttList.Add(item.attType);
                    AttDate.Add(item.Count);
                }
                ViewBag.AttList = JsonConvert.SerializeObject(AttList); //利用JSON轉成序列化
                ViewBag.AttDate = JsonConvert.SerializeObject(AttDate);
                return View();
            }
        }

     
    }
}