﻿using S_KYA_Common.Data.SqlServer;
using S_KYA_Common.Provider;
using S_KYA_Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S_KYA_Core.Dal
{
    public class Dal_Sys_Role
    {
        public static Dal_Sys_Role Instance
        {
            get { return SingletonProvider<Dal_Sys_Role>.Instance; }
        }

        public bool Exists(int RoleId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Sys_Role");
            strSql.Append(" where ");
            strSql.Append(" RoleID = @RoleId  ");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoleID", SqlDbType.Int,4)
            };
            parameters[0].Value = RoleId;
            Mod_Sys_Role o = (Mod_Sys_Role)SqlEasy.ExecuteScalar(strSql.ToString(), parameters);
            if (o != null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Mod_Sys_Role model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Sys_Role(");
            strSql.Append("RoleName");
            strSql.Append(") values (");
            strSql.Append("@RoleName");
            strSql.Append(") ");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                        new SqlParameter("@RoleName", SqlDbType.NVarChar,50)
            };

            parameters[0].Value = model.RoleName;

            object obj = SqlEasy.ExecuteNonQuery(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);

            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Mod_Sys_Role model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Sys_Role set ");

            strSql.Append(" RoleName = @RoleName ");
            strSql.Append(" where RoleID=@RoleID ");

            SqlParameter[] parameters = {
                        new SqlParameter("@RoleName", SqlDbType.NVarChar,50),
                        new SqlParameter("@RoleID", SqlDbType.Int,4)
            };

            parameters[0].Value = model.RoleName;
            parameters[1].Value = model.RoleID;
            int rows = SqlEasy.ExecuteNonQuery(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int RoleID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Sys_Role ");
            strSql.Append(" where RoleID=@RoleID");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoleID", SqlDbType.Int,4)
            };
            parameters[0].Value = RoleID;
            int rows = SqlEasy.ExecuteNonQuery(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
