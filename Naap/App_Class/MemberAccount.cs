using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Naap.Models;

/// <summary>
/// 會員資訊類別
/// </summary>
public static class MemberAccount
{
    /// <summary>
    /// 登入會員角色
    /// </summary>
    public static EnumList.LoginRole Role { get; set; } = EnumList.LoginRole.Guest;
    /// <summary>
    /// 登入會員角色名稱
    /// </summary>
    public static string RoleName { get { return EnumList.GetRoleName(Role); } }
    /// <summary>
    /// 會員帳號
    /// </summary>
    public static string MemberNo { get; set; } = "";
    /// <summary>
    /// 會員名稱
    /// </summary>
    public static string MemberName { get; set; } = "";
    /// <summary>
    /// 會員電子信箱
    /// </summary>
    public static string MemberEmail { get; set; } = "";
    /// <summary>
    /// 會員是否已登入
    /// </summary>
    public static bool IsLogin { get; set; } = false;

    public static void Login()
    {
        using (SharpDbEntities db = new SharpDbEntities())
        {
            var data = db.member.Where(m => m.mno == MemberNo).FirstOrDefault();
            if (data == null)
                Logout();
            else
            {
                IsLogin = true;
                MemberName = data.mname;
                MemberEmail = data.email_member;
                Role = EnumList.GetRoleType(data.code_role);
            }
        }
    }

    public static void Logout()
    {
        IsLogin = false;
        Role = EnumList.LoginRole.Guest;
        MemberNo = "";
        MemberName = "";
        MemberEmail = "";
    }
}
