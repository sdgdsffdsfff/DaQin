using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBUtil;
using Models;

namespace Dal
{
    /// <summary>
    /// 角色
    /// </summary>
    public class RoleDal
    {
        #region 变量
        private SQLiteHelper sqliteHelper = new SQLiteHelper();
        #endregion

        #region 获取集合
        /// <summary>
        /// 获取集合
        /// </summary>
        public List<RoleModel> GetList(ref PagerModel pager)
        {
            StringBuilder sql = new StringBuilder(string.Format(@"
                select role.*
                from Sys_Role role
                where 1=1 "));

            string orderby = string.Format("order by {0} {1}", pager.sort, pager.order);
            PagerModel pagerModel = sqliteHelper.FindPageBySql<RoleModel>(sql.ToString(), orderby, pager.rows, pager.page);
            pager.totalRows = pagerModel.totalRows;
            pager.result = pagerModel.result;
            return pagerModel.result as List<RoleModel>;
        }
        #endregion

        #region 获取全部角色
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<RoleModel> GetListAll()
        {
            StringBuilder sql = new StringBuilder(string.Format(@"
                select *
                from Sys_Role"));

            List<RoleModel> list = sqliteHelper.FindListBySql<RoleModel>(sql.ToString());
            return list;
        }
        #endregion

    }
}
