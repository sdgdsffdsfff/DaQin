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
    /// MySql数据库DAL
    /// </summary>
    public class MySqlDal : IDal
    {
        #region 获取所有表名
        /// <summary>
        /// 获取数据库名
        /// </summary>
        public List<string> GetAllTableName()
        {
            return new List<string>();
        }
        #endregion

        #region 获取表的所有字段名及字段类型
        /// <summary>
        /// 获取表的所有字段名及字段类型
        /// </summary>
        public List<Dictionary<string, string>> GetAllColumns(string tableName)
        {
            return new List<Dictionary<string, string>>();
        }
        #endregion

    }
}
