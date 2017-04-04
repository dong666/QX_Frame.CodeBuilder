using CSharp_FlowchartToCode_DG.CodeCreate;
using QX_Frame.Helper_DG_Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;


namespace CSharp_FlowchartToCode_DG
{
    public partial class MainForm : Form
    {
        #region 全局变量
        //全局变量

        public static Dictionary<string, dynamic> CreateInfoDic = new Dictionary<string, dynamic>();         //存储全部信息的List

        public List<string> FeildName = new List<string>();          //表字段名称
        public List<string> FeildType = new List<string>();          //表字段类型
        public List<string> FeildIsNullable = new List<string>();    //表字段可空
        public List<string> FeildLength = new List<string>();        //表字段长度
        public List<string> FeildDescription = new List<string>();   //表字段说明
        public List<string> FeildIsPK = new List<string>();          //表字段是否主键
        public List<string> FeildIsIdentity = new List<string>();    //表字段是否自增


        string CodeTxt = "";        //代码字符串，用于输出到文件
        string dir = Path_Helper_DG.DeskTopPath;                  //获取路径
        string filePath = @"qixiaoSrc\QixiaoConfig.ini";          //获取配置文件的路径

        #endregion

        public MainForm()
        {
            InitializeComponent();

        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                // set copyright
                label_Author.Text += Info.Author;
                label_Version.Text += Info.VersionNum;
                label_Description.Text += Info.Description;

                //set config
                textBox1.Text = Ini_Helper_DG.selectStringValue(filePath, "config", "dataSource", ".");//data source
                comboBox1.Text = Ini_Helper_DG.selectStringValue(filePath, "config", "loginType", "Integrated Security=True");// system or sa ?
                comboBox2.Text = Ini_Helper_DG.selectStringValue(filePath, "config", "outputType");//output type

                //set code builder config
                textBox3.Text = Ini_Helper_DG.selectStringValue(filePath, "code", "using"); //using
                textBox2.Text = Ini_Helper_DG.selectStringValue(filePath, "code", "namespace");//namespace
                textBox9.Text = Ini_Helper_DG.selectStringValue(filePath, "code", "TableName");//table name
                textBox5.Text = Ini_Helper_DG.selectStringValue(filePath, "code", "class");//class name 
                textBox7.Text = Ini_Helper_DG.selectStringValue(filePath, "code", "ClassNamePlus");//ClassExtends
                textBox8.Text = Ini_Helper_DG.selectStringValue(filePath, "code", "ClassExtends");//ClassExtends
                textBox6.Text = dir + "\\" + Ini_Helper_DG.selectStringValue(filePath, "code", "filePathRelativeDeskTop") + "\\";//filePath
                textBox4.Text = Ini_Helper_DG.selectStringValue(filePath, "code", "fileName");//fileName

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //write ini config record the opration history
        private void setInitConfigFile()
        {
            //set config
            Ini_Helper_DG.Update(filePath, "config", "dataSource", textBox1.Text);//data source
            Ini_Helper_DG.Update(filePath, "config", "loginType", comboBox1.Text);// system or sa ?
            Ini_Helper_DG.Update(filePath, "config", "outputType", comboBox2.Text);//output type

            //set code builder config
            Ini_Helper_DG.Update(filePath, "code", "using", textBox3.Text); //using
            Ini_Helper_DG.Update(filePath, "code", "namespace", textBox2.Text);//namespace
            Ini_Helper_DG.Update(filePath, "code", "TableName", textBox9.Text);//table name
            Ini_Helper_DG.Update(filePath, "code", "class", textBox5.Text);//class name
            Ini_Helper_DG.Update(filePath, "code", "ClassNamePlus", textBox7.Text);// ClassExtends
            Ini_Helper_DG.Update(filePath, "code", "ClassExtends", textBox8.Text);// ClassExtends
            Ini_Helper_DG.Update(filePath, "code", "fileName", textBox4.Text);//fileName
        }

        #region 获取数据库结构的代码
        //获取数据库信息
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "select name from sys.databases where database_id > 4";//查询sqlserver中的非系统库
                string ConnectionStr = $"Data Source={textBox1.Text.Trim()};Initial Catalog=master;{comboBox1.Text.Trim()};";
                DataSet ds = SqlHelper.ExecuteDataSet(ConnectionStr, sql);
                DataTable dataTable = ds.Tables[0];

                TreeNode grand = new TreeNode(textBox1.Text.Trim());//添加节点服务器地址
                grand.ImageIndex = 1;
                treeView1.Nodes[0].Nodes.Add(grand);

                foreach (DataRow row in dataTable.Rows)
                {
                    TreeNode root = new TreeNode(row["name"].ToString());//创建节点
                    root.Name = row["name"].ToString();
                    root.ImageIndex = 2;
                    grand.Nodes.Add(root);
                    TreeNode biao = new TreeNode("Tables");
                    biao.Name = "Tables";
                    biao.ImageIndex = 3;
                    root.Nodes.Add(biao);


                    //获取表名
                    string sqltable = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
                    DataSet ds2 = SqlHelper.ExecuteDataSet("Data Source=" + textBox1.Text.Trim() + ";Initial Catalog=" + row["name"] + ";Integrated Security=True", sqltable);
                    DataTable dt2 = ds2.Tables[0];
                    List<string> tableNameList = new List<string>();
                    foreach (DataRow row2 in dt2.Rows)
                    {
                        tableNameList.Add(row2[0].ToString());
                    }
                    tableNameList.Sort();// sort the list
                    foreach (var item in tableNameList)
                    {
                        TreeNode biaovalue = new TreeNode(item);
                        biaovalue.Name = item;
                        biaovalue.ImageIndex = 4;
                        biao.Nodes.Add(biaovalue);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            getTableInfo();
        }
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            getTableInfo();
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            getTableInfo();
        }
        //获取数据库表信息
        public void getTableInfo()
        {
            try
            {
                string database = this.treeView1.SelectedNode.Parent.Parent.Name;
                string table = this.treeView1.SelectedNode.Name;

                textBox5.Text = table;//将table的表名赋值给TableName变量，方便后续传值; Model
                textBox9.Text = table;//将table的表名赋值给TableName变量，方便后续传值; Model
                textBox4.Text = table + textBox7.Text.Trim() + ".cs";//fileName

                string connStr = "Data Source=" + textBox1.Text.Trim() + ";Initial Catalog=" + database + ";Integrated Security=True";

                string sql = @"select syscolumns.name as Field ,systypes.name as FieldType , syscolumns.length as Length,syscolumns.isnullable as Nullable, sys.extended_properties.value as Description  ,IsPK = Case  when exists ( select 1 from sysobjects  inner join sysindexes  on sysindexes.name = sysobjects.name  inner join sysindexkeys  on sysindexes.id = sysindexkeys.id  and  sysindexes.indid = sysindexkeys.indid  where xtype='PK'  and parent_obj = syscolumns.id and sysindexkeys.colid = syscolumns.colid ) then 1 else 0 end ,IsIdentity = Case syscolumns.status when 128 then 1 else 0 end  from syscolumns inner join systypes on(  syscolumns.xtype = systypes.xtype and systypes.name <>'_default_' and systypes.name<>'sysname'  ) left outer join sys.extended_properties on  ( sys.extended_properties.major_id=syscolumns.id and minor_id=syscolumns.colid  ) where syscolumns.id = (select id from sysobjects where name='" + table + @"') order by syscolumns.colid ";

                DataSet ds = SqlHelper.ExecuteDataSet(connStr, sql);

                DataTable dt = ds.Tables[0];
                this.dataGridView1.DataSource = dt.DefaultView;
                //设置初始值为全选中
                for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                {
                    this.dataGridView1.Rows[i].Cells[0].Value = true;
                }
            }
            catch (Exception)
            {
                //throw;
                //MessageBox.Show("无法检索到字段！");
            }
        }
        #endregion

        #region 操作栏按钮点击事件 全选和清空
        //全选按钮
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                int count = Convert.ToInt32(this.dataGridView1.Rows.Count.ToString());
                for (int i = 0; i < count; i++)
                {
                    //如果DataGridView是可编辑的，将数据提交，否则处于编辑状态的行无法取到 
                    this.dataGridView1.EndEdit();
                    DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)this.dataGridView1.Rows[i].Cells["Check"];
                    checkCell.Value = true;
                }
            }
            catch (Exception)
            {

            }
        }
        //清空按钮
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                int count = Convert.ToInt32(this.dataGridView1.Rows.Count.ToString());
                for (int i = 0; i < count; i++)
                {
                    //如果DataGridView是可编辑的，将数据提交，否则处于编辑状态的行无法取到 
                    this.dataGridView1.EndEdit();
                    DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)this.dataGridView1.Rows[i].Cells["Check"];
                    checkCell.Value = false;
                }
            }
            catch (Exception)
            {
            }
        }
        //open code dir
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                string dirPath = textBox6.Text;
                using (System.Diagnostics.Process.Start("Explorer.exe", dirPath)) { }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region page2

        //将文本框文件保存到桌面
        private void saveCodeToFile()
        {
            string dirPath = textBox6.Text;
            string fileComplexPath = dirPath + textBox4.Text;
            Path_Helper_DG.CreateDirectoryIfNotExist(dirPath);
            using (FileStream fs = new FileStream(fileComplexPath, FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(CodeTxt);
                sw.Close();
            }
            MessageBox.Show("OutPut->" + fileComplexPath);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            saveCodeToFile();
        }

        //返回代码生成设置页面 也就是首页
        private void button14_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedTab = tabPage1;//转换到首页
        }

        //全选的按钮 的事件
        private void button12_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();//全选
        }
        //复制按钮的事件
        private void button13_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();//复制选择文本
        }


        #endregion

        #region function button
        /// <summary>
        /// set button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            ChangeTexBox4();
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            ChangeTexBox4();
        }
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            ChangeTexBox4();
        }
        private void ChangeTexBox4()
        {
            try
            {
                string[] createFileName = new string[2];
                if (textBox4.Text.Contains("."))
                {
                    createFileName = textBox4.Text.Trim().Split('.');
                }
                createFileName[0] = textBox5.Text.Trim() + textBox7.Text.Trim();
                textBox4.Text = $"{createFileName[0]}.{createFileName[1]}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region code builder
        //common set infoList
        private void SetInfoList()
        {
            //先将字段列表全部清空
            CreateInfoDic.Clear();

            FeildName.Clear();
            FeildType.Clear();
            FeildLength.Clear();
            FeildIsNullable.Clear();
            FeildDescription.Clear();
            FeildIsPK.Clear();
            FeildIsIdentity.Clear();

            int count = Convert.ToInt32(this.dataGridView1.Rows.Count.ToString());
            for (int i = 0; i < count; i++)
            {
                //如果DataGridView是可编辑的，将数据提交，否则处于编辑状态的行无法取到 
                this.dataGridView1.EndEdit();
                DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)this.dataGridView1.Rows[i].Cells["Check"];
                Boolean flag = Convert.ToBoolean(checkCell.Value);
                if (flag == true)     //查找被选择的数据行 
                {
                    //从 DATAGRIDVIEW 中获取数据项 
                    string FName = this.dataGridView1.Rows[i].Cells[1].Value.ToString().Trim();
                    FeildName.Add(FName);
                    string FType = this.dataGridView1.Rows[i].Cells[2].Value.ToString().Trim();
                    FeildType.Add(FType);
                    string FLength = this.dataGridView1.Rows[i].Cells[3].Value.ToString().Trim();
                    FeildLength.Add(FLength);
                    string FIsNullable = this.dataGridView1.Rows[i].Cells[4].Value.ToString().Trim();
                    FeildIsNullable.Add(FIsNullable);
                    string FMark = this.dataGridView1.Rows[i].Cells[5].Value.ToString().Trim();
                    FeildDescription.Add(FMark);
                    string FIsPK = this.dataGridView1.Rows[i].Cells[6].Value.ToString().Trim();
                    FeildIsPK.Add(FIsPK);
                    string FIsIdentity = this.dataGridView1.Rows[i].Cells[7].Value.ToString().Trim();
                    FeildIsIdentity.Add(FIsIdentity);
                }
            }
        }

        //builder common transmit settings
        private void InitCreateInfoDic()
        {
            SetInfoList();//设置信息

            CreateInfoDic.Add("Using", textBox3.Text.Trim());
            CreateInfoDic.Add("NameSpace", textBox2.Text.Trim());
            CreateInfoDic.Add("TableName", textBox9.Text.Trim());
            CreateInfoDic.Add("Class", textBox5.Text.Trim());
            CreateInfoDic.Add("ClassNamePlus", textBox7.Text.Trim());
            CreateInfoDic.Add("ClassExtends", textBox8.Text.Trim());

            CreateInfoDic.Add("FeildName", FeildName);
            CreateInfoDic.Add("FeildType", FeildType);
            CreateInfoDic.Add("FeildLength", FeildLength);
            CreateInfoDic.Add("FeildIsNullable", FeildIsNullable);
            CreateInfoDic.Add("FeildDescription", FeildDescription);
            CreateInfoDic.Add("FeildIsPK", FeildIsPK);
            CreateInfoDic.Add("FeildIsIdentity", FeildIsIdentity);
        }

        //build bode common component Func
        private void CommonComponent(Func<string> method)
        {
            try
            {
                setInitConfigFile();//record the opration history
                InitCreateInfoDic();//init create info to dictionary

                richTextBox1.Text = null;

                CodeTxt = method();

                richTextBox1.Text = CodeTxt;    //获取代码
                if (comboBox2.Text.Equals("To File"))
                {
                    saveCodeToFile();//save code to file
                }
                else
                {
                    this.tabControl1.SelectedTab = tabPage2;//trasfer to code view
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        //build Entities
        private void button1_Click(object sender, EventArgs e)
        {
            CommonComponent(() => CodeForEntity.CreateCode(CreateInfoDic));
        }
        //build SqlStatements
        private void button11_Click(object sender, EventArgs e)
        {
            CommonComponent(() => CodeForSqlStatement.CreateCode(CreateInfoDic));
        }
        //build QX_Frame.QueryObject
        private void button3_Click(object sender, EventArgs e)
        {
            CommonComponent(() => QX_FrameToQueryObject.CreateCode(CreateInfoDic));
        }
        //build QX_Frame.DataService
        private void button9_Click(object sender, EventArgs e)
        {
            CommonComponent(() => QX_FrameToDataService.CreateCode(CreateInfoDic));
        }
        //build QX_Frame.Contract
        private void button17_Click(object sender, EventArgs e)
        {
            CommonComponent(() =>QX_FrameToDataContract.CreateCode(CreateInfoDic));

        }
        #endregion


    }
}
