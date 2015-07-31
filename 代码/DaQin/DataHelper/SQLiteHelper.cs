using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.EntityClient;
using System.Data.Objects.DataClasses;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Models;

namespace DBUtil
{
    /// <summary>
    /// SQLite操作类
    /// 2015年6月20日
    /// 写程序之前，首先引用System.Data.SQLite;
    /// </summary>
    public class SQLiteHelper
    {
        #region 静态变量
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private static string connectionString;
        #endregion

        #region 成员变量
        /// <summary>
        /// 事务
        /// </summary>
        private SQLiteTransaction m_Transaction;
        /// <summary>
        /// 数据库连接
        /// </summary>
        private SQLiteConnection m_Connection;
        /// <summary>
        /// 开启事务标志
        /// </summary>
        private bool m_TranFlag;
        #endregion

        #region 初始化
        public static void Init()
        {
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        }
        #endregion

        #region 构造函数
        public SQLiteHelper()
        {
            m_Connection = new SQLiteConnection(connectionString);
        }
        #endregion

        #region 基础方法
        #region  执行简单SQL语句
        #region Exists
        public bool Exists(string sqlString)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(sqlString, m_Connection))
            {
                try
                {
                    m_Connection.Open();
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    m_Connection.Close();
                    throw ex;
                }
                finally
                {
                    cmd.Dispose();
                    m_Connection.Close();
                }
            }
        }
        #endregion

        #region 执行SQL语句，返回影响的记录数
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSql(string sqlString)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(sqlString, m_Connection))
            {
                try
                {
                    if (m_Connection.State != ConnectionState.Open) m_Connection.Open();
                    if (m_TranFlag) cmd.Transaction = m_Transaction;
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    cmd.Dispose();
                    if (!m_TranFlag) m_Connection.Close();
                }
            }
        }
        #endregion

        #region 执行一条计算查询结果语句，返回查询结果
        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）
        /// </summary>
        /// <param name="sqlString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public object GetSingle(string sqlString)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(sqlString, m_Connection))
            {
                try
                {
                    m_Connection.Open();
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (Exception ex)
                {
                    m_Connection.Close();
                    throw ex;
                }
                finally
                {
                    cmd.Dispose();
                    m_Connection.Close();
                }
            }
        }
        #endregion

        #region 执行查询语句，返回SQLiteDataReader
        /// <summary>
        /// 执行查询语句，返回SQLiteDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="sqlString">查询语句</param>
        /// <returns>SQLiteDataReader</returns>
        public SQLiteDataReader ExecuteReader(string sqlString)
        {
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            SQLiteCommand cmd = new SQLiteCommand(sqlString, connection);
            try
            {
                connection.Open();
                SQLiteDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 执行查询语句，返回DataSet
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="sqlString">查询语句</param>
        /// <returns>DataSet</returns>
        public DataSet Query(string sqlString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SQLiteDataAdapter command = new SQLiteDataAdapter(sqlString, connection);
                    command.Fill(ds, "ds");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
                return ds;
            }
        }
        #endregion
        #endregion

        #region 执行带参数的SQL语句
        #region 执行SQL语句，返回影响的记录数
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSql(string SQLString, params  SQLiteParameter[] cmdParms)
        {
            using (SQLiteCommand cmd = new SQLiteCommand())
            {
                try
                {
                    PrepareCommand(cmd, m_Connection, null, SQLString, cmdParms);
                    if (m_TranFlag) cmd.Transaction = m_Transaction;
                    int rows = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return rows;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    cmd.Dispose();
                    if (!m_TranFlag) m_Connection.Close();
                }
            }
        }
        #endregion

        #region 执行查询语句，返回SQLiteDataReader
        /// <summary>
        /// 执行查询语句，返回SQLiteDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SQLiteDataReader</returns>
        public SQLiteDataReader ExecuteReader(string sqlString, params SQLiteParameter[] cmdParms)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            try
            {
                PrepareCommand(cmd, m_Connection, null, sqlString, cmdParms);
                SQLiteDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region 执行查询语句，返回DataSet
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="sqlString">查询语句</param>
        /// <returns>DataSet</returns>
        public DataSet Query(string sqlString, params SQLiteParameter[] cmdParms)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            PrepareCommand(cmd, m_Connection, null, sqlString, cmdParms);
            using (SQLiteDataAdapter da = new SQLiteDataAdapter(cmd))
            {
                DataSet ds = new DataSet();
                try
                {
                    da.Fill(ds, "ds");
                    cmd.Parameters.Clear();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    cmd.Dispose();
                    m_Connection.Close();
                }
                return ds;
            }
        }
        #endregion

        #region PrepareCommand
        private void PrepareCommand(SQLiteCommand cmd, SQLiteConnection conn, SQLiteTransaction trans, string cmdText, SQLiteParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open) conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null) cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;
            if (cmdParms != null)
            {
                foreach (SQLiteParameter parm in cmdParms)
                {
                    cmd.Parameters.Add(parm);
                }
            }
        }
        #endregion
        #endregion
        #endregion

        #region 增删改查
        #region 新ID
        public string NewId()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
        #endregion

        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        public void Insert(object obj)
        {
            StringBuilder strSql = new StringBuilder();
            Type type = obj.GetType();
            strSql.Append(string.Format("insert into {0}(", type.Name));

            PropertyInfo[] propertyInfoList = GetEntityProperties(type);
            List<string> propertyNameList = new List<string>();
            foreach (PropertyInfo propertyInfo in propertyInfoList)
            {
                propertyNameList.Add(propertyInfo.Name);
            }

            strSql.Append(string.Format("{0})", string.Join(",", propertyNameList.ToArray())));
            strSql.Append(string.Format(" values ({0})", string.Join(",", propertyNameList.ConvertAll<string>(a => ":" + a).ToArray())));
            SQLiteParameter[] parameters = new SQLiteParameter[propertyInfoList.Length];
            for (int i = 0; i < propertyInfoList.Length; i++)
            {
                PropertyInfo propertyInfo = propertyInfoList[i];
                object val = propertyInfo.GetValue(obj, null);
                SQLiteParameter oracleParameter = new SQLiteParameter(":" + propertyInfo.Name, val == null ? DBNull.Value : val);
                parameters[i] = oracleParameter;
            }

            ExecuteSql(strSql.ToString(), parameters);
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        public void Update(object obj)
        {
            object oldObj = Find(obj);
            if (oldObj == null) throw new Exception("无法获取到旧数据");

            StringBuilder strSql = new StringBuilder();
            Type type = obj.GetType();
            strSql.Append(string.Format("update {0} ", type.Name));

            PropertyInfo[] propertyInfoList = GetEntityProperties(type);
            List<string> propertyNameList = new List<string>();
            int savedCount = 0;
            foreach (PropertyInfo propertyInfo in propertyInfoList)
            {
                object oldVal = propertyInfo.GetValue(oldObj, null);
                object val = propertyInfo.GetValue(obj, null);
                if (!object.Equals(oldVal, val))
                {
                    propertyNameList.Add(propertyInfo.Name);
                    savedCount++;
                }
            }

            strSql.Append(string.Format(" set "));
            SQLiteParameter[] parameters = new SQLiteParameter[savedCount];
            StringBuilder sbPros = new StringBuilder();
            int k = 0;
            for (int i = 0; i < propertyInfoList.Length; i++)
            {
                PropertyInfo propertyInfo = propertyInfoList[i];
                object oldVal = propertyInfo.GetValue(oldObj, null);
                object val = propertyInfo.GetValue(obj, null);
                if (!object.Equals(oldVal, val))
                {
                    sbPros.Append(string.Format(" {0}=:{0},", propertyInfo.Name));
                    SQLiteParameter oracleParameter = new SQLiteParameter(":" + propertyInfo.Name, val == null ? DBNull.Value : val);
                    parameters[k++] = oracleParameter;
                }
            }
            if (sbPros.Length > 0)
            {
                strSql.Append(sbPros.ToString(0, sbPros.Length - 1));
            }
            strSql.Append(string.Format(" where {0}='{1}'", GetIdName(obj.GetType()), GetIdVal(obj).ToString()));

            if (savedCount > 0)
            {
                ExecuteSql(strSql.ToString(), parameters);
            }
        }
        #endregion

        #region 删除
        /// <summary>
        /// 根据Id删除
        /// </summary>
        public void Delete<T>(int id)
        {
            Type type = typeof(T);
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(string.Format("delete from {0} where {2}='{1}'", type.Name, id, GetIdName(type)));

            ExecuteSql(sbSql.ToString());
        }
        /// <summary>
        /// 根据Id集合删除
        /// </summary>
        public void BatchDelete<T>(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids)) return;

            Type type = typeof(T);
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(string.Format("delete from {0} where {2} in ({1})", type.Name, ids, GetIdName(type)));

            ExecuteSql(sbSql.ToString());
        }
        /// <summary>
        /// 根据条件删除
        /// </summary>
        public void Delete<T>(string conditions)
        {
            if (string.IsNullOrWhiteSpace(conditions)) return;

            Type type = typeof(T);
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(string.Format("delete from {0} where {1}", type.Name, conditions));

            ExecuteSql(sbSql.ToString());
        }
        #endregion

        #region 获取实体
        #region 根据实体获取实体
        /// <summary>
        /// 根据实体获取实体
        /// </summary>
        private object Find(object obj)
        {
            Type type = obj.GetType();

            object result = Activator.CreateInstance(type);
            bool hasValue = false;
            IDataReader rd = null;

            string sql = string.Format("select * from {0} where {2}='{1}'", type.Name, GetIdVal(obj), GetIdName(obj.GetType()));

            try
            {
                rd = ExecuteReader(sql);

                PropertyInfo[] propertyInfoList = GetEntityProperties(type);

                int fcnt = rd.FieldCount;
                List<string> fileds = new List<string>();
                for (int i = 0; i < fcnt; i++)
                {
                    fileds.Add(rd.GetName(i).ToUpper());
                }

                while (rd.Read())
                {
                    hasValue = true;
                    IDataRecord record = rd;

                    foreach (PropertyInfo pro in propertyInfoList)
                    {
                        if (!fileds.Contains(pro.Name.ToUpper()) || record[pro.Name] == DBNull.Value)
                        {
                            continue;
                        }

                        pro.SetValue(result, record[pro.Name] == DBNull.Value ? null : getReaderValue(record[pro.Name], pro.PropertyType), null);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (rd != null && !rd.IsClosed)
                {
                    rd.Close();
                    rd.Dispose();
                }
            }

            if (hasValue)
            {
                return result;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 根据Id获取实体
        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        private object FindById(Type type, int id)
        {
            object result = Activator.CreateInstance(type);
            IDataReader rd = null;
            bool hasValue = false;

            string sql = string.Format("select * from {0} where {2}='{1}'", type.Name, id, GetIdName(type));

            try
            {
                rd = ExecuteReader(sql);

                PropertyInfo[] propertyInfoList = GetEntityProperties(type);

                int fcnt = rd.FieldCount;
                List<string> fileds = new List<string>();
                for (int i = 0; i < fcnt; i++)
                {
                    fileds.Add(rd.GetName(i).ToUpper());
                }

                while (rd.Read())
                {
                    hasValue = true;
                    IDataRecord record = rd;

                    foreach (PropertyInfo pro in propertyInfoList)
                    {
                        if (!fileds.Contains(pro.Name.ToUpper()) || record[pro.Name] == DBNull.Value)
                        {
                            continue;
                        }

                        pro.SetValue(result, record[pro.Name] == DBNull.Value ? null : getReaderValue(record[pro.Name], pro.PropertyType), null);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (rd != null && !rd.IsClosed)
                {
                    rd.Close();
                    rd.Dispose();
                }
            }

            if (hasValue)
            {
                return result;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 根据Id获取实体
        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        public T FindById<T>(string id) where T : new()
        {
            Type type = typeof(T);
            T result = (T)Activator.CreateInstance(type);
            IDataReader rd = null;
            bool hasValue = false;

            string sql = string.Format("select * from {0} where {2}='{1}'", type.Name, id, GetIdName(type));

            try
            {
                rd = ExecuteReader(sql);

                PropertyInfo[] propertyInfoList = GetEntityProperties(type);

                int fcnt = rd.FieldCount;
                List<string> fileds = new List<string>();
                for (int i = 0; i < fcnt; i++)
                {
                    fileds.Add(rd.GetName(i).ToUpper());
                }

                while (rd.Read())
                {
                    hasValue = true;
                    IDataRecord record = rd;

                    foreach (PropertyInfo pro in propertyInfoList)
                    {
                        if (!fileds.Contains(pro.Name.ToUpper()) || record[pro.Name] == DBNull.Value)
                        {
                            continue;
                        }

                        pro.SetValue(result, record[pro.Name] == DBNull.Value ? null : getReaderValue(record[pro.Name], pro.PropertyType), null);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (rd != null && !rd.IsClosed)
                {
                    rd.Close();
                    rd.Dispose();
                }
            }

            if (hasValue)
            {
                return result;
            }
            else
            {
                return default(T);
            }
        }
        #endregion

        #region 根据sql获取实体
        /// <summary>
        /// 根据sql获取实体
        /// </summary>
        public T FindBySql<T>(string sql) where T : new()
        {
            Type type = typeof(T);
            T result = (T)Activator.CreateInstance(type);
            IDataReader rd = null;
            bool hasValue = false;

            try
            {
                rd = ExecuteReader(sql);

                PropertyInfo[] propertyInfoList = GetEntityProperties(type);

                int fcnt = rd.FieldCount;
                List<string> fileds = new List<string>();
                for (int i = 0; i < fcnt; i++)
                {
                    fileds.Add(rd.GetName(i).ToUpper());
                }

                while (rd.Read())
                {
                    hasValue = true;
                    IDataRecord record = rd;

                    foreach (PropertyInfo pro in propertyInfoList)
                    {
                        if (!fileds.Contains(pro.Name.ToUpper()) || record[pro.Name] == DBNull.Value)
                        {
                            continue;
                        }

                        pro.SetValue(result, record[pro.Name] == DBNull.Value ? null : getReaderValue(record[pro.Name], pro.PropertyType), null);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (rd != null && !rd.IsClosed)
                {
                    rd.Close();
                    rd.Dispose();
                }
            }

            if (hasValue)
            {
                return result;
            }
            else
            {
                return default(T);
            }
        }
        #endregion
        #endregion

        #region 获取列表
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<T> FindListBySql<T>(string sql) where T : new()
        {
            List<T> list = new List<T>();
            object obj;
            IDataReader rd = null;

            try
            {
                rd = ExecuteReader(sql);

                if (typeof(T) == typeof(int))
                {
                    while (rd.Read())
                    {
                        list.Add((T)rd[0]);
                    }
                }
                else if (typeof(T) == typeof(string))
                {
                    while (rd.Read())
                    {
                        list.Add((T)rd[0]);
                    }
                }
                else
                {
                    PropertyInfo[] propertyInfoList = (typeof(T)).GetProperties();

                    int fcnt = rd.FieldCount;
                    List<string> fileds = new List<string>();
                    for (int i = 0; i < fcnt; i++)
                    {
                        fileds.Add(rd.GetName(i).ToUpper());
                    }

                    while (rd.Read())
                    {
                        IDataRecord record = rd;
                        obj = new T();


                        foreach (PropertyInfo pro in propertyInfoList)
                        {
                            if (!fileds.Contains(pro.Name.ToUpper()) || record[pro.Name] == DBNull.Value)
                            {
                                continue;
                            }

                            pro.SetValue(obj, record[pro.Name] == DBNull.Value ? null : getReaderValue(record[pro.Name], pro.PropertyType), null);
                        }
                        list.Add((T)obj);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (rd != null && !rd.IsClosed)
                {
                    rd.Close();
                    rd.Dispose();
                }
            }

            return list;
        }
        #endregion

        #region 获取列表
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<T> FindListBySql<T>(string sql, params SQLiteParameter[] cmdParms) where T : new()
        {
            List<T> list = new List<T>();
            object obj;
            IDataReader rd = null;

            try
            {
                rd = ExecuteReader(sql, cmdParms);

                if (typeof(T) == typeof(int))
                {
                    while (rd.Read())
                    {
                        list.Add((T)rd[0]);
                    }
                }
                else if (typeof(T) == typeof(string))
                {
                    while (rd.Read())
                    {
                        list.Add((T)rd[0]);
                    }
                }
                else
                {
                    PropertyInfo[] propertyInfoList = (typeof(T)).GetProperties();

                    int fcnt = rd.FieldCount;
                    List<string> fileds = new List<string>();
                    for (int i = 0; i < fcnt; i++)
                    {
                        fileds.Add(rd.GetName(i).ToUpper());
                    }

                    while (rd.Read())
                    {
                        IDataRecord record = rd;
                        obj = new T();


                        foreach (PropertyInfo pro in propertyInfoList)
                        {
                            if (!fileds.Contains(pro.Name.ToUpper()) || record[pro.Name] == DBNull.Value)
                            {
                                continue;
                            }

                            pro.SetValue(obj, record[pro.Name] == DBNull.Value ? null : getReaderValue(record[pro.Name], pro.PropertyType), null);
                        }
                        list.Add((T)obj);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (rd != null && !rd.IsClosed)
                {
                    rd.Close();
                    rd.Dispose();
                }
            }

            return list;
        }
        #endregion

        #region 分页获取列表
        /// <summary>
        /// 分页(任意entity，尽量少的字段)
        /// </summary>
        public PagerModel FindPageBySql<T>(string sql, string orderby, int pageSize, int currentPage) where T : new()
        {
            PagerModel pageViewModel = new PagerModel();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string commandText = string.Format("select count(*) from ({0}) T", sql);
                IDbCommand cmd = new SQLiteCommand(commandText, connection);
                pageViewModel.totalRows = int.Parse(cmd.ExecuteScalar().ToString());

                int startRow = pageSize * (currentPage - 1);

                StringBuilder sb = new StringBuilder();
                sb.Append(sql);
                if (!string.IsNullOrWhiteSpace(orderby))
                {
                    sb.Append(" ");
                    sb.Append(orderby);
                }
                sb.AppendFormat(" limit {0} offset {1}", pageSize, startRow);

                List<T> list = FindListBySql<T>(sb.ToString());
                pageViewModel.result = list;
            }

            return pageViewModel;
        }
        #endregion

        #region 分页获取列表
        /// <summary>
        /// 分页(任意entity，尽量少的字段)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public PagerModel FindPageBySql<T>(string sql, string orderby, int pageSize, int currentPage, params SQLiteParameter[] cmdParms) where T : new()
        {
            PagerModel pageViewModel = new PagerModel();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string commandText = string.Format("select count(*) from ({0}) T", sql);
                SQLiteCommand cmd = new SQLiteCommand(commandText, connection);
                PrepareCommand(cmd, connection, null, commandText, cmdParms);
                pageViewModel.totalRows = int.Parse(cmd.ExecuteScalar().ToString());
                cmd.Parameters.Clear();

                int startRow = pageSize * (currentPage - 1);

                StringBuilder sb = new StringBuilder();
                sb.Append(sql);
                if (!string.IsNullOrWhiteSpace(orderby))
                {
                    sb.Append(" ");
                    sb.Append(orderby);
                }
                sb.AppendFormat(" limit {0} offset {1}", pageSize, startRow);

                List<T> list = FindListBySql<T>(sb.ToString(), cmdParms);
                pageViewModel.result = list;
            }

            return pageViewModel;
        }


        #endregion

        #region 分页获取列表
        /// <summary>
        /// 分页(任意entity，尽量少的字段)
        /// </summary>
        public DataSet FindPageBySql(string sql, string orderby, int pageSize, int currentPage, out int totalCount, params SQLiteParameter[] cmdParms)
        {
            DataSet ds = null;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string commandText = string.Format("select count(*) from ({0}) T", sql);
                IDbCommand cmd = new SQLiteCommand(commandText, connection);
                totalCount = int.Parse(cmd.ExecuteScalar().ToString());

                int startRow = pageSize * (currentPage - 1);

                StringBuilder sb = new StringBuilder();
                sb.Append(sql);
                if (!string.IsNullOrWhiteSpace(orderby))
                {
                    sb.Append(" ");
                    sb.Append(orderby);
                }
                sb.AppendFormat(" limit {0} offset {1}", pageSize, startRow);

                ds = Query(sb.ToString(), cmdParms);
            }

            return ds;
        }
        #endregion

        #region getReaderValue 转换数据
        /// <summary>
        /// 转换数据
        /// </summary>
        private Object getReaderValue(Object rdValue, Type ptype)
        {

            if (ptype == typeof(decimal))
                return Convert.ToDecimal(rdValue);

            if (ptype == typeof(int))
                return Convert.ToInt32(rdValue);

            if (ptype == typeof(long))
                return Convert.ToInt64(rdValue);

            return rdValue;
        }
        #endregion

        #region 获取主键名称
        /// <summary>
        /// 获取主键名称
        /// </summary>
        public string GetIdName(Type type)
        {
            PropertyInfo[] propertyInfoList = GetEntityProperties(type);
            foreach (PropertyInfo propertyInfo in propertyInfoList)
            {
                if (propertyInfo.GetCustomAttributes(typeof(IsIdAttribute), false).Length > 0)
                {
                    return propertyInfo.Name;
                }
            }
            return "Id";
        }
        #endregion

        #region 获取主键值
        /// <summary>
        /// 获取主键名称
        /// </summary>
        public object GetIdVal(object val)
        {
            string idName = GetIdName(val.GetType());
            if (!string.IsNullOrWhiteSpace(idName))
            {
                return val.GetType().GetProperty(idName).GetValue(val, null);
            }
            return 0;
        }
        #endregion

        #region 获取实体类属性
        /// <summary>
        /// 获取实体类属性
        /// </summary>
        private PropertyInfo[] GetEntityProperties(Type type)
        {
            List<PropertyInfo> result = new List<PropertyInfo>();
            PropertyInfo[] propertyInfoList = type.GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfoList)
            {
                if (propertyInfo.GetCustomAttributes(typeof(EdmRelationshipNavigationPropertyAttribute), false).Length == 0
                    && propertyInfo.GetCustomAttributes(typeof(BrowsableAttribute), false).Length == 0)
                {
                    result.Add(propertyInfo);
                }
            }
            return result.ToArray();
        }
        #endregion
        #endregion

        #region 事务
        #region 开始事务
        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {
            m_TranFlag = true;
            if (m_Connection.State != ConnectionState.Open) m_Connection.Open();
            m_Transaction = m_Connection.BeginTransaction();
        }
        #endregion

        #region 提交事务
        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTransaction()
        {
            m_Transaction.Commit();
            m_TranFlag = false;
            m_Connection.Close();
        }
        #endregion

        #region 回滚事务
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollbackTransaction()
        {
            try
            {
                m_Transaction.Rollback();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                m_TranFlag = false;
                m_Connection.Close();
            }
        }
        #endregion
        #endregion

    }
}