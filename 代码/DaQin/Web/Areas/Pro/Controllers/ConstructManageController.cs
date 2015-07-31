using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Common.Utils;
using Dal;
using Models;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Threading;

namespace DaQin.Areas.Pro.Controllers
{
    /// <summary>
    /// 施工单
    /// </summary>
    public class ConstructManageController : Common.ControllerBase
    {
        #region 变量
        ConstructDal constructDal = new ConstructDal();
        ConstructDetDal constructDetDal = new ConstructDetDal();
        ConstructAttDal constructAttDal = new ConstructAttDal();
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
        public ActionResult GetList(PagerModel pager, string billCode, string startDate, string endDate, string cusName, string proMinister, string address)
        {
            constructDal.GetList(ref pager, billCode, startDate, endDate, cusName, proMinister, address);
            var json = new
            {
                total = pager.totalRows,
                rows = pager.result as List<ConstructModel>
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Add页面
        [PermissionAttribute(Code = "Add")]
        public ActionResult Add()
        {
            ViewBag.billCode = BillCodeDal.Create("Pro_Construct");
            return View();
        }
        #endregion

        #region Edit页面
        [PermissionAttribute(Code = "Edit")]
        public ActionResult Edit(string id, string isView)
        {
            ViewBag.isView = isView;
            ConstructModel construct = constructDal.Get(id);
            ViewData["construct"] = construct;

            return View();
        }
        #endregion

        #region View页面
        [PermissionAttribute(Code = "View")]
        public ActionResult View(string id)
        {
            ConstructModel construct = constructDal.Get(id);
            ViewData["construct"] = construct;

            return View();
        }
        #endregion

        #region Upload页面
        public ActionResult Upload(int index)
        {
            ViewBag.index = index;

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
                string attatch = Request["attatch"];
                Pro_Construct construct = JsonHelper.ConvertToModel<Pro_Construct>(strParams);
                List<Pro_ConstructDet> constructDetList = JsonHelper.ConvertToList<Pro_ConstructDet>(detail);
                List<Pro_ConstructAtt> constructAttList = JsonHelper.ConvertToList<Pro_ConstructAtt>(attatch);

                constructDal.InsertOrUpdate(construct, constructDetList, constructAttList);

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
                constructDal.Del(StringUtil.ConvertIds(ids));

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
            List<ConstructDetModel> list = constructDetDal.GetList(parentId);
            var json = new
            {
                total = list.Count,
                rows = list
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 获取附件列表
        public ActionResult GetAttList(string parentId)
        {
            List<ConstructAttModel> list = constructAttDal.GetList(parentId);
            var json = new
            {
                total = list.Count,
                rows = list
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 上传文件
        public ActionResult UploadFile()
        {
            //首先判断路径是否存在,不存在则创建路径
            string path = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["UploadFiles"], "施工单附件/");
            string physicalPath = Server.MapPath(path);
            if (!Directory.Exists(physicalPath))
            {
                Directory.CreateDirectory(physicalPath);
            }

            HttpPostedFileBase file = Request.Files[0];
            string newFileName = file.FileName;

            int i = 0;
            while (true)
            {
                if (System.IO.File.Exists(Path.Combine(physicalPath, newFileName)))
                {
                    i++;
                    newFileName = Path.GetFileNameWithoutExtension(file.FileName) + i.ToString() + Path.GetExtension(file.FileName);
                }
                else
                {
                    break;
                }
                Thread.Sleep(1);
            }

            string savePath = Path.Combine(physicalPath, newFileName);
            file.SaveAs(savePath);
            var json = new
            {
                name = newFileName,
                url = Path.Combine(path, newFileName)
            };
            return Content(JsonConvert.SerializeObject(json));
        }
        #endregion

        #region 下载文件
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="url">文件url</param>
        public ActionResult DownloadFile(string name, string url)
        {
            try
            {
                name = Server.UrlDecode(name);
                string path = Server.MapPath(Server.UrlDecode(url));

                string UserAgent = Request.ServerVariables["http_user_agent"].ToLower();
                if (UserAgent.IndexOf("firefox") == -1)
                {
                    //非火狐浏览器
                    Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode(name));
                }
                else
                {
                    Response.AddHeader("content-disposition", "attachment;filename=" + name);
                }
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                byte[] bArr = new byte[fs.Length];
                fs.Read(bArr, 0, bArr.Length);
                fs.Close();
                Response.ContentEncoding = Encoding.UTF8;
                Response.BinaryWrite(bArr);
                Response.Flush();
                Response.End();
            }
            catch { }

            return Content("");
        }
        #endregion

    }
}
