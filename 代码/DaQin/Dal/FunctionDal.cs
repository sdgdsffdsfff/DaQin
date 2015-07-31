using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBUtil;
using Models;

namespace Dal
{
    /// <summary>
    /// 功能
    /// </summary>
    public class FunctionDal
    {
        #region 变量
        private SQLiteHelper sqliteHelper = new SQLiteHelper();
        #endregion

        #region 获取全部功能列表
        /// <summary>
        /// 获取全部功能列表
        /// </summary>
        public List<FunctionModel> GetListAll()
        {
            return sqliteHelper.FindListBySql<FunctionModel>(string.Format("select * from Sys_Function"));
        }
        #endregion

    }
}
