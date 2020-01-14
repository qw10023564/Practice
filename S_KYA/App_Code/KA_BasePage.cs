﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S_KYA_Core.Model;
using System.Web.SessionState;
using StackExchange.Redis;

public abstract class KA_BasePage : IHttpHandler, IRequiresSessionState
{

    private static bool useRedis = true;
    private static Mod_Sys_User _Sys_User = null;
    public static Mod_Sys_User Sys_User
    {
        get
        {
            if (useRedis)
            {
                //Cookie名字要设置
                if (HttpContext.Current.Request.Cookies["hello"] == null)
                {
                    return null;
                }
                var token = HttpContext.Current.Request.Cookies["hello"].Value;
                var user = MySessionService.Get<Mod_Sys_User>(token);
                return _Sys_User = _Sys_User ?? user as Mod_Sys_User;
            }
            else
            {
                //传统Session
                return _Sys_User = _Sys_User ?? HttpContext.Current.Session["kya_Sys_UserInfo"] as Mod_Sys_User;
            }
        }
        set
        {
            _Sys_User = value;
            if (_Sys_User != null)
            {
                if (useRedis)
                {
                    //存Cookie，存Session
                    var token = MySessionService.Add(_Sys_User, "hha");
                    HttpContext.Current.Response.Cookies.Clear();
                    HttpContext.Current.Response.Cookies.Add(new HttpCookie("hello", token));
                    HttpContext.Current.Response.Cookies["hello"].Expires = _Sys_User.ExpiresTime.AddMinutes(-2);//早两分钟失效
                }
                else
                {
                    //传统Session
                    HttpContext.Current.Session["kya_Sys_UserInfo"] = _Sys_User;
                }
            }
        }
    }

    public bool IsReusable => false;

    public abstract void ProcessRequest(HttpContext context);

    /// <summary>
    /// 页面检测权限
    /// </summary>
    /// <param name="Mid">页面ID</param>
    /// <returns></returns>
    public static bool CheckRights(string Mid)
    {
        bool hasRight = false;
        if (_Sys_User.UserName.ToLower() == "admin")
        {
            return true;
        }

        hasRight = false;
        //base.OperatorGroupIDs.Contains(p.Group_ID) && 
        List<Mod_Sys_Author> srg = (from p in kBuffer._Table_Sys_Author
                                    where p.ResourceId.ToString() == Mid && p.RoleId == _Sys_User.RoleId && p.ResourceType == "1"
                                    orderby p.ResourceId, p.RoleId
                                    select p).ToList();
        if (srg != null && srg.Count > 0)
        {
            hasRight = true;
        }
        return hasRight;
    }

    /// <summary>
    /// 页面按钮权限
    /// </summary>
    /// <param name="Bid">按钮ID</param>
    /// <returns></returns>
    public static bool CheckButtonRights(string Bid)
    {
        bool hasRight = false;
        if (_Sys_User.UserName.ToLower() == "admin")
        {
            return true;
        }

        hasRight = false;
        //base.OperatorGroupIDs.Contains(p.Group_ID) && 
        List<Mod_Sys_Author> srg = (from p in kBuffer._Table_Sys_Author
                                    where p.ResourceId.ToString() == Bid && p.RoleId == _Sys_User.RoleId && p.ResourceType == "2"
                                    orderby p.ResourceId, p.RoleId
                                    select p).ToList();
        if (srg != null && srg.Count > 0)
        {
            hasRight = true;
        }
        return hasRight;
    }
}
