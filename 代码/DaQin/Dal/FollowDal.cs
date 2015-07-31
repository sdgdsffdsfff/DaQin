using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBUtil;
using Models;

namespace Dal
{
    /// <summary>
    /// 跟踪表
    /// </summary>
    public class FollowDal
    {
        #region 变量
        private SQLiteHelper sqliteHelper = new SQLiteHelper();
        #endregion

        #region 获取列表
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<FollowModel> GetList(ref PagerModel pager, string startDate, string endDate, string address, string designer, string adviser, string follower)
        {
            StringBuilder sql = new StringBuilder(string.Format(@"
                select *
                from Pro_Follow 
                where 1=1 "));

            if (!string.IsNullOrWhiteSpace(startDate))
            {
                sql.AppendFormat(" and FollowDate >= '{0}'", startDate);
            }

            if (!string.IsNullOrWhiteSpace(endDate))
            {
                sql.AppendFormat(" and FollowDate <= '{0}'", endDate);
            }

            if (!string.IsNullOrWhiteSpace(address))
            {
                sql.AppendFormat(" and Address like '%{0}%'", address);
            }

            if (!string.IsNullOrWhiteSpace(designer))
            {
                sql.AppendFormat(" and Designer like '%{0}%'", designer);
            }

            if (!string.IsNullOrWhiteSpace(adviser))
            {
                sql.AppendFormat(" and Adviser like '%{0}%'", adviser);
            }

            if (!string.IsNullOrWhiteSpace(follower))
            {
                sql.AppendFormat(" and Follower like '%{0}%'", follower);
            }

            string orderby = string.Format("order by {0} {1}", pager.sort, pager.order);
            PagerModel pagerModel = sqliteHelper.FindPageBySql<FollowModel>(sql.ToString(), orderby, pager.rows, pager.page);
            pager.totalRows = pagerModel.totalRows;
            pager.result = pagerModel.result;
            return pagerModel.result as List<FollowModel>;
        }
        #endregion

        #region 添加或修改
        /// <summary>
        /// 添加或修改
        /// </summary>
        public void InsertOrUpdate(Pro_Follow follow, List<Pro_FollowDet> followDetList)
        {
            try
            {
                sqliteHelper.BeginTransaction();

                if (string.IsNullOrWhiteSpace(follow.Id))
                {
                    follow.Id = sqliteHelper.NewId();
                    sqliteHelper.Insert(follow);
                }
                else
                {
                    sqliteHelper.Update(follow);
                }

                followDetList.ForEach(item => { item.ParentId = follow.Id; item.Id = sqliteHelper.NewId(); });
                sqliteHelper.Delete<Pro_FollowDet>(string.Format(" ParentId='{0}' ", follow.Id));
                foreach (Pro_FollowDet item in followDetList)
                {
                    sqliteHelper.Insert(item);
                }

                sqliteHelper.CommitTransaction();
            }
            catch (Exception ex)
            {
                sqliteHelper.RollbackTransaction();
                throw ex;
            }
        }
        #endregion

        #region 获取
        /// <summary>
        /// 获取
        /// </summary>
        public FollowModel Get(string id)
        {
            FollowModel follow = sqliteHelper.FindBySql<FollowModel>(string.Format("select * from Pro_Follow where Id='{0}'", id));

            return follow;
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        public void Del(string ids)
        {
            try
            {
                sqliteHelper.BeginTransaction();

                sqliteHelper.BatchDelete<Pro_Follow>(ids);
                sqliteHelper.ExecuteSql(string.Format("delete from Pro_FollowDet where ParentId in ({0})", ids));

                sqliteHelper.CommitTransaction();
            }
            catch (Exception ex)
            {
                sqliteHelper.RollbackTransaction();
                throw ex;
            }
        }
        #endregion

    }
}
