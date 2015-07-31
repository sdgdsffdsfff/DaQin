using Common;
using Common.Utils;
using Dal;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DaQin.Areas.Pro.Controllers
{
    /// <summary>
    /// 在建工地
    /// </summary>
    public class ConstructionSiteManageController : Common.ControllerBase
    {
        #region 变量
        ConstructionSiteDal constructionSiteDal = new ConstructionSiteDal();
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
        public ActionResult GetList(PagerModel pager, string address, string cusName, string designer, string proMinister, string checker, string startDateStart, string startDateEnd, string endDateStart, string endDateEnd)
        {
            constructionSiteDal.GetList(ref pager, address, cusName, designer, proMinister, checker, startDateStart, startDateEnd, endDateStart, endDateEnd);
            var json = new
            {
                total = pager.totalRows,
                rows = pager.result as List<ConstructionSiteModel>
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
            ConstructionSiteModel constructionSite = constructionSiteDal.Get(id);
            ViewData["constructionSite"] = constructionSite;

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
                Pro_ConstructionSite constructionSite = JsonHelper.ConvertToModel<Pro_ConstructionSite>(strParams);

                constructionSiteDal.InsertOrUpdate(constructionSite);

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
                constructionSiteDal.Del(StringUtil.ConvertIds(ids));

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
