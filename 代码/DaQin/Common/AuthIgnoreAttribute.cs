using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// 忽略权限拦截
    /// </summary>
    [Serializable, AttributeUsage(AttributeTargets.Method)]
    public class AuthIgnoreAttribute : Attribute
    {
    }
}
