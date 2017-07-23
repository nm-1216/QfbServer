using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ExcelUp
{
    public partial class FrmResult : Form
    {
        Microsoft.Practices.EnterpriseLibrary.Data.Database db = Microsoft.Practices.EnterpriseLibrary.Data.DatabaseFactory.CreateDatabase("conn");

        public FrmResult()
        {
            InitializeComponent();
            this.dgvList.AutoGenerateColumns = false;


            btnPre.Enabled = false;
            btnNext.Enabled = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            bind(30, 1);
        }

        private void bind(int pageSize, int currPage)
        {
            if (currPage < 1)
                currPage = 1;

            lblPage.Text = currPage.ToString();



            var bll = new QfbServer.Bll.MeasureDatas(Os.Brain.Data.DataActions.select);

            int x = 0;

            SqlParameter[] p = new SqlParameter[] {
                //new SqlParameter("@Id", txtSId.Text),
                //new SqlParameter("@ProjectId", txtSProjectId.Text),
                new SqlParameter("@ProjectName", txtXM.Text),
                //new SqlParameter("@ProductId", txtSProductId.Text),
                new SqlParameter("@ProductName", txtCP.Text),
                //new SqlParameter("@TargetId", txtSTargetId.Text),
                new SqlParameter("@TargetName", txtJCX.Text),
                //new SqlParameter("@TargetType", txtSTargetType.Text),
                //new SqlParameter("@PageId", string.IsNullOrEmpty(txtPage.Text)?null:txtPage.Text),
                //new SqlParameter("@MeasurePoint", txtSMeasurePoint.Text),
                //new SqlParameter("@Direction", txtSDirection.Text),
                //new SqlParameter("@Value1", txtSValue1.Text),
                //new SqlParameter("@Value2", txtSValue2.Text),
                //new SqlParameter("@Value3", txtSValue3.Text),
                //new SqlParameter("@Value4", txtSValue4.Text),
                //new SqlParameter("@Username", txtSUsername.Text),
                //new SqlParameter("@Timestamp", txtSTimestamp.Text),
                //new SqlParameter("@UpperTolerance", txtSUpperTolerance.Text),
                //new SqlParameter("@LowerTolerance", txtSLowerTolerance.Text),
                //new SqlParameter("@Value5", txtSValue5.Text),
                //new SqlParameter("@Value6", txtSValue6.Text),
                //new SqlParameter("@Value7", txtSValue7.Text),
                //new SqlParameter("@Value8", txtSValue8.Text),
                //new SqlParameter("@Value9", txtSValue9.Text),
                //new SqlParameter("@Value10", txtSValue10.Text),
            };

            var list = bll.GetList(pageSize, currPage, out x, p);
            var pageCound = PageCount(x, pageSize);

            label4.Text = string.Format("共 {0} 条数据, {1} 页， 第 {2} 页", x, pageCound, currPage);

            if (currPage <= 1)
            {
                btnPre.Enabled = false;
            }
            else
            {
                btnPre.Enabled = true;
            }

            if (currPage >= pageCound)
            {
                btnNext.Enabled = false;
            }
            else
            {
                btnNext.Enabled = true;
            }


            dgvList.DataSource = list;

        }


        private void FrmResult_Load(object sender, EventArgs e)
        {
            Type type = dgvList.GetType();
            System.Reflection.PropertyInfo pi = type.GetProperty("DoubleBuffered",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            pi.SetValue(dgvList, true, null);

            foreach (DataGridViewColumn item in this.dgvList.Columns)
            {
                item.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void btnPre_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblPage.Text.Trim()))
            {
                int x = int.Parse(lblPage.Text.Trim());
                bind(30, x - 1);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblPage.Text.Trim()))
            {
                int x = int.Parse(lblPage.Text.Trim());
                bind(30, x + 1);
            }
        }

        private int PageCount(int RecordCount, int PageSize)
        {
            int num = RecordCount / PageSize;
            return RecordCount % PageSize == 0 ? num : num + 1;
        }

        private void dgvList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow Row = dgvList.Rows[e.RowIndex];

                var cel1 = Row.Cells["Value1"];
                var cel2 = Row.Cells["Value2"];
                var cel3 = Row.Cells["Value3"];
                var cel4 = Row.Cells["Value4"];
                var cel5 = Row.Cells["Value5"];

                var cel6 = Row.Cells["Value6"];
                var cel7 = Row.Cells["Value7"];
                var cel8 = Row.Cells["Value8"];
                var cel9 = Row.Cells["Value9"];
                var cel10 = Row.Cells["Value10"];


                var LowerTolerance = Row.Cells["LowerTolerance"];
                var UpperTolerance = Row.Cells["UpperTolerance"];
                var TargetName = Row.Cells["TargetName"];




                SetCellRedCorlor(cel1, UpperTolerance, LowerTolerance, TargetName);
                SetCellRedCorlor(cel2, UpperTolerance, LowerTolerance, TargetName);
                SetCellRedCorlor(cel3, UpperTolerance, LowerTolerance, TargetName);
                SetCellRedCorlor(cel4, UpperTolerance, LowerTolerance, TargetName);
                SetCellRedCorlor(cel5, UpperTolerance, LowerTolerance, TargetName);
                SetCellRedCorlor(cel6, UpperTolerance, LowerTolerance, TargetName);
                SetCellRedCorlor(cel7, UpperTolerance, LowerTolerance, TargetName);
                SetCellRedCorlor(cel8, UpperTolerance, LowerTolerance, TargetName);
                SetCellRedCorlor(cel9, UpperTolerance, LowerTolerance, TargetName);
                SetCellRedCorlor(cel10, UpperTolerance, LowerTolerance, TargetName);
            }
        }

        private void SetCellRedCorlor(DataGridViewCell cel, DataGridViewCell max, DataGridViewCell min, DataGridViewCell Target)
        {
            //Surface
            //Trim

            if (null != cel.Value && !string.IsNullOrEmpty(cel.Value.ToString()))
            {
                var value = cel.Value.ToString().ToUpper();
                cel.Value = value;

                if (null != Target.Value && !string.IsNullOrEmpty(Target.Value.ToString()))
                {
                    var Targetvalue = Target.Value.ToString().ToUpper().Trim();

                    if (Targetvalue == "SURFACE" || Targetvalue == "TRIM")
                    {
                        //NOT OK,NG
                        if (null == max.Value || string.IsNullOrEmpty(max.Value.ToString()))
                            return;

                        if (null == min.Value || string.IsNullOrEmpty(min.Value.ToString()))
                            return;

                        try
                        {
                            var _max = float.Parse(max.Value.ToString());
                            var _min = float.Parse(min.Value.ToString());
                            var temp = float.Parse(cel.Value.ToString());
                            if (_max < temp || _min > temp)
                            {
                                cel.Style.BackColor = Color.Red;
                            }
                            else
                            {
                                cel.Style.BackColor = Color.White;
                            }
                        }
                        catch
                        {
                            cel.Style.BackColor = Color.Black;
                            cel.Style.ForeColor = Color.White;
                        }
                    }
                    else
                    {
                        //OK,NG
                        if (value != "OK")
                        {
                            if (value == "NG")
                                cel.Style.BackColor = Color.Red;
                            else
                            {
                                cel.Style.BackColor = Color.Black;
                                cel.Style.ForeColor = Color.White;
                            }
                        }
                        else
                        {
                            cel.Style.BackColor = Color.White;
                        }
                    }
                }
                else
                {
                    //暂时不做处理
                }
            }
        }

        private void dgvList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewColumn column = dgvList.Columns[e.ColumnIndex];
                if (column is DataGridViewButtonColumn)
                {
                    var temp = column as DataGridViewButtonColumn;

                    if (temp.DefaultCellStyle.NullValue.ToString() == "上传")
                    {
                        DataGridViewRow Row = dgvList.Rows[e.RowIndex];

                        var cel1 = Row.Cells["Value1"].Value ?? "";
                        var cel2 = Row.Cells["Value2"].Value ?? "";
                        var cel3 = Row.Cells["Value3"].Value ?? "";
                        var cel4 = Row.Cells["Value4"].Value ?? "";
                        var cel5 = Row.Cells["Value5"].Value ?? "";

                        var cel6 = Row.Cells["Value6"].Value ?? "";
                        var cel7 = Row.Cells["Value7"].Value ?? "";
                        var cel8 = Row.Cells["Value8"].Value ?? "";
                        var cel9 = Row.Cells["Value9"].Value ?? "";
                        var cel10 = Row.Cells["Value10"].Value ?? "";


                        var id = Row.Cells["ID"].Value ?? "";




                        SqlParameter[] p = new SqlParameter[] {
                            new SqlParameter("@Value1",cel1),
                            new SqlParameter("@Value2",cel2),
                            new SqlParameter("@Value3",cel3),
                            new SqlParameter("@Value4",cel4),
                            new SqlParameter("@Value5",cel5),


                            new SqlParameter("@Value6",cel6),
                            new SqlParameter("@Value7",cel7),
                            new SqlParameter("@Value8",cel8),
                            new SqlParameter("@Value9",cel9),
                            new SqlParameter("@Value10",cel10),

                            new SqlParameter("@id",id)
                        };

                        var dbc = db.GetSqlStringCommand("UPDATE [dbo].[MeasureDatas] SET [Value1] = @Value1,[Value2] = @Value2,[Value3] = @Value3,[Value4] = @Value4,[Value5] = @Value5,[Value6] = @Value6,[Value7] = @Value7,[Value8] = @Value8,[Value9] = @Value9,[Value10] = @Value10 WHERE ID=@ID");
                        dbc.Parameters.AddRange(p);
                        var number = db.ExecuteNonQuery(dbc);
                        if (number > 0)
                        {
                            MessageBox.Show("修改成功");
                        }
                        else
                        {
                            MessageBox.Show("删除失败");
                        }
                    }
                }
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {

            string sql = @"SELECT 
                            [ProjectName] 项目号
                            ,[ProductName] 零件号
                            ,[TargetName] 检测项
                            ,[MeasurePoint] 检测点
                            ,[Direction] 方向
                            ,[UpperTolerance] 上公差
                            ,[LowerTolerance] 下公差
                            ,[Value1] 值1
                            ,[Value2] 值2
                            ,[Value3] 值3
                            ,[Value4] 值4
                            ,[Value5] 值5
                            ,[Value6] 值6
                            ,[Value7] 值7
                            ,[Value8] 值8
                            ,[Value9] 值9
                            ,[Value10] 值10
                            ,[Username] 用户
                            ,CONVERT(varchar(100), DATEADD(s,convert(int,Timestamp/1000),'1970-01-01 08:00:00'), 112) 时间
                            FROM [MeasureDatas] 
                            where (''=@ProjectName or ProjectName like @ProjectName)  
                            and (''=@ProductName or ProductName like @ProductName) 
                            and (''=@TargetName or TargetName like @TargetName) 
                        "
                ;


            SqlParameter[] p = new SqlParameter[] {
            new SqlParameter("@ProjectName","%"+ txtXM.Text.Trim()+"%"),
            new SqlParameter("@ProductName","%"+txtCP.Text.Trim()+"%"),
            new SqlParameter("@TargetName","%"+txtJCX.Text.Trim()+"%")
            };

            var dbc = db.GetSqlStringCommand(sql);
            dbc.Parameters.AddRange(p);
            var ds = db.ExecuteDataSet(dbc);


            if (null == ds || ds.Tables[0].Rows.Count <= 0)
            {
                MessageBox.Show("没有可下载文件");
            }

            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            var dir = path.SelectedPath;

            var FillPath = string.Format("{0}/{1}", dir, Guid.NewGuid().ToString() + ".xlsx");

            ExcelHelper1 excel = new ExcelHelper1(FillPath);

            excel.DataTableToExcel(ds.Tables[0], "检测值", true);
            excel.Dispose();
        }
    }
}
