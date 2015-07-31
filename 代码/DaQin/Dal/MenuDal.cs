using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBUtil;
using Models;

namespace Dal
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class MenuDal
    {
        #region 变量
        private SQLiteHelper sqliteHelper = new SQLiteHelper();
        #endregion

        #region 获取全部菜单
        /// <summary>
        /// 获取全部菜单
        /// </summary>
        public List<MenuModel> GetListAll()
        {
            return sqliteHelper.FindListBySql<MenuModel>(string.Format("select * from Sys_Menu order by Sort"));
        }
        #endregion

    }
}
