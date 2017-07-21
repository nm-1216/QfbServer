using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelUp
{
    public partial class FrmIndex2 : Form
    {
        string _temp = "项目号：{0}        零件号：{1}        零件名称：{2}        检测项：{3}";

        Microsoft.Practices.EnterpriseLibrary.Data.Database db = Microsoft.Practices.EnterpriseLibrary.Data.DatabaseFactory.CreateDatabase("conn");

        private int iid = 0;
        private int pid = 1;
        private int max = 1;
        public FrmIndex2(int iid,int pid)
        {
            this.iid = iid;
            this.pid = pid;
            InitializeComponent();
        }

        private void GvBind()
        {
            if (this.pid <= 1)
                this.pid = 1;

            if (this.pid >= max)
                this.pid = max;

            SqlParameter[] p = new SqlParameter[] {
                new SqlParameter("@MeasPageNo",pid),
                new SqlParameter("@MeasItemID",iid)
            };

            string sql = @"SELECT a.MeasPageNo,b.MeasItemNO,c.ProjectNo,c.PartNo,c.PartName
            FROM [QfbServer].[dbo].[MeasurementPages] a
            left join [dbo].[MeasurementItems] b on a.MeasItemID=b.MeasItemID
            left join [dbo].[MeasurementReports] c on b.MeasReportID=c.MeasReportID
            where a.MeasPageNo=@MeasPageNo and b.MeasItemID=@MeasItemID;
            select [MeasPointID],[PointNo],[UpperTol],[LowerTol],[Direct] 
            from [dbo].[MeasurementPoints] a
            left join [dbo].[MeasurementPages] b on a.MeasPageID=b.MeasPageID
            where b.MeasPageNo=@MeasPageNo and b.MeasItemID=@MeasItemID;
            SELECT count(*) num  FROM [MeasurementPages] where [MeasItemID]=@MeasItemID
            ";

            var dbc = db.GetSqlStringCommand(sql);
            dbc.Parameters.AddRange(p);
            var ds = db.ExecuteDataSet(dbc);

            if (null != ds)
            {
                lbl1.Text = string.Format(_temp,
                    ds.Tables[0].Rows[0]["ProjectNo"].ToString(),
                    ds.Tables[0].Rows[0]["PartNo"].ToString(),
                    ds.Tables[0].Rows[0]["PartName"].ToString(),
                    ds.Tables[0].Rows[0]["MeasItemNO"].ToString()
                    );

                var _num =ds.Tables[2].Rows[0][0].ToString();
                if (!string.IsNullOrEmpty(_num))
                {
                    max = int.Parse(_num);
                }

                if (pid > 1)
                {
                    btnPre.Enabled = true;
                }
                else
                {
                    btnPre.Enabled = false;
                }

                if (pid < max)
                {
                    btnNext.Enabled = true;
                }
                else
                {
                    btnNext.Enabled = false;
                }

                lblPage.Text = string.Format("{0} of {1}", pid, max);

                GvList.DataSource = ds.Tables[1];

            }

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

        private void FrmIndex2_Load(object sender, EventArgs e)
        {
            this.btnNext.Enabled = false;
            this.btnPre.Enabled = false;
            GvList.AutoGenerateColumns = false;
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

        private void btnPre_Click(object sender, EventArgs e)
        {
            this.pid--;
            GvBind();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.pid++;
            GvBind();
        }
    }
}
