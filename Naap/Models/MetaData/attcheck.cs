using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Naap.Models
{
    [MetadataType(typeof(attcheckMetaDate))]
    public partial class attcheck
    {
        public class attcheckMetaDate
        {
            [Key]
            public int sno { get; set; }
            [Display(Name = "日期時間")]
            public Nullable<System.DateTime> adate { get; set; }
            [Display(Name = "來源IP")]
            public string SourceIP { get; set; }      
           [Display(Name = "目的IP")]
            public string DestIP { get; set; }
            [Display(Name = "攻擊描述")]
            public string description { get; set; }
            [Display(Name = "攻擊次數")]
            public Nullable<short> atimes { get; set; }
            [Display(Name = "威脅等級")]
            public Nullable<int> is_valid { get; set; }
            [Display(Name = "攻擊類型")]
            public string attType { get; set; }
        }
    }
}