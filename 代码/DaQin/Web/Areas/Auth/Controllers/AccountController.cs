using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Common.Utils;
using Dal;
using Models;

namespace DaQin.Areas.Auth.Controllers
{
    /// <summary>
    /// 用户登录、主界面相关
    /// </summary>
    public class AccountController : Common.ControllerBase
    {
        #region 变量
        UserDal userDal = new UserDal();
        MenuDal menuDal = new MenuDal();
        #endregion

        #region Index页面(系统主界面)
        public ActionResult Index()
        {
            List<MenuModel> menuList;
            if (AdminUtil.LoginUser.UserName == "admin")
            {
                menuList = menuDal.GetListAll();
            }
            else
            {
                menuList = userDal.GetMenuListByUserName(AdminUtil.LoginUser.UserName);
            }
            ViewData["rootMenuList"] = menuList.FindAll(item => item.Id.Length == 3);
            ViewData["subMenuListAll"] = menuList.FindAll(item => item.Id.Length == 5);

            return View();
        }
        #endregion

        #region Login页面
        [AuthIgnore]
        public ActionResult Login()
        {
            return View();
        }
        #endregion

        #region 登录
        [AuthIgnore]
        public ActionResult LoginIn(string userName, string pwd)
        {
            if (AdminUtil.Login(userName, pwd))
            {
                return Content("OK");
            }
            return Content("用户名或密码不正确");
        }
        #endregion

        #region 退出登录
        [AuthIgnore]
        public ActionResult LoginOut()
        {
            AdminUtil.LoginOut();
            return Content("OK");
        }
        #endregion

    }
}
