using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBUtil;
using Models;

namespace Dal
{
    /// <summary>
    /// 施工单附件
    /// </summary>
    public class ConstructAttDal
    {
        #region 变量
        private SQLiteHelper sqliteHelper = new SQLiteHelper();
        #endregion

        #region 获取列表
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<ConstructAttModel> GetList(string parentId)
        {
            StringBuilder sql = new StringBuilder(string.Format(@"
                select *
                from Pro_ConstructAtt
                where ParentId='{0}'", parentId));

            List<ConstructAttModel> list = sqliteHelper.FindListBySql<ConstructAttModel>(sql.ToString());
            return list;
        }
        #endregion

    }
}
