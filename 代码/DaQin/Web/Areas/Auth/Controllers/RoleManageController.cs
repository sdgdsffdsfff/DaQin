using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Dal;
using Models;

namespace DaQin.Areas.Auth.Controllers
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public class RoleManageController : Common.ControllerBase
    {
        #region 变量
        private RoleDal roleDal = new RoleDal();
        private MenuDal menuDal = new MenuDal();
        private FunctionDal functionDal = new FunctionDal();
        private RoleFunctionDal roleFunctionDal = new RoleFunctionDal();
        #endregion

        #region Index页面
        [PermissionAttribute(Code = "Index")]
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 获取列表
        /// <summary>
        /// 获取列表
        /// </summary>
        public ActionResult GetList(PagerModel pager)
        {
            roleDal.GetList(ref pager);
            var json = new
            {
                total = pager.totalRows,
                rows = pager.result as List<RoleModel>
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 角色权限页面
        [PermissionAttribute(Code = "RoleRights")]
        public ActionResult RoleRights(string roleId)
        {
            ViewBag.roleId = roleId;
            List<MenuModel> menuListAll = menuDal.GetListAll();
            List<MenuModel> rootMenuList = menuListAll.FindAll(item => item.Pid == "1");
            List<FunctionModel> functionListAll = functionDal.GetListAll();
            List<RoleFunctionModel> roleFunctionListAll = roleFunctionDal.GetListAll();
            ViewData["menuListAll"] = menuListAll;
            ViewData["rootMenuList"] = rootMenuList;
            ViewData["functionListAll"] = functionListAll;
            ViewData["roleFunctionListAll"] = roleFunctionListAll;

            return View();
        }
        #endregion

        #region 保存角色权限
        [PermissionAttribute(Code = "RoleRights")]
        [HttpPost]
        public ActionResult SaveRoleRights()
        {
            try
            {
                string roleId = Request["roleId"];
                string functionIds = Request["functionIds"];
                roleFunctionDal.InsertOrUpdate(roleId, functionIds);

                return Content("OK");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        #endregion

    }
}
