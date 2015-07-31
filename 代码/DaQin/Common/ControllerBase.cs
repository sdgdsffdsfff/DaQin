using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Common.Utils;

namespace Common
{
    public class ControllerBase : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            MethodInfo method = filterContext.Controller.GetType().GetMethod(filterContext.ActionDescriptor.ActionName);
            object[] authIgnoreAttributes = method.GetCustomAttributes(typeof(AuthIgnoreAttribute), false);
            if (authIgnoreAttributes.Length == 0)
            {
                if (!AdminUtil.IsLogin) //如果没有登录，则跳转到登录页面
                {
                    if (filterContext.HttpContext.Request.HttpMethod == "POST")
                    {
                        filterContext.Result = Content("账户失效，请重新登录");
                    }
                    else
                    {
                        filterContext.Result = JavaScript("<script>top.window.location='/Auth/Account/Login';</script>");
                    }
                    return;
                }
                else
                {
                    object[] permissionAttributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(PermissionAttribute), false);
                    if (permissionAttributes.Length > 0)
                    {
                        if (!(((PermissionAttribute)permissionAttributes[0]).Code == "Edit" && filterContext.ActionParameters["isView"] != null && filterContext.ActionParameters["isView"].ToString() == "true"))
                        {
                            if (!AdminUtil.HasRights(this, ((PermissionAttribute)permissionAttributes[0]).Code))
                            {
                                filterContext.Result = Content("没有权限");
                            }
                        }
                    }
                }
            }
        }
    }
}
