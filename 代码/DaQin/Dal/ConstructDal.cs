using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBUtil;
using Models;

namespace Dal
{
    /// <summary>
    /// 施工单
    /// </summary>
    public class ConstructDal
    {
        #region 变量
        private SQLiteHelper sqliteHelper = new SQLiteHelper();
        #endregion

        #region 获取列表
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<ConstructModel> GetList(ref PagerModel pager, string billCode, string startDate, string endDate, string cusName, string proMinister, string address)
        {
            StringBuilder sql = new StringBuilder(string.Format(@"
                select *
                from Pro_Construct 
                where 1=1 "));

            if (!string.IsNullOrWhiteSpace(startDate))
            {
                sql.AppendFormat(" and ConstructDate >= '{0}'", startDate);
            }

            if (!string.IsNullOrWhiteSpace(endDate))
            {
                sql.AppendFormat(" and ConstructDate <= '{0}'", endDate);
            }

            if (!string.IsNullOrWhiteSpace(cusName))
            {
                sql.AppendFormat(" and CusName like '%{0}%'", cusName);
            }

            if (!string.IsNullOrWhiteSpace(proMinister))
            {
                sql.AppendFormat(" and ProMinister like '%{0}%'", proMinister);
            }

            if (!string.IsNullOrWhiteSpace(address))
            {
                sql.AppendFormat(" and Address like '%{0}%'", address);
            }

            string orderby = string.Format("order by {0} {1}", pager.sort, pager.order);
            PagerModel pagerModel = sqliteHelper.FindPageBySql<ConstructModel>(sql.ToString(), orderby, pager.rows, pager.page);
            pager.totalRows = pagerModel.totalRows;
            pager.result = pagerModel.result;
            return pagerModel.result as List<ConstructModel>;
        }
        #endregion

        #region 添加或修改
        /// <summary>
        /// 添加或修改
        /// </summary>
        public void InsertOrUpdate(Pro_Construct construct, List<Pro_ConstructDet> constructDetList, List<Pro_ConstructAtt> constructAttList)
        {
            try
            {
                sqliteHelper.BeginTransaction();

                if (string.IsNullOrWhiteSpace(construct.Id))
                {
                    construct.Id = sqliteHelper.NewId();
                    sqliteHelper.Insert(construct);
                }
                else
                {
                    sqliteHelper.Update(construct);
                }

                constructDetList.ForEach(item => { item.ParentId = construct.Id; item.Id = sqliteHelper.NewId(); });
                sqliteHelper.Delete<Pro_ConstructDet>(string.Format(" ParentId='{0}' ", construct.Id));
                foreach (Pro_ConstructDet item in constructDetList)
                {
                    sqliteHelper.Insert(item);
                }

                constructAttList.ForEach(item => { item.ParentId = construct.Id; item.Id = sqliteHelper.NewId(); });
                sqliteHelper.Delete<Pro_ConstructAtt>(string.Format(" ParentId='{0}' ", construct.Id));
                foreach (Pro_ConstructAtt item in constructAttList)
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
        public ConstructModel Get(string id)
        {
            ConstructModel construct = sqliteHelper.FindBySql<ConstructModel>(string.Format("select * from Pro_Construct where Id='{0}'", id));

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

                sqliteHelper.BatchDelete<Pro_Construct>(ids);
                sqliteHelper.ExecuteSql(string.Format("delete from Pro_ConstructDet where ParentId in ({0})", ids));
                sqliteHelper.ExecuteSql(string.Format("delete from Pro_ConstructAtt where ParentId in ({0})", ids));

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
