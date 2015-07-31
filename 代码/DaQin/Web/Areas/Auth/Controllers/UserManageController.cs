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
    /// 用户管理
    /// </summary>
    public class UserManageController : Common.ControllerBase
    {
        #region 变量
        UserDal userDal = new UserDal();
        RoleDal roleDal = new RoleDal();
        #endregion

        #region Index页面
        [PermissionAttribute(Code = "Index")]
        public ActionResult Index()
        {
            ViewBag.AddRights = AdminUtil.HasRights(this, "Add");
            ViewBag.EditRights = AdminUtil.HasRights(this, "Edit");
            ViewBag.DelRights = AdminUtil.HasRights(this, "Del");
            ViewBag.ViewRights = AdminUtil.HasRights(this, "View");

            return View();
        }
        #endregion

        #region 获取列表
        /// <summary>
        /// 获取列表
        /// </summary>
        public ActionResult GetList(PagerModel pager, string userName, string showName)
        {
            userDal.GetList(ref pager, userName, showName);
            var json = new
            {
                total = pager.totalRows,
                rows = pager.result as List<UserModel>
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Add页面
        [PermissionAttribute(Code = "Add")]
        public ActionResult Add()
        {
            List<RoleModel> roleListAll = roleDal.GetListAll();
            ViewData["roleListAll"] = roleListAll;

            return View();
        }
        #endregion

        #region Edit页面
        [PermissionAttribute(Code = "Edit")]
        public ActionResult Edit(string id, string isView)
        {
            ViewBag.isView = isView;
            List<RoleModel> roleListAll = roleDal.GetListAll();
            ViewData["roleListAll"] = roleListAll;
            UserModel user = userDal.GetById(id);
            ViewData["user"] = user;

            return View();
        }
        #endregion

        #region ChangePwd页面
        public ActionResult ChangePwd(string id)
        {
            UserModel user = userDal.GetById(id);
            ViewData["user"] = user;

            return View();
        }

        [HttpPost]
        public ActionResult SaveChangePwd(string id)
        {
            try
            {
                string oldPwd = Request["oldPwd"];
                string pwd = Request["Pwd"];
                Sys_User user = userDal.GetById(id).Convert<Sys_User>();

                if (MD5Helper.Encrypt(oldPwd) != user.Pwd)
                {
                    return Content("您输入的旧密码不正确");
                }

                user.Pwd = pwd;
                userDal.InsertOrUpdate(user);

                return Content("OK");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        #endregion

        #region 保存添加或修改
        /// <summary>
        /// 保存添加或修改
        /// </summary>
        [HttpPost]
        public ActionResult InsertOrUpdate()
        {
            try
            {
                string strParams = Request["params"];
                string roleId = Request["roleId"];
                Sys_User user = JsonHelper.ConvertToModel<Sys_User>(strParams);

                userDal.InsertOrUpdate(user, roleId);

                return Content("OK");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        [HttpPost]
        [PermissionAttribute(Code = "Del")]
        public ActionResult Del(string ids)
        {
            try
            {
                userDal.Del(StringUtil.ConvertIds(ids));

                return Content("OK");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        #endregion

        #region 判断用户名是否重复
        /// <summary>
        /// 判断用户名是否重复
        /// </summary>
        [HttpPost]
        public ActionResult Exists(string id, string userName)
        {
            try
            {
                if (userName.Trim().ToLower() == "admin")
                {
                    var json = new { data = "不能使用 admin 作为用户名" };
                    return Json(json, JsonRequestBehavior.AllowGet);
                }

                UserModel user = userDal.GetByUserName(userName);

                if (string.IsNullOrWhiteSpace(id)) //添加
                {
                    if (user == null)
                    {
                        var json = new { data = true };
                        return Json(json, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var json = new { data = "用户" + userName + "已存在" };
                        return Json(json, JsonRequestBehavior.AllowGet);
                    }
                }
                else //修改
                {
                    UserModel old = userDal.GetById(id);

                    if (user.UserName == old.UserName)
                    {
                        var json = new { data = true };
                        return Json(json, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var json = new { data = "用户 " + userName + " 已存在" };
                        return Json(json, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        #endregion

    }
}
