using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBUtil;
using Models;

namespace Dal
{
    /// <summary>
    /// 施工单明细
    /// </summary>
    public class ConstructDetDal
    {
        #region 变量
        private SQLiteHelper sqliteHelper = new SQLiteHelper();
        #endregion

        #region 获取列表
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<ConstructDetModel> GetList(string parentId)
        {
            StringBuilder sql = new StringBuilder(string.Format(@"
                select *
                from Pro_ConstructDet
                where ParentId='{0}'", parentId));

            List<ConstructDetModel> list = sqliteHelper.FindListBySql<ConstructDetModel>(sql.ToString());
            return list;
        }
        #endregion

    }
}
