using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Naap.Models
{
    public class NetWorkActiveViewModel
    {
        [Display(Name = "Start Date")]
        [DisplayFormat(ApplyFormatInEditMode =true , DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]

        public DateTime? EndDate { get; set; }
        public IPagedList<SharpTb> SharpTbList { get; set; }

        public string Protocol { get; set; }    //通訊協定
        public IEnumerable<SelectListItem> ProtocolList { get; set; } //通訊協定下拉選單用
        public string searchText { get; set; }              
       
    }
}