using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBUtil;
using Models;

namespace Dal
{
    /// <summary>
    /// 角色权限
    /// </summary>
    public class RoleFunctionDal
    {
        #region 变量
        private SQLiteHelper sqliteHelper = new SQLiteHelper();
        #endregion

        #region 获取全部角色权限
        /// <summary>
        /// 获取全部角色权限
        /// </summary>
        public List<RoleFunctionModel> GetListAll()
        {
            return sqliteHelper.FindListBySql<RoleFunctionModel>(string.Format("select * from Sys_RoleFunction"));
        }
        #endregion

        #region 删除
        private void Del(string roleId)
        {
            sqliteHelper.ExecuteSql(string.Format("delete from Sys_RoleFunction where RoleId='{0}'", roleId));
        }
        #endregion

        #region 添加或修改
        /// <summary>
        /// 添加或修改
        /// </summary>
        public void InsertOrUpdate(string roleId, string functionIds)
        {
            try
            {
                sqliteHelper.BeginTransaction();
                Del(roleId);
                if (!string.IsNullOrWhiteSpace(functionIds))
                {
                    string[] functionIdArr = functionIds.Split(',');
                    foreach (string functionId in functionIdArr)
                    {
                        Sys_RoleFunction item = new Sys_RoleFunction();
                        item.Id = sqliteHelper.NewId();
                        item.RoleId = roleId;
                        item.FunctionId = functionId;
                        sqliteHelper.Insert(item);
                    }
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

    }
}
