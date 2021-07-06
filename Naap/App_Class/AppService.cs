using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Naap.Models;

/// <summary>
/// 應用程式服務類別
/// </summary>
public static class AppService
{
    /// <summary>
    /// 應用程式名稱
    /// </summary>
    public static string AppName
    {
        get
        {
            object obj_value = WebConfigurationManager.AppSettings["AppName"];
            return (obj_value == null) ? "未定義名稱" : obj_value.ToString();
        }
    }
    /// <summary>
    /// 除錯模式
    /// </summary>
    public static bool DebugMode
    {
        get
        {
            object obj_value = WebConfigurationManager.AppSettings["DebugMode"];
            string str_value = (obj_value == null) ? "0" : obj_value.ToString();
            return (str_value == "1");
        }
    }

    public static string OrderNo
    {
        get { return (HttpContext.Current.Session["OrderNo"] == null) ? "" : HttpContext.Current.Session["OrderNo"].ToString(); }
        set { HttpContext.Current.Session["OrderNo"] = value; }
    }

    public static string OrderName
    {
        get { return (HttpContext.Current.Session["OrderName"] == null) ? "" : HttpContext.Current.Session["OrderName"].ToString(); }
        set { HttpContext.Current.Session["OrderName"] = value; }
    }

    public static int OrderAmount 
    {
        get { return (HttpContext.Current.Session["OrderAmount"] == null) ? 0 : (int)HttpContext.Current.Session["OrderAmount"]; }
        set { HttpContext.Current.Session["OrderAmount"] = value; }
    }

    public static int OrderUserCount(string orderNo)
    {
        using (SharpDbEntities db = new SharpDbEntities())
        {
            return db.member.Where(m => m.mno == orderNo).Count();
        }
    }

    public static DateTime? StartDate { get; set; } = DateTime.MinValue;
    public static DateTime? EndDate { get; set; } = DateTime.MinValue;
    public static string SearText { get; set; }
    public static string ProtocolGen { get; set; } 
    public static List<SelectListItem> GetProtocolList()
    {
        List<SelectListItem> ProtocolList = new List<SelectListItem>();
        using (SharpDbEntities db = new SharpDbEntities())
        {
            var data = db.SharpTb.Select(c => new { c.Protocol }).Distinct().ToList();
            foreach (var item in data)
            {
                ProtocolList.Add(new SelectListItem()
                {
                    Text = item.Protocol,
                    Value = item.Protocol,
                    Selected = false
                });
            }
        }
        return ProtocolList;
    }
}
