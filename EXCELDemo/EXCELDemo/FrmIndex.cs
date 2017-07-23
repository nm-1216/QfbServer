using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utility;

namespace ExcelUp
{
    public partial class FrmIndex : Form
    {
        Microsoft.Practices.EnterpriseLibrary.Data.Database db = Microsoft.Practices.EnterpriseLibrary.Data.DatabaseFactory.CreateDatabase("conn");
        private delegate void funHandle(int nValue, string strProjectNo);

        private funHandle myHandle = null;

        public FrmIndex()
        {
            InitializeComponent();
            GvList.AutoGenerateColumns = false;

        }

        private void bind()
        {
            string strSqlReport = "select distinct [ProjectNo] text,[ProjectNo] value  from [MeasurementReports]";
            var ds = db.ExecuteDataSet(CommandType.Text, strSqlReport);

            if (null != ds)
            {
                var dt = ds.Tables[0];
                DataRow newRow;
                newRow = dt.NewRow();
                newRow["text"] = "==请选择项目名称==";
                newRow["value"] = "";
                dt.Rows.InsertAt(newRow, 0);

                cbbXM.ValueMember = "value";
                cbbXM.DisplayMember = "text";
                cbbXM.DataSource = dt;
            }
        }

        private void FrmIndex_Load(object sender, EventArgs e)
        {
            bind();
            this.AcceptButton = btnSearch;
        }

        private void cbbXM_SelectedIndexChanged(object sender, EventArgs e)
        {
            var temp = cbbXM.SelectedValue.ToString();


            SqlParameter[] p = new SqlParameter[] {
                new SqlParameter("@ProjectNo",temp)
            };

            var dbc = db.GetSqlStringCommand("SELECT distinct [PartNo] text,[PartNo] value FROM [MeasurementReports] WHERE ProjectNo=@ProjectNo");
            dbc.Parameters.AddRange(p);
            var ds = db.ExecuteDataSet(dbc);

            if (null != ds)
            {
                var dt = ds.Tables[0];
                DataRow newRow;
                newRow = dt.NewRow();
                newRow["text"] = "==请选择零件号==";
                newRow["value"] = "";
                dt.Rows.InsertAt(newRow, 0);

                cbbLJ.ValueMember = "value";
                cbbLJ.DisplayMember = "text";
                cbbLJ.DataSource = dt;
            }
        }


        private void GvBind()
        {
            SqlParameter[] p = new SqlParameter[] {
                new SqlParameter("@ProjectNo",cbbXM.SelectedValue),
                new SqlParameter("@PartNo",cbbLJ.SelectedValue)
            };

            var dbc = db.GetSqlStringCommand("SELECT [MeasReportID],[ProjectNo],[PartNo],[PartName],[CreateTime] FROM [MeasurementReports] WHERE (''=@ProjectNo OR [ProjectNo]=@ProjectNo) AND (''=@PartNo OR [PartNo]=@PartNo)");
            dbc.Parameters.AddRange(p);
            var ds = db.ExecuteDataSet(dbc);


            GvList.DataSource = ds.Tables[0];
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GvBind();
        }

        private void GvList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);

        }

        private void GvList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewColumn column = GvList.Columns[e.ColumnIndex];
                var id = GvList.Rows[e.RowIndex].Cells["id"].Value.ToString();
                var ProjectNo = GvList.Rows[e.RowIndex].Cells["ProjectNo"].Value.ToString();
                var PartNo = GvList.Rows[e.RowIndex].Cells["PartNo"].Value.ToString();
                var PartName = GvList.Rows[e.RowIndex].Cells["PartName"].Value.ToString();

                if (column is DataGridViewButtonColumn)
                {
                    var temp = column as DataGridViewButtonColumn;

                    if (temp.DefaultCellStyle.NullValue.ToString() == "删除")
                    {
                        if (MessageBox.Show("确定要删除数据吗 ?删除数据将不可恢复！", "Confirm Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {

                            SqlParameter[] p = new SqlParameter[] {
                                new SqlParameter("@MeasReportID",id),
                            };

                            var dbc = db.GetStoredProcCommand("DeleteMeasurementReports");
                            dbc.Parameters.AddRange(p);
                            var number = db.ExecuteNonQuery(dbc);
                            if (number > 0)
                            {
                                GvBind();
                            }
                            else
                            {
                                MessageBox.Show("删除失败");
                            }
                        }

                    }
                    else if (temp.DefaultCellStyle.NullValue.ToString() == "查询")
                    {
                        FrmIndex1 _FrmIndex1 = new FrmIndex1(int.Parse(id), ProjectNo, PartNo, PartName);
                        _FrmIndex1.Show();
                    }
                    else
                    {
                        MessageBox.Show("未知响应事件");
                    }
                }
            }
        }

        private void 用户管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmUser _frmUser = new FrmUser();
            _frmUser.Show();
        }

        private void 测量标准导入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要导入数据吗？", "Confirm Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
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
                        var m_ds = ExcelHelper.ExcelToDataSet(openFileDialog.FileName, System.Windows.Forms.Application.StartupPath + @"/XMLFile.xml");

                        if (string.IsNullOrEmpty(strError))
                        {
                            #region XmlFile OK
                            if (m_ds != null && m_ds.Tables.Count > 0)
                            {
                                this.progressBar1.Value = 0;
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
        }

        private void InsertDataBase(object ods)
        {

            #region init
            DataSet ds = ods as DataSet;
            if (ds == null)
            {
                MessageBox.Show("没有数据可以导入数据库！");
                return;
            }

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

            if (project.Rows.Count != 1)
            {
                MessageBox.Show("Excel文件不正确");
                return;
            }
            else
            {
                var row = project.Rows[0];

                SqlParameter[] p = new SqlParameter[] {
                    new SqlParameter("@ProjectNo",row["Project"]),
                    new SqlParameter("@PartNo",row["PartNo"])
                };

                var sql=("select ProjectNo,PartNo from MeasurementReports where ProjectNo=@ProjectNo and PartNo=@PartNo");


                var dbc = db.GetSqlStringCommand(sql);
                dbc.Parameters.AddRange(p);
                var temp = db.ExecuteDataSet(dbc);

                if(null!=temp && temp.Tables[0].Rows.Count>0)
                {
                    MessageBox.Show("已经存在 此零件号");
                    return;
                }
            }

            #endregion

            MethodInvoker mi = new MethodInvoker(ShowProgressBar);
            this.BeginInvoke(mi);


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
        }

        private void ShowProgressBar()
        {
            if (!this.progressBar1.Visible)
                this.progressBar1.Visible = true;
            myHandle = new funHandle(SetProgressValue);
        }

        public void SetProgressValue(int value, string strProjectNo)
        {

            this.progressBar1.Value = value;
            if (value > this.progressBar1.Maximum - 1)
            {
                progressBar1.Visible = false;
                progressBar1.Value = 0;
                this.测量标准导入ToolStripMenuItem.Enabled = true;
                bind();
            }
        }

        private void 测量标准下发ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmIndex4 _frmIndex4 = new FrmIndex4();
            _frmIndex4.Show();
        }

        private void 历史查询与分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmResult _frmResult = new FrmResult();
            _frmResult.Show();
        }

        private void 检测值分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmIndex3 _frmIndex3 = new FrmIndex3();
            _frmIndex3.Show();
        }
    }
}
