using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Utils
{
    public class StringUtil
    {
        /// <summary>
        /// 给ids加上单引号
        /// </summary>
        public static string ConvertIds(string ids)
        {
            string[] idArr = ids.Split(',');
            for (int i = 0; i < idArr.Length; i++)
            {
                idArr[i] = string.Format("'{0}'", idArr[i]);
            }
            return string.Join(",", idArr);
        }
    }
}
