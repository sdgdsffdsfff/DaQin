using DBUtil;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dal
{
    /// <summary>
    /// 单号
    /// </summary>
    public class BillCodeDal
    {
        #region 变量
        private static SQLiteHelper sqliteHelper = new SQLiteHelper();
        private static object _lock = new object();
        #endregion

        #region 生成单号
        /// <summary>
        /// 生成单号
        /// </summary>
        /// <param name="busiCode">业务代码(使用数据库表名)</param>
        public static string Create(string busiCode)
        {
            lock (_lock)
            {
                try
                {
                    Random rnd = new Random();
                    Pro_BillCode billCode = sqliteHelper.FindBySql<Pro_BillCode>(string.Format("select * from Pro_BillCode where BusiCode='{0}'", busiCode));
                    long newCode = long.Parse(billCode.BillCode) + rnd.Next(0, 10);
                    billCode.BillCode = newCode.ToString();

                    sqliteHelper.BeginTransaction();

                    sqliteHelper.Update(billCode);

                    sqliteHelper.CommitTransaction();

                    return billCode.BillCode;
                }
                catch (Exception ex)
                {
                    sqliteHelper.RollbackTransaction();
                    throw ex;
                }
            }
        }
        #endregion

    }
}
