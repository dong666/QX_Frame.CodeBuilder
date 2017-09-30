using QX_Frame.Bantina;
using QX_Frame.Bantina.Options;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CSharp_FlowchartToCode_DG
{

    public partial class MicrosoftSqlServerLoginForm : Form
    {
        MainForm mainForm;
        public MicrosoftSqlServerLoginForm()
        {
            InitializeComponent();
        }
        public MicrosoftSqlServerLoginForm(MainForm form)
        {
            InitializeComponent();
            this.mainForm = form;
            //CheckConnectTypeChoose
            CheckConnectTypeChoose();
            //init configuration
            ReadConfiguration();
        }

        //cancel button
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
        //connect button
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                mainForm.treeView1.Nodes[0].Nodes.Clear();//clear nodes
                GetDataBaseInfo();
                WriteConfiguration();
                this.Close();
                this.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ReadConfiguration()
        {
            string[] serverNameArray = IO_Helper_DG.Ini_SelectStringValue(CommonVariables.configFilePath, "sqlserver", "ServerName", ".").Split(',');
            comboBox2.Text = serverNameArray[0];
            comboBox2.Items.Clear();
            foreach (var item in serverNameArray)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    comboBox2.Items.Add(item);
                }
            }
            comboBox3.Text = IO_Helper_DG.Ini_SelectStringValue(CommonVariables.configFilePath, "sqlserver", "Authentication", "Windows Authentication");
            string[] saLoginArray = IO_Helper_DG.Ini_SelectStringValue(CommonVariables.configFilePath, "sqlserver", "SaLogin", "sa").Split(',');
            comboBox4.Items.Clear();
            foreach (var item in saLoginArray)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    comboBox4.Items.Add(item);
                }
            }
            if (comboBox3.Text.Equals("SQL Server Authentication"))
            {
                comboBox4.Text = saLoginArray[0];
                textBox1.Text = IO_Helper_DG.Ini_SelectStringValue(CommonVariables.configFilePath, "sqlserver", "SaPassword", "123456");
            }
        }
        private void WriteConfiguration()
        {
            //保存5条信息，默认取第一条，新登录的顶到第一条，删除最后一条，逗号分隔 1,2,3,'',''
            string[] serverNameArray = IO_Helper_DG.Ini_SelectStringValue(CommonVariables.configFilePath, "sqlserver", "ServerName", ".").Split(',');
            string[] newServerNameArray = new string[] { comboBox2.Text, "", "", "", "" };//save 5 lines
            int newServerNameIndex = 1;
            foreach (var item in serverNameArray)
            {
                if (!newServerNameArray.Contains(item))
                {
                    newServerNameArray[newServerNameIndex] = item;
                    newServerNameIndex++;
                }
            }
            IO_Helper_DG.Ini_Update(CommonVariables.configFilePath, "sqlserver", "ServerName", string.Join(",", newServerNameArray));
            IO_Helper_DG.Ini_Update(CommonVariables.configFilePath, "sqlserver", "Authentication", comboBox3.Text);
            if (checkBox1.Checked)
            {
                string[] saLoginArray = IO_Helper_DG.Ini_SelectStringValue(CommonVariables.configFilePath, "sqlserver", "SaLogin", ".").Split(',');
                string[] newSaLoginArray = new string[] { comboBox4.Text, "", "", "", "" };//save 5 lines
                int newsaLoginIndex = 1;
                foreach (var item in saLoginArray)
                {
                    if (!newSaLoginArray.Contains(item))
                    {
                        newSaLoginArray[newsaLoginIndex] = item;
                        newsaLoginIndex++;
                    }
                }
                IO_Helper_DG.Ini_Update(CommonVariables.configFilePath, "sqlserver", "SaLogin", string.Join(",", newSaLoginArray));
                IO_Helper_DG.Ini_Update(CommonVariables.configFilePath, "sqlserver", "SaPassword", textBox1.Text);
            }
        }
        //get database info fill mainForm.TreeView1
        private void GetDataBaseInfo()
        {
            if (comboBox3.Text.Equals("Windows Authentication"))
            {
                CommonVariables.SetCurrentDbConnection($"Data Source={comboBox2.Text.Trim()};Initial Catalog=master;Integrated Security = True", Opt_DataBaseType.SqlServer);
            }
            else if (comboBox3.Text.Equals("SQL Server Authentication"))
            {
                CommonVariables.SetCurrentDbConnection($"Data Source={comboBox2.Text.Trim()};Initial Catalog=master;Persist Security Info=True; User ID={comboBox4.Text.Trim()};Password={textBox1.Text.Trim()};", Opt_DataBaseType.SqlServer);
            }

            string sql = "select name from sys.databases where database_id > 4";//查询sqlserver中的非系统库

            DataTable dataTable = Db_Helper_DG.ExecuteDataTable(sql);

            TreeNode grand = new TreeNode(comboBox2.Text.Trim());//添加节点服务器地址
            grand.ImageIndex = 1;
            mainForm.treeView1.Nodes[0].Nodes.Add(grand);

            mainForm.comboBox3.Items.Clear();//清空SqlPower里面的数据库下拉框数据

            foreach (DataRow row in dataTable.Rows)
            {
                string dbName = row["name"].ToString();
                TreeNode root = new TreeNode(dbName);//创建节点
                root.Name = dbName;
                root.ImageIndex = 2;
                grand.Nodes.Add(root);

                TreeNode biao = new TreeNode("Tables");
                biao.Name = "Tables";
                biao.ImageIndex = 3;
                root.Nodes.Add(biao);

                mainForm.comboBox3.Items.Add(dbName);//添加SqlPower里面的数据库下拉框数据

                //获取表名
                string sqlTable = $"use [{root.Name}] SELECT name FROM sysobjects WHERE xtype = 'U' AND OBJECTPROPERTY (id, 'IsMSShipped') = 0 and name <> 'sysdiagrams'";
                DataTable dt2 = Db_Helper_DG.ExecuteDataTable(sqlTable);
                string[] tableNameArray = new string[dt2.Rows.Count];
                foreach (DataRow row2 in dt2.Rows)
                {
                    TreeNode biaovalue = new TreeNode(row2[0].ToString());
                    biaovalue.Name = row2[0].ToString();
                    biaovalue.ImageIndex = 4;
                    biao.Nodes.Add(biaovalue);
                }
            }

            //Nodes Expand
            grand.Expand();
            mainForm.treeView1.Nodes[0].Expand();
        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e) => CheckConnectTypeChoose();
        private void CheckConnectTypeChoose()
        {
            if (comboBox3.Text.Equals("Windows Authentication"))
            {
                this.comboBox4.Enabled = false;
                this.textBox1.Enabled = false;
                this.checkBox1.Checked = false;
                this.checkBox1.Enabled = false;
            }
            else if (comboBox3.Text.Equals("SQL Server Authentication"))
            {
                this.comboBox4.Enabled = true;
                this.textBox1.Enabled = true;
                this.checkBox1.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Thanks for Using Microsoft SQL Server 2070");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("No More Configuration");
        }
    }
}
