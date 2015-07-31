using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Dal;
using Models;

namespace Common.Utils
{
    public class AdminUtil
    {
        #region 登录
        /// <summary>
        /// 登录
        /// </summary>
        public static bool Login(string userName, string pwd)
        {
            UserDal userDal = new UserDal();
            UserModel userModel = userDal.GetByUserName(userName);
            if (userModel != null)
            {
                if (userModel.Pwd == MD5Helper.Encrypt(pwd))
                {
                    HttpContext.Current.Session["_sysAuth"] = userModel;
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 退出登录
        /// <summary>
        /// 退出登录
        /// </summary>
        public static void LoginOut()
        {
            HttpContext.Current.Session.Clear();
        }
        #endregion

        #region 获取登录用户
        /// <summary>
        /// 获取登录用户
        /// </summary>
        public static UserModel LoginUser
        {
            get
            {
                return (UserModel)HttpContext.Current.Session["_sysAuth"];
            }
        }
        #endregion

        #region 判断是否登录
        /// <summary>
        /// 判断是否登录
        /// </summary>
        public static bool IsLogin
        {
            get
            {
                if (HttpContext.Current.Session["_sysAuth"] != null)
                {
                    return true;
                }
                return false;
            }
        }
        #endregion

        #region 判断用户是否有权限
        /// <summary>
        /// 判断用户是否有权限
        /// </summary>
        public static bool HasRights(ControllerBase controller, string action)
        {
            if (!IsLogin) return false;
            if (LoginUser.UserName == "admin") return true;
            UserDal userDal = new UserDal();
            List<FunctionModel> functionList = userDal.GetFunctionListByUserName(LoginUser.UserName);
            if (functionList.Exists(item => item.Controller == controller.GetType().FullName && item.Action == action))
            {
                return true;
            }
            return false;
        }
        #endregion

    }
}
