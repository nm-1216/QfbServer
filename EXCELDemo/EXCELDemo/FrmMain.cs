using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utility;
using System.IO;
using System.Threading;
using System.Data.SqlClient;
using System.Data.Common;
using ExcelUp;

namespace EXCELDemo
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            bind();
        }

        public string ApiUrl = System.Configuration.ConfigurationManager.AppSettings["ApiUrl"].ToString();

        private DataSet m_ds = new DataSet();
        private Dictionary<string, Byte[]> imageDic = new Dictionary<string, byte[]>();
        Microsoft.Practices.EnterpriseLibrary.Data.Database db = Microsoft.Practices.EnterpriseLibrary.Data.DatabaseFactory.CreateDatabase("conn");

        private delegate void funHandle(int nValue, string strProjectNo);
        private funHandle myHandle = null;
        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "F:\\";
            openFileDialog.Filter = "2007文件|*.xlsx|2003文件|*.xls";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string strError = string.Empty;
                    m_ds = ExcelHelper.ExcelToDataSet(openFileDialog.FileName, System.Windows.Forms.Application.StartupPath + @"/XMLFile.xml");

                    if (string.IsNullOrEmpty(strError))
                    {
                        #region XmlFile OK
                        if (m_ds != null && m_ds.Tables.Count > 0)
                        {
                            this.progressBar1.Visible = true;
                            this.progressBar1.Value = 0;
                            this.btnImport.Enabled = false;
                            this.button1.Enabled = false;
                            this.button2.Enabled = false;
                            Thread t = new Thread(new ParameterizedThreadStart(InsertDataBase));
                            t.Start(m_ds);
                        }
                        #endregion
                    }
                    else
                    {
                        #region XmlFile Error
                        MessageBox.Show(strError);
                        #endregion
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox2.SelectedValue.ToString().Trim()))
            {
                MessageBox.Show("检验项目不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(comboBox1.SelectedValue.ToString()))
            {
                MessageBox.Show("报告单号不能为空！");
                return;
            }

            try
            {
                this.dataGridView1.DataSource = null;
                this.pictureBox1.Image = null;
                //测量报告表
                StringBuilder tempSql = new StringBuilder();
                tempSql.Append(" select c.MeasPageNo,d.PointNo,d.PointType,d.XAxis,d.YAxis,d.ZAxis,d.UpperTol,d.LowerTol,d.Direct,d.[AVG],d.CorrectDirect");
                tempSql.Append(" from ((MeasurementReports a left join MeasurementItems b on a.MeasReportID=b.MeasReportID) ");
                tempSql.Append("   left join MeasurementPages c on b.MeasItemID=c.MeasItemID)");
                tempSql.Append("   left join MeasurementPoints d on c.MeasPageID=d.MeasPageID");
                tempSql.Append(" where 1=1 ");
                tempSql.Append(" and a.ProjectNo='" + comboBox1.SelectedValue.ToString() + "'");
                tempSql.Append(" and b.MeasItemName='" + comboBox2.SelectedValue.ToString() + "'");
                if (!string.IsNullOrEmpty(txtYM.Text.Trim()))
                {
                    tempSql.Append(" and c.MeasPageNo=" + txtYM.Text.Trim());
                }
                tempSql.Append(" order by a.MeasReportID,b.MeasItemID,c.MeasPageID,d.MeasPointID");

                Os.Brain.Common.Debug.WriteLog("./debug.txt", tempSql.ToString());

                var ds = db.ExecuteDataSet(CommandType.Text, tempSql.ToString());

                if (null == ds && ds.Tables.Count <= 0)
                {
                    MessageBox.Show("查无数据");
                    return;
                }

                DataTable dt = ds.Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    this.dataGridView1.DataSource = dt;
                }
                for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
                {

                    this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                }


                if (!string.IsNullOrEmpty(txtYM.Text.Trim()))
                {
                    StringBuilder tempSqlimage = new StringBuilder();
                    tempSqlimage.Append(" select c.MeasPageImage");
                    tempSqlimage.Append(" from (MeasurementReports a left join MeasurementItems b on a.MeasReportID=b.MeasReportID) ");
                    tempSqlimage.Append("   left join MeasurementPages c on b.MeasItemID=c.MeasItemID");
                    tempSqlimage.Append(" where 1=1 ");
                    tempSqlimage.Append(" and a.ProjectNo='" + comboBox1.SelectedValue.ToString() + "'");
                    tempSqlimage.Append(" and b.MeasItemName='" + comboBox2.SelectedValue.ToString() + "'");
                    tempSqlimage.Append(" and c.MeasPageNo=" + txtYM.Text.Trim());
                    tempSqlimage.Append(" order by a.MeasReportID,b.MeasItemID,c.MeasPageID");

                    var dsimg = db.ExecuteDataSet(CommandType.Text, tempSqlimage.ToString());

                    DataTable dtimage = dsimg.Tables[0];

                    if (dtimage != null && dtimage.Rows.Count > 0)
                    {
                        Byte[] bytes = dtimage.Rows[0][0] as Byte[];
                        if (null != bytes)
                        {
                            MemoryStream ms = new MemoryStream(bytes);  //核心方法  将图片加载到内存流中  
                            this.pictureBox1.Image = Image.FromStream(ms);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// 选中页码上传图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtYM.Text.Trim()))
            {
                MessageBox.Show("页码不能为空！");
                return;
            }
            //创建一个对话框对象
            OpenFileDialog ofd = new OpenFileDialog();
            //为对话框设置标题
            ofd.Title = "请选择上传的图片";
            //设置筛选的图片格式
            ofd.Filter = "图片格式|*.jpg";
            //设置是否允许多选
            ofd.Multiselect = false;
            //如果你点了“确定”按钮
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //获得文件的完整路径（包括名字后后缀）
                string filePath = ofd.FileName;
                int position = filePath.LastIndexOf("\\");
                //从完整路径中截取出来文件名“1.jpg”
                string fileName = filePath.Substring(position + 1);
                Byte[] bytes = GetContent(filePath);

                MemoryStream ms = new MemoryStream(bytes);

                //测点信息表上传图片
                string strMeasPageID = GetMeasPageID(comboBox1.SelectedValue.ToString(), comboBox2.SelectedValue.ToString(), txtYM.Text.Trim());
                string strSqlMeasPage = " update MeasurementPages set MeasPageImage=@Picture " + " where MeasPageID=" + strMeasPageID;
                DbCommand dbc = db.GetSqlStringCommand(strSqlMeasPage);
                db.AddInParameter(dbc, "@Picture", DbType.AnsiString, bytes);
                int ipa = db.ExecuteNonQuery(dbc);
                this.pictureBox1.Image = Image.FromStream(ms);
            }

        }

        public string picAddress = string.Empty;
        /// <summary>
        /// 将指定路径下的文件转换成二进制代码，用于传输到数据库
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public Byte[] GetContent(string filepath)
        {
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            Byte[] byData = new Byte[fs.Length];//新建用于保存文件流的字节数组
            fs.Read(byData, 0, byData.Length);//读取文件流
            fs.Close();
            return byData;
        }

        /// <summary>
        /// 导入数据库
        /// </summary>
        private void InsertDataBase(object ods)
        {
            MethodInvoker mi = new MethodInvoker(ShowProgressBar);
            this.BeginInvoke(mi);

            DataSet ds = ods as DataSet;
            if (ds == null)
            {
                MessageBox.Show("没有数据可以导入数据库！");
                return;
            }



            //拷贝DataTable1的结构和数据
            DataTable NewDataTable = ds.Tables[0].Copy();

            for (int i = 1; i < ds.Tables.Count; i++)
            {
                //添加DataTable2的数据
                foreach (DataRow dr in ds.Tables[i].Rows)
                {
                    NewDataTable.ImportRow(dr);
                }
            }

            if (NewDataTable.Rows.Count <= 0)
            {
                MessageBox.Show("没有数据可以导入数据库！");
                return;
            }

            DataView dataView = NewDataTable.DefaultView;

            DataTable project = dataView.ToTable(true, "Project", "PartNo", "PartName");

            StringBuilder sb = new StringBuilder();

            string strProjectNo = string.Empty;

            #region Project
            foreach (DataRow row in project.Rows)
            {
                strProjectNo = row["Project"].ToString();
                sb.AppendFormat("insert into MeasurementReports(ProjectNo,PartNo,PartName,CreateTime,Creator) values('{0}','{1}','{2}','{3}','{4}');"
                    , row["Project"]
                    , row["PartNo"]
                    , row["PartName"]
                    , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    , "管理员"
                    );
            }

            db.ExecuteNonQuery(CommandType.Text, sb.ToString());

            this.Invoke(this.myHandle, new object[] { (10 * 100 / 100), strProjectNo });

            var ProjectId = 0;

            ProjectId = int.Parse(db.ExecuteScalar(CommandType.Text, "select top 1 * from MeasurementReports order by MeasReportID desc").ToString());
            #endregion


            #region item
            sb.Clear();
            DataTable SheetName = dataView.ToTable(true, "SheetName");

            foreach (DataRow row in SheetName.Rows)
            {
                sb.AppendFormat("INSERT INTO [MeasurementItems]([MeasReportID],[MeasItemNO],[MeasItemName]) VALUES ({0},'{1}','{1}');"
                    , ProjectId
                    , row["SheetName"]
                    );
            }
            db.ExecuteNonQuery(CommandType.Text, sb.ToString());
            #endregion
            this.Invoke(this.myHandle, new object[] { (30 * 100 / 100), strProjectNo });


            #region page
            sb.Clear();
            DataTable Pages = dataView.ToTable(true, "SheetName", "Pages");
            foreach (DataRow row in Pages.Rows)
            {
                var ItemId = 0;
                ItemId = int.Parse(db.ExecuteScalar(CommandType.Text, string.Format("select top 1 * from MeasurementItems where [MeasReportID]={0} and [MeasItemNO]='{1}'", ProjectId, row["SheetName"])).ToString());

                sb.AppendFormat("INSERT INTO [dbo].[MeasurementPages]([MeasItemID],[MeasPageNo]) VALUES ({0},'{1}');"
                    , ItemId
                    , row["Pages"]
                );
            }
            db.ExecuteNonQuery(CommandType.Text, sb.ToString());

            #endregion
            this.Invoke(this.myHandle, new object[] { (50 * 100 / 100), strProjectNo });


            #region point
            sb.Clear();
            DataTable point = dataView.ToTable(true, "SheetName", "Pages");
            foreach (DataRow row in point.Rows)
            {
                var PageId = 0;
                PageId = int.Parse(db.ExecuteScalar(CommandType.Text,
                    string.Format("select [MeasPageID] from [MeasurementPages] a left join [dbo].[MeasurementItems] b on a.[MeasItemID]=b.MeasItemID where b.MeasItemNO='{0}' and [MeasPageNo]={1} and [MeasReportID]={2}",
                    row["SheetName"],
                    row["Pages"],
                    ProjectId
                    )
                ).ToString());

                string filter = "Pages = '" + row["Pages"].ToString() + "' and SheetName = '" + row["SheetName"].ToString() + "'";

                DataRow[] drArr = NewDataTable.Select(filter);

                foreach (var item in drArr)
                {
                    string sql = @"INSERT INTO [dbo].[MeasurementPoints]
                    ([MeasPageID],[PointNo],[PointType],[XAxis]
                    ,[YAxis],[ZAxis],[UpperTol],[LowerTol]
                    ,[Direct],[AVG]
                    ,[CorrectDirect])
                    VALUES
                    ({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}');";

                    sb.AppendFormat(sql
                    , PageId

                    , item["PointNo"]
                    , item["PointType"]
                    , item["XAxis"]
                    , item["YAxis"]
                    , item["ZAxis"]

                    , item["UpperTol"]
                    , item["LowerTol"]
                    , item["Direct"]
                    , item["AVG"]
                    , item["CorrectDirect"]
                );

                }


            }
            db.ExecuteNonQuery(CommandType.Text, sb.ToString());
            #endregion

            this.Invoke(this.myHandle, new object[] { (100 * 100 / 100), strProjectNo });

            this.button1.Enabled = true;
            this.button2.Enabled = true;
            this.btnImport.Enabled = true;


        }

        /// <summary>
        /// 获取测量报告表最大流水号
        /// </summary>
        private string GetProjectNoMax()
        {
            string strRet = "0001";
            try
            {
                //测量报告表
                string strSqlReport = "select top 1 Mid(ProjectNo,11,4)  from  MeasurementReports order by ProjectNo Desc";

                DataTable dt = db.ExecuteDataSet(CommandType.Text, strSqlReport).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    strRet = (Convert.ToInt32(dt.Rows[0][0].ToString()) + 1).ToString("0000");
                }
            }
            catch
            {
            }
            return strRet;
        }

        /// <summary>
        /// 获取测量报告表主键
        /// </summary>
        private string GetMeasReportID(string strProjectNo)
        {
            string strRet = "";
            try
            {
                //测量报告表
                string strSqlReport = "select top 1 MeasReportID  from  MeasurementReports where ProjectNo='" + strProjectNo.Trim() + "'";
                DataTable dt = db.ExecuteDataSet(CommandType.Text, strSqlReport).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    strRet = dt.Rows[0][0].ToString();
                }
            }
            catch
            {
            }
            return strRet;
        }

        /// <summary>
        /// 获取测量项目表最大流水号
        /// </summary>
        private string GetItemNoMax()
        {
            string strRet = "0001";
            try
            {
                //测量报告表
                string strSqlReport = "select top 1 Mid(MeasItemNO,11,4)  from  MeasurementItems order by MeasItemNO Desc";
                DataTable dt = db.ExecuteDataSet(CommandType.Text, strSqlReport).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    strRet = (Convert.ToInt32(dt.Rows[0][0].ToString()) + 1).ToString("0000");
                }
            }
            catch
            {
            }
            return strRet;
        }

        /// <summary>
        /// 获取测量项目表主键
        /// </summary>
        private string GetMeasItemID(string strMeasItemNO)
        {
            string strRet = "";
            try
            {
                //测量报告表
                string strSqlReport = "select top 1 MeasItemID  from  MeasurementItems where MeasItemNO='" + strMeasItemNO.Trim() + "'";
                DataTable dt = db.ExecuteDataSet(CommandType.Text, strSqlReport).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    strRet = dt.Rows[0][0].ToString();
                }
            }
            catch
            {
            }
            return strRet;
        }

        /// <summary>
        /// 获取测量页表主键
        /// </summary>
        private string GetMeasPageID(string strMeasItemID, string strMeasPageNo)
        {
            string strRet = "";
            try
            {
                //测量报告表
                string strSqlReport = "select top 1 MeasPageID  from  MeasurementPages where MeasItemID=" + strMeasItemID.Trim() + " and MeasPageNo=" + strMeasPageNo.Trim();
                DataTable dt = db.ExecuteDataSet(CommandType.Text, strSqlReport).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    strRet = dt.Rows[0][0].ToString();
                }
            }
            catch
            {
            }
            return strRet;
        }

        /// <summary>
        /// 获取测量页表主键
        /// </summary>
        private string GetMeasPageID(string strProjectNo, string strMeasItemName, string strMeasPageNo)
        {
            string strRet = "";
            try
            {
                //测量报告表
                StringBuilder tempSql = new StringBuilder();
                tempSql.Append(" select c.MeasPageID");
                tempSql.Append(" from (MeasurementReports a left join MeasurementItems b on a.MeasReportID=b.MeasReportID) ");
                tempSql.Append("   left join MeasurementPages c on b.MeasItemID=c.MeasItemID");
                tempSql.Append(" where 1=1 ");
                tempSql.Append(" and a.ProjectNo='" + strProjectNo + "'");
                tempSql.Append(" and b.MeasItemName='" + strMeasItemName + "'");
                tempSql.Append(" and c.MeasPageNo=" + strMeasPageNo);
                tempSql.Append(" order by a.MeasReportID,b.MeasItemID,c.MeasPageID");
                DataTable dt = db.ExecuteDataSet(CommandType.Text, tempSql.ToString()).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    strRet = dt.Rows[0][0].ToString();
                }
            }
            catch
            {
            }
            return strRet;
        }

        /// <summary>
        /// 校验，如果为null，转换为空
        /// </summary>
        /// <param name="strMeasItemNO"></param>
        /// <param name="strResult"></param>
        /// <returns></returns>
        private string CheckIsNull(object strValue, string strResult)
        {
            if (strValue != null)
            {
                if (!string.IsNullOrEmpty(strValue.ToString()))
                {
                    return strValue.ToString();
                }
                else
                {
                    return strResult;
                }
            }
            else
            {
                return strResult;
            }
        }

        /// <summary>  
        /// 线程函数中调用的函数  
        /// </summary>  
        private void ShowProgressBar()
        {
            myHandle = new funHandle(SetProgressValue);
        }

        public void SetProgressValue(int value, string strProjectNo)
        {

            this.progressBar1.Value = value;
            // this.label1.Text = "Progress :" + value.ToString() + "%";

            // 这里关闭，比较好，呵呵！  
            if (value > this.progressBar1.Maximum - 1)
            {
                progressBar1.Visible = false;
                progressBar1.Value = 0;
                this.btnImport.Enabled = true;//设置可用
                this.button1.Enabled = true;
                this.button2.Enabled = true;
                bind();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = null;
            button1_Click(null, null);
        }

        private void BtnCreateJson_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("生成新的JSON文件吗 ?", "Confirm Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    Os.Brain.Net.UrlRequest.GetText(ApiUrl + "/api/CreateJson");
                    MessageBox.Show("生成成功");
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.Message);
                }

            }
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            FrmUser user = new FrmUser();
            user.Show();
        }

        private void btnResult_Click(object sender, EventArgs e)
        {
            FrmResult result = new FrmResult();
            result.Show();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x112)
            {
                switch ((int)m.WParam)
                {
                    //禁止双击标题栏关闭窗体
                    case 0xF063:
                    case 0xF093:
                        m.WParam = IntPtr.Zero;
                        break;

                    //禁止拖拽标题栏还原窗体
                    //case 0xF012:
                    //case 0xF010:
                    //    m.WParam = IntPtr.Zero;
                    //    break;

                    //禁止双击标题栏
                    case 0xf122:
                        m.WParam = IntPtr.Zero;
                        break;


                    //禁止最大化按钮
                    case 0xf020:
                        m.WParam = IntPtr.Zero;
                        break;

                    //禁止最小化按钮
                    case 0xf030:
                        m.WParam = IntPtr.Zero;
                        break;

                    //禁止还原按钮
                    case 0xf120:
                        m.WParam = IntPtr.Zero;
                        break;
                }
            }
            base.WndProc(ref m);
        }


        private void bind()
        {

            string strSqlReport = "select distinct [ProjectNo] text,[ProjectNo] value  from [MeasurementReports]";
            DataTable dt = db.ExecuteDataSet(CommandType.Text, strSqlReport).Tables[0];

            DataRow newRow;
            newRow = dt.NewRow();
            newRow["text"] = "==请选择项目名称==";
            newRow["value"] = "";
            dt.Rows.InsertAt(newRow, 0);

            comboBox1.ValueMember = "value";
            comboBox1.DisplayMember = "text";
            comboBox1.DataSource = dt;


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var temp = comboBox1.SelectedValue.ToString();

            string strSqlReport = "SELECT distinct [MeasItemNO] text,[MeasItemNO] value FROM [MeasurementItems] A LEFT JOIN [dbo].[MeasurementReports] B ON A.MeasReportID=B.MeasReportID WHERE B.ProjectNo='" + temp + "'";
            DataTable dt = db.ExecuteDataSet(CommandType.Text, strSqlReport).Tables[0];

            DataRow newRow;
            newRow = dt.NewRow();
            newRow["text"] = "==请选择检测项目==";
            newRow["value"] = "";
            dt.Rows.InsertAt(newRow, 0);

            comboBox2.ValueMember = "value";
            comboBox2.DisplayMember = "text";
            comboBox2.DataSource = dt;

        }
    }
}
