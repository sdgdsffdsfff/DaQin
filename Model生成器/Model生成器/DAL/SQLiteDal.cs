using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBUtil;

namespace DAL
{
    /// <summary>
    /// SQLite数据库DAL
    /// </summary>
    public class SQLiteDal : IDal
    {
        #region 获取所有表名
        /// <summary>
        /// 获取数据库名
        /// </summary>
        public List<string> GetAllTableName()
        {
            SQLiteHelper sqliteHelper = new SQLiteHelper();
            DataTable dt = sqliteHelper.Query("select tbl_name from sqlite_master where type='table'");
            List<string> result = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                result.Add(dr["tbl_name"].ToString());
            }
            return result;
        }
        #endregion

        #region 获取表的所有字段名及字段类型
        /// <summary>
        /// 获取表的所有字段名及字段类型
        /// </summary>
        public List<Dictionary<string, string>> GetAllColumns(string tableName)
        {
            SQLiteHelper sqliteHelper = new SQLiteHelper();
            DataTable dt = sqliteHelper.Query("PRAGMA table_info('" + tableName + "')");
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("name", dr["name"].ToString());
                dic.Add("notnull", dr["notnull"].ToString());
                result.Add(dic);
            }
            return result;
        }
        #endregion

    }
}
