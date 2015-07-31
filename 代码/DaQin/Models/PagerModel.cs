using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class PagerModel
    {
        /// <summary>
        /// 当前页数
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 每页记录数
        /// </summary>
        public int rows { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string sort { get; set; }
        /// <summary>
        /// 排序的方式asc，desc
        /// </summary>
        public string order { get; set; }
        /// <summary>
        /// 记录
        /// </summary>
        public object result { get; set; }
        /// <summary>
        /// 记录数
        /// </summary>
        public int totalRows { get; set; }
    }
}