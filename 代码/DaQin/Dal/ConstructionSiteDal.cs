using DBUtil;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dal
{
    /// <summary>
    /// 在建工地
    /// </summary>
    public class ConstructionSiteDal
    {
        #region 变量
        private SQLiteHelper sqliteHelper = new SQLiteHelper();
        #endregion

        #region 获取列表
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<ConstructionSiteModel> GetList(ref PagerModel pager, string address, string cusName, string designer, string proMinister, string checker, string startDateStart, string startDateEnd, string endDateStart, string endDateEnd)
        {
            StringBuilder sql = new StringBuilder(string.Format(@"
                select *
                from Pro_ConstructionSite 
                where 1=1 "));

            if (!string.IsNullOrWhiteSpace(address))
            {
                sql.AppendFormat(" and Address like '%{0}%'", address);
            }

            if (!string.IsNullOrWhiteSpace(cusName))
            {
                sql.AppendFormat(" and CusName like '%{0}%'", cusName);
            }

            if (!string.IsNullOrWhiteSpace(designer))
            {
                sql.AppendFormat(" and Designer like '%{0}%'", designer);
            }

            if (!string.IsNullOrWhiteSpace(proMinister))
            {
                sql.AppendFormat(" and ProMinister like '%{0}%'", proMinister);
            }

            if (!string.IsNullOrWhiteSpace(checker))
            {
                sql.AppendFormat(" and Checker like '%{0}%'", checker);
            }

            if (!string.IsNullOrWhiteSpace(startDateStart))
            {
                sql.AppendFormat(" and StartDate >= '{0}'", startDateStart);
            }

            if (!string.IsNullOrWhiteSpace(startDateEnd))
            {
                sql.AppendFormat(" and StartDate <= '{0}'", startDateEnd);
            }

            if (!string.IsNullOrWhiteSpace(endDateStart))
            {
                sql.AppendFormat(" and EndDate >= '{0}'", endDateStart);
            }

            if (!string.IsNullOrWhiteSpace(endDateEnd))
            {
                sql.AppendFormat(" and EndDate <= '{0}'", endDateEnd);
            }

            string orderby = string.Format("order by {0} {1}", pager.sort, pager.order);
            PagerModel pagerModel = sqliteHelper.FindPageBySql<ConstructionSiteModel>(sql.ToString(), orderby, pager.rows, pager.page);
            pager.totalRows = pagerModel.totalRows;
            pager.result = pagerModel.result;
            return pagerModel.result as List<ConstructionSiteModel>;
        }
        #endregion

        #region 添加或修改
        /// <summary>
        /// 添加或修改
        /// </summary>
        public void InsertOrUpdate(Pro_ConstructionSite constructionSite)
        {
            try
            {
                sqliteHelper.BeginTransaction();

                if (string.IsNullOrWhiteSpace(constructionSite.Id))
                {
                    constructionSite.Id = sqliteHelper.NewId();
                    sqliteHelper.Insert(constructionSite);
                }
                else
                {
                    sqliteHelper.Update(constructionSite);
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
        public ConstructionSiteModel Get(string id)
        {
            ConstructionSiteModel construct = sqliteHelper.FindBySql<ConstructionSiteModel>(string.Format("select * from Pro_ConstructionSite where Id='{0}'", id));

            return construct;
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

                sqliteHelper.BatchDelete<Pro_ConstructionSite>(ids);

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
