//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Naap.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class attcheck
    {
        public int sno { get; set; }
        public Nullable<System.DateTime> adate { get; set; }
        public string SourceIP { get; set; }
        public string DestIP { get; set; }
        public string description { get; set; }
        public Nullable<short> atimes { get; set; }
        public Nullable<int> is_valid { get; set; }
        public string attType { get; set; }
    }
}
