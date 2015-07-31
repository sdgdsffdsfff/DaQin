using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBUtil;
using Models;
using System.Security.Cryptography;

namespace Dal
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserDal
    {
        #region 变量
        private SQLiteHelper sqliteHelper = new SQLiteHelper();
        #endregion

        #region 获取列表
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<UserModel> GetList(ref PagerModel pager, string userName, string showName)
        {
            StringBuilder sql = new StringBuilder(string.Format(@"
                select *
                from Sys_User 
                where UserName<>'admin' "));

            if (!string.IsNullOrWhiteSpace(userName))
            {
                sql.AppendFormat(" and UserName like '%{0}%'", userName);
            }

            if (!string.IsNullOrWhiteSpace(showName))
            {
                sql.AppendFormat(" and ShowName like '%{0}%'", showName);
            }

            string orderby = string.Format("order by {0} {1}", pager.sort, pager.order);
            PagerModel pagerModel = sqliteHelper.FindPageBySql<UserModel>(sql.ToString(), orderby, pager.rows, pager.page);
            pager.totalRows = pagerModel.totalRows;
            pager.result = pagerModel.result;
            return pagerModel.result as List<UserModel>;
        }
        #endregion

        #region 添加或修改
        /// <summary>
        /// 添加或修改
        /// </summary>
        public void InsertOrUpdate(Sys_User user, string roleId)
        {
            try
            {
                sqliteHelper.BeginTransaction();

                if (string.IsNullOrWhiteSpace(user.Id))
                {
                    user.Id = sqliteHelper.NewId();
                    user.Pwd = Encrypt(user.Pwd);
                    sqliteHelper.Insert(user);
                }
                else
                {
                    UserModel old = GetById(user.Id);
                    if (old != null && old.Pwd != user.Pwd)
                    {
                        user.Pwd = Encrypt(user.Pwd);
                    }

                    sqliteHelper.Update(user);
                }

                sqliteHelper.Delete<Sys_UserRole>(string.Format(" UserId='{0}' ", user.Id));
                Sys_UserRole userRole = new Sys_UserRole();
                userRole.Id = sqliteHelper.NewId();
                userRole.UserId = user.Id;
                userRole.RoleId = roleId;
                sqliteHelper.Insert(userRole);

                sqliteHelper.CommitTransaction();
            }
            catch (Exception ex)
            {
                sqliteHelper.RollbackTransaction();
                throw ex;
            }
        }
        #endregion

        #region 添加或修改
        /// <summary>
        /// 添加或修改
        /// </summary>
        public void InsertOrUpdate(Sys_User user)
        {
            try
            {
                sqliteHelper.BeginTransaction();

                if (string.IsNullOrWhiteSpace(user.Id))
                {
                    user.Id = sqliteHelper.NewId();
                    user.Pwd = Encrypt(user.Pwd);
                    sqliteHelper.Insert(user);
                }
                else
                {
                    Sys_User old = GetById(user.Id);
                    if (old != null && old.Pwd != user.Pwd)
                    {
                        user.Pwd = Encrypt(user.Pwd);
                    }

                    sqliteHelper.Update(user);
                }

                sqliteHelper.CommitTransaction();
            }
            catch (Exception ex)
            {
                sqliteHelper.RollbackTransaction();
                throw ex;
            }
        }
        #endregion

        #region 获取
        /// <summary>
        /// 获取
        /// </summary>
        public UserModel GetById(string id)
        {
            UserModel user = sqliteHelper.FindBySql<UserModel>(string.Format(@"
                select u.*, ur.RoleId 
                from Sys_User u
                left join Sys_UserRole ur
                where u.Id='{0}'", id));

            return user;
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        public void Del(string ids)
        {
            try
            {
                sqliteHelper.BeginTransaction();

                sqliteHelper.BatchDelete<Sys_User>(ids);

                sqliteHelper.CommitTransaction();
            }
            catch (Exception ex)
            {
                sqliteHelper.RollbackTransaction();
                throw ex;
            }
        }
        #endregion

        #region 根据用户名获取用户
        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        public UserModel GetByUserName(string userName)
        {
            return sqliteHelper.FindBySql<UserModel>(string.Format("select * from Sys_User where UserName='{0}'", userName));
        }
        #endregion

        #region 根据用户名获取菜单
        /// <summary>
        /// 根据用户名获取菜单
        /// </summary>
        public List<MenuModel> GetMenuListByUserName(string userName)
        {
            List<FunctionModel> functionList = sqliteHelper.FindListBySql<FunctionModel>(string.Format(@"
                select DISTINCT func.*
                from Sys_Function func
                left join Sys_RoleFunction rf on func.Id=rf.FunctionId
                left join Sys_Role role on rf.RoleId=role.Id
                left join Sys_UserRole ur on role.Id=ur.RoleId
                left join Sys_User u on ur.UserId=u.Id
                where u.UserName='{0}'", userName));
            List<string> menuIdList = new List<string>();
            foreach (FunctionModel function in functionList)
            {
                if (!menuIdList.Exists(item => item == function.Id.Substring(0, function.Id.Length - 2)))
                {
                    menuIdList.Add(function.Id.Substring(0, function.Id.Length - 2));
                }
                if (!menuIdList.Exists(item => item == function.Id.Substring(0, function.Id.Length - 4)))
                {
                    menuIdList.Add(function.Id.Substring(0, function.Id.Length - 4));
                }
            }
            if (menuIdList.Count > 0)
            {
                return sqliteHelper.FindListBySql<MenuModel>(string.Format(@"
                select *
                from Sys_Menu 
                where Id in ({0})
                order by Sort", string.Join(",", menuIdList.ConvertAll<string>(item => string.Format("'{0}'", item)).ToArray())));
            }
            return new List<MenuModel>();
        }
        #endregion

        #region 根据用户名获取功能集合
        /// <summary>
        /// 根据用户名获取功能集合
        /// </summary>
        public List<FunctionModel> GetFunctionListByUserName(string userName)
        {
            return sqliteHelper.FindListBySql<FunctionModel>(string.Format(@"
                select DISTINCT func.*
                from Sys_Function func
                left join Sys_RoleFunction rf on func.Id=rf.FunctionId
                left join Sys_Role role on rf.RoleId=role.Id
                left join Sys_UserRole ur on role.Id=ur.RoleId
                left join Sys_User u on ur.UserId=u.Id
                where u.UserName='{0}'", userName));
        }
        #endregion

        #region MD5加密
        /// <summary>
        /// MD5加密
        /// </summary>
        public string Encrypt(string text)
        {
            byte[] bArr = ASCIIEncoding.UTF8.GetBytes(text);
            MD5 md5 = new MD5CryptoServiceProvider();
            bArr = md5.ComputeHash(bArr);
            StringBuilder result = new StringBuilder();
            foreach (byte b in bArr)
            {
                result.Append(b.ToString("X2"));
            }
            return result.ToString();
        }
        #endregion

    }
}
