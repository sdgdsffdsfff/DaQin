using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;
using DBUtil;
using Utils;

namespace Model生成器
{
    public partial class Form1 : Form
    {
        #region Form1
        public Form1()
        {
            InitializeComponent();
        }
        #endregion

        #region Form1_Load
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        #endregion

        //生成
        private void btnCreate_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(delegate()
            {
                IDal dal = DalFactory.CreateDal(ConfigurationManager.AppSettings["DBType"]);
                List<string> tableNameList = dal.GetAllTableName();

                #region 操作控件
                InvokeDelegate invokeDelegate = delegate()
                {
                    btnCreate.Enabled = false;
                    progressBar1.Visible = true;
                    progressBar1.Minimum = 0;
                    progressBar1.Maximum = tableNameList.Count;
                    progressBar1.Value = 0;
                };
                InvokeUtil.Invoke(this, invokeDelegate);
                #endregion

                int i = 0;
                foreach (string tableName in tableNameList)
                {
                    StringBuilder sb = new StringBuilder();
                    StringBuilder sbExt = new StringBuilder();
                    List<Dictionary<string, string>> columnList = dal.GetAllColumns(tableName);

                    #region 原始Model
                    sb.Append("using System;\r\n");
                    sb.Append("using System.Collections.Generic;\r\n");
                    sb.Append("using System.Linq;\r\n");
                    sb.Append("\r\n");
                    sb.Append("namespace Models\r\n");
                    sb.Append("{\r\n");
                    sb.Append("    public class " + tableName + "\r\n");
                    sb.Append("    {\r\n");
                    foreach (Dictionary<string, string> column in columnList)
                    {
                        sb.Append("        public string " + column["name"] + " { get; set; }\r\n");
                    }
                    sb.Append("    }\r\n");
                    sb.Append("}\r\n");
                    FileHelper.WriteFile(Application.StartupPath + "\\models", sb.ToString(), tableName);
                    #endregion

                    #region 扩展Model
                    int pos = tableName.IndexOf("_");
                    string extName = tableName.Substring(pos + 1) + "Model";
                    sbExt.Append("using System;\r\n");
                    sbExt.Append("using System.Collections.Generic;\r\n");
                    sbExt.Append("using System.Linq;\r\n");
                    sbExt.Append("\r\n");
                    sbExt.Append("namespace Models\r\n");
                    sbExt.Append("{\r\n");
                    sbExt.Append("    public class " + extName + " : " + tableName + "\r\n");
                    sbExt.Append("    {\r\n");
                    sbExt.Append("\r\n");
                    sbExt.Append("    }\r\n");
                    sbExt.Append("}\r\n");
                    FileHelper.WriteFile(Application.StartupPath + "\\extmodels", sbExt.ToString(), extName);
                    #endregion

                    #region 操作控件
                    invokeDelegate = delegate()
                    {
                        progressBar1.Value = ++i;
                    };
                    InvokeUtil.Invoke(this, invokeDelegate);
                    #endregion
                }

                #region 操作控件
                invokeDelegate = delegate()
                {
                    btnCreate.Enabled = true;
                    progressBar1.Visible = false;
                    progressBar1.Value = 0;
                };
                InvokeUtil.Invoke(this, invokeDelegate);
                #endregion

                MessageBox.Show("完成");
            })).Start();
        }
    }
}
