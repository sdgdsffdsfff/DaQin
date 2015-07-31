using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Common
{
    /// <summary>
    /// 用于权限认证，标记在Action上面
    /// </summary>
    public class PermissionAttribute : ActionFilterAttribute
    {
        public string Code { get; set; }
    }
}
