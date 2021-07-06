using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Naap.Models
{
    [MetadataType(typeof(SharpTbMetaDate))]
    public partial class SharpTb
    {
        private class SharpTbMetaDate
        {
            [Key]
            public int seq { get; set; }
            [Display(Name = "時間")]            
            public DateTime? DataTime { get; set; }
            [Display(Name = "來源IP")]
            public string SourceIP { get; set; }
            [Display(Name = "來源網卡")]
            public string SourceMac { get; set; }
            [Display(Name = "來源埠")]
            public string SourcePort { get; set; }
            [Display(Name = "目的IP")]
            public string DestIP { get; set; }
            [Display(Name = "目的網卡")]
            public string DestMac { get; set; }
            [Display(Name = "目的埠")]
            public string DestPort { get; set; }
            [Display(Name = "網路型式")]
            public string FromNet { get; set; }
            [Display(Name = "協定")]
            public string Protocol { get; set; }
        }
    }
}