﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S_KYA_Core.Dal;
using S_KYA_Core.Model;
using S_KYA_Common.Provider;
using S_KYA_Common;
namespace S_KYA_Core.Bll
{
    public class Bll_Sys_User
    {
        public static Bll_Sys_User Instance
        {
            get { return SingletonProvider<Bll_Sys_User>.Instance; }
        }
        public bool UserLogin(string UserName, string Pwd, int saveCookieDay = 1)
        {
            if (UserLogin(UserName, Pwd))
            {
                return true;
            }
            return false;
        }


        //用户登陆
        public bool UserLogin(string UserName, string Pwd)
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Pwd))
                return false;

            Mod_Sys_User u = Dal.Dal_Sys_User.Instance.testGetUser(UserName);
            if (null == u)
            {
                return false;
            }
            string md5pass = StringHelper.MD5string(Pwd + u.PassSalt);
            if (u.PassWord.Equals(md5pass, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
    }

}