using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Common;
using Common.Utils;
using Dal;
using Models;
using Newtonsoft.Json.Linq;

namespace DaQin.Areas.Pro.Controllers
{
    /// <summary>
    /// 跟踪表
    /// </summary>
    public class FollowManageController : Common.ControllerBase
    {
        #region 变量
        FollowDal followDal = new FollowDal();
        FollowDetDal followDetDal = new FollowDetDal();
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
        public ActionResult GetList(PagerModel pager, string startDate, string endDate, string address, string designer, string adviser, string follower)
        {
            followDal.GetList(ref pager, startDate, endDate, address, designer, adviser, follower);
            var json = new
            {
                total = pager.totalRows,
                rows = pager.result as List<FollowModel>
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Add页面
        [PermissionAttribute(Code = "Add")]
        public ActionResult Add()
        {
            return View();
        }
        #endregion

        #region Edit页面
        [PermissionAttribute(Code = "Edit")]
        public ActionResult Edit(string id, string isView)
        {
            ViewBag.isView = isView;
            FollowModel follow = followDal.Get(id);
            ViewData["follow"] = follow;

            return View();
        }
        #endregion

        #region View页面
        [PermissionAttribute(Code = "View")]
        public ActionResult View(string id)
        {
            FollowModel follow = followDal.Get(id);
            ViewData["follow"] = follow;

            return View();
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
                string detail = Request["detail"];
                Pro_Follow follow = JsonHelper.ConvertToModel<Pro_Follow>(strParams);
                List<Pro_FollowDet> followDetList = JsonHelper.ConvertToList<Pro_FollowDet>(detail);

                followDal.InsertOrUpdate(follow, followDetList);

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
                followDal.Del(StringUtil.ConvertIds(ids));

                return Content("OK");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        #endregion

        #region 获取明细列表
        public ActionResult GetDetList(string parentId)
        {
            List<FollowDetModel> list = followDetDal.GetList(parentId);
            var json = new
            {
                total = list.Count,
                rows = list
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
