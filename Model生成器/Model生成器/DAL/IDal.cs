using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    /// <summary>
    /// 数据库操作接口
    /// </summary>
    public interface IDal
    {
        /// <summary>
        /// 获取数据库名
        /// </summary>
        List<string> GetAllTableName();
        /// <summary>
        /// 获取表的所有字段名及字段类型
        /// </summary>
        List<Dictionary<string, string>> GetAllColumns(string tableName);
    }
}
