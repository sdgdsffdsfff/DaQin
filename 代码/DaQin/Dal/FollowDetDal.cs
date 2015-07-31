using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBUtil;
using Models;

namespace Dal
{
    /// <summary>
    /// 跟踪表明细
    /// </summary>
    public class FollowDetDal
    {
        #region 变量
        private SQLiteHelper sqliteHelper = new SQLiteHelper();
        #endregion

        #region 获取列表
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<FollowDetModel> GetList(string parentId)
        {
            StringBuilder sql = new StringBuilder(string.Format(@"
                select *
                from Pro_FollowDet
                where ParentId='{0}'", parentId));

            List<FollowDetModel> list = sqliteHelper.FindListBySql<FollowDetModel>(sql.ToString());
            return list;
        }
        #endregion

    }
}
