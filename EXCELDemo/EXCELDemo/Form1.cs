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

namespace EXCELDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private DataSet m_ds = new DataSet();
        private Dictionary<string, Byte[]> imageDic = new Dictionary<string, byte[]>();
        private SqlHelper m_SqlHelper = new SqlHelper();
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
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show("检验项目不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("报告单号不能为空！");
                return;
            }
            //if (m_ds != null && m_ds.Tables.Count > 1)
            //{
            //    this.pictureBox1.Image = null;
            //    dataGridView1.DataSource = null;
            //    if (string.IsNullOrEmpty(comboBox2.Text))
            //    {
            //        dataGridView1.DataSource = m_ds.Tables[comboBox1.Text];
            //    }
            //    else
            //    {
            //        DataTable dt = m_ds.Tables[comboBox1.Text];
                    
            //        DataTable dtNew = dt.Clone();
                    
            //        for (int i = 0; i < dt.Rows.Count; i++)
            //        {
            //            if (dt.Rows[i]["PageNo"].ToString() == comboBox2.Text)//查询条件 
            //            {
            //                dtNew.Rows.Add(dt.Rows[i].ItemArray);
            //            }
            //        }
            //        dataGridView1.DataSource = dtNew;

            //        if (imageDic.ContainsKey(comboBox2.Text))
            //        {
            //            Byte[] bytes = imageDic[comboBox2.Text];

            //            MemoryStream ms = new MemoryStream(bytes);  //核心方法  将图片加载到内存流中  
            //            this.pictureBox1.Image = Image.FromStream(ms);
            //        }
                    
            //    }
            //    for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            //   {

            //        this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

            //    }
            //}
            try
            {
                this.dataGridView1.DataSource = null;
                this.pictureBox1.Image = null;
                //测量报告表
                StringBuilder tempSql = new StringBuilder();
                tempSql.Append(" select c.MeasPageNo,d.PointNo,d.PointType,d.XAxis,d.YAxis,d.ZAxis,d.UpperTol,d.LowerTol,d.Direct,d.[AVG],d.CorrectDirect");
                tempSql.Append(" from ((MeasurementReport a left join MeasurementItem b on a.MeasReportID=b.MeasReportID) ");
                tempSql.Append("   left join MeasurementPage c on b.MeasItemID=c.MeasItemID)");
                tempSql.Append("   left join MeasurementPoint d on c.MeasPageID=d.MeasPageID");
                tempSql.Append(" where 1=1 ");
                tempSql.Append(" and a.ProjectNo='" + textBox4.Text + "'");
                tempSql.Append(" and b.MeasItemName='" + comboBox1.Text + "'");
                if (!string.IsNullOrEmpty(comboBox2.Text))
                {
                    tempSql.Append(" and c.MeasPageNo=" + comboBox2.Text);
                }
                tempSql.Append(" order by a.MeasReportID,b.MeasItemID,c.MeasPageID,d.MeasPointID");
                DataTable dt = m_SqlHelper.FillDataTable(tempSql.ToString());

                if (dt != null && dt.Rows.Count > 0)
                {
                    this.dataGridView1.DataSource = dt;
                }
                for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
                {

                    this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                }

                
                if (!string.IsNullOrEmpty(comboBox2.Text))
                {
                    StringBuilder tempSqlimage = new StringBuilder();
                    tempSqlimage.Append(" select c.MeasPageImage");
                    tempSqlimage.Append(" from (MeasurementReport a left join MeasurementItem b on a.MeasReportID=b.MeasReportID) ");
                    tempSqlimage.Append("   left join MeasurementPage c on b.MeasItemID=c.MeasItemID");
                    tempSqlimage.Append(" where 1=1 ");
                    tempSqlimage.Append(" and a.ProjectNo='" + textBox4.Text + "'");
                    tempSqlimage.Append(" and b.MeasItemName='" + comboBox1.Text + "'");
                    tempSqlimage.Append(" and c.MeasPageNo=" + comboBox2.Text);
                    tempSqlimage.Append(" order by a.MeasReportID,b.MeasItemID,c.MeasPageID");
                    DataTable dtimage = m_SqlHelper.FillDataTable(tempSqlimage.ToString());

                    if (dtimage != null && dtimage.Rows.Count > 0)
                    {
                        Byte[] bytes = dtimage.Rows[0][0] as Byte[];

                        MemoryStream ms = new MemoryStream(bytes);  //核心方法  将图片加载到内存流中  
                        this.pictureBox1.Image = Image.FromStream(ms);
                    }                    
                }
                
            }
            catch
            {
            }

        }

        /// <summary>
        /// 选中页码上传图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox2.Text))
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

                //if (imageDic.ContainsKey(comboBox2.Text))
                //{
                //    imageDic[comboBox2.Text]=bytes;
                //}
                //else
                //{
                //    imageDic.Add(comboBox2.Text, bytes);
                //}
                MemoryStream ms = new MemoryStream(bytes);  //核心方法  将图片加载到内存流中  

                //测点信息表上传图片
                string strMeasPageID = GetMeasPageID(textBox4.Text, comboBox1.Text, comboBox2.Text);
                string strSqlMeasPage = " update MeasurementPage set MeasPageImage=@Picture "+
                                        " where MeasPageID=" + strMeasPageID;
                int ipa = m_SqlHelper.ExecuteNonQuery(strSqlMeasPage, bytes);
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
            Microsoft.Practices.EnterpriseLibrary.Data.Database db = Microsoft.Practices.EnterpriseLibrary.Data.DatabaseFactory.CreateDatabase("conn");

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

            db.ExecuteNonQuery(CommandType.Text,sb.ToString());

            this.Invoke(this.myHandle, new object[] { (10 * 100 / 100), strProjectNo });

            var ProjectId = 0;

            ProjectId=int.Parse(db.ExecuteScalar(CommandType.Text, "select top 1 * from MeasurementReports order by MeasReportID desc").ToString());
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
                ItemId = int.Parse(db.ExecuteScalar(CommandType.Text, string.Format("select top 1 * from MeasurementItems where [MeasReportID]={0} and [MeasItemNO]='{1}'",ProjectId,row["SheetName"])).ToString());

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
                string strSqlReport = "select top 1 Mid(ProjectNo,11,4)  from  MeasurementReport order by ProjectNo Desc";
                DataTable dt = m_SqlHelper.FillDataTable(strSqlReport);

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
                string strSqlReport = "select top 1 MeasReportID  from  MeasurementReport where ProjectNo='" + strProjectNo.Trim() + "'";
                DataTable dt = m_SqlHelper.FillDataTable(strSqlReport);

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
                string strSqlReport = "select top 1 Mid(MeasItemNO,11,4)  from  MeasurementItem order by MeasItemNO Desc";
                DataTable dt = m_SqlHelper.FillDataTable(strSqlReport);

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
                string strSqlReport = "select top 1 MeasItemID  from  MeasurementItem where MeasItemNO='" + strMeasItemNO.Trim() + "'";
                DataTable dt = m_SqlHelper.FillDataTable(strSqlReport);

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
                string strSqlReport = "select top 1 MeasPageID  from  MeasurementPage where MeasItemID=" + strMeasItemID.Trim() + " and MeasPageNo=" + strMeasPageNo.Trim();
                DataTable dt = m_SqlHelper.FillDataTable(strSqlReport);

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
                tempSql.Append(" from (MeasurementReport a left join MeasurementItem b on a.MeasReportID=b.MeasReportID) ");
                tempSql.Append("   left join MeasurementPage c on b.MeasItemID=c.MeasItemID");
                tempSql.Append(" where 1=1 ");
                tempSql.Append(" and a.ProjectNo='" + strProjectNo + "'");
                tempSql.Append(" and b.MeasItemName='" + strMeasItemName + "'");
                tempSql.Append(" and c.MeasPageNo=" + strMeasPageNo);
                tempSql.Append(" order by a.MeasReportID,b.MeasItemID,c.MeasPageID");
                DataTable dt = m_SqlHelper.FillDataTable(tempSql.ToString());
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
        private string CheckIsNull(object strValue,string strResult)
        {
            if (strValue!= null)
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
                textBox4.Text = strProjectNo;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = null;
            button1_Click(null, null);
        }

        private void BtnCreateJson_Click(object sender, EventArgs e)
        {
            try
            {
                Os.Brain.Net.UrlRequest.GetText("http://114.55.105.88:8088/api/CreateJson");
                MessageBox.Show("生成成功");
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message);
            }
        }
    }
}
