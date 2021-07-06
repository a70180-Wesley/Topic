using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Naap.Models
{
    //[MetadataType(typeof(bas_member))]
    public  class bas_member
    {
        //private class bas_member_metadata
        //{
        [Key]
        public int rowid { get; set; }
        [Display(Name = "會員帳號")]
        [Required(ErrorMessage = "會員編號不可空白!!")]
        public string mno { get; set; }
        [Display(Name = "會員姓名")]
        [Required(ErrorMessage = "會員名稱不可空白!!")]
        public string mname { get; set; }
        [Required(ErrorMessage = "密碼不可空白")]
        [DataType(DataType.Password)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = false, HtmlEncode = true)]
        [Display(Name = "密碼")]
        public string password { get; set; }
        [Display(Name = "確認密碼")]
        [DataType(DataType.Password)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = false, HtmlEncode = true)]
        [System.ComponentModel.DataAnnotations.Compare("password", ErrorMessage = "確認密碼不相符!!")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "會員角色")]
        public string code_role { get; set; }
        [Display(Name = "性別")]
        [Required(ErrorMessage = "性別不可空白!!")]
        public string code_gender { get; set; }
        [Display(Name = "電子信箱")]
        [Required(ErrorMessage = "電子信箱不可空白!!")]
        [EmailAddress(ErrorMessage = "電子信箱格式錯誤!!")]
        public string email_member { get; set; }
        [Display(Name = "LineID")]
        public string line_id { get; set; }
        [Display(Name = "出生日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public Nullable<System.DateTime> date_birth { get; set; }
        [Display(Name = "建檔時間")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        public System.DateTime date_create { get; set; }
        [Display(Name = "核准")]
        public bool is_valid { get; set; }
        [Display(Name = "審核代碼")]
        public string code_valid { get; set; }
        [Display(Name = "備註")]
        public string remark { get; set; }
    }
    //}
}