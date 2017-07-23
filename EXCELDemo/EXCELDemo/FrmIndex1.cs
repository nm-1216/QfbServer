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
    public partial class FrmIndex1 : Form
    {
        private int id = 0;
        string ProjectNo = string.Empty;
        string PartNo = string.Empty;
        string PartName = string.Empty;

        Microsoft.Practices.EnterpriseLibrary.Data.Database db = Microsoft.Practices.EnterpriseLibrary.Data.DatabaseFactory.CreateDatabase("conn");


        public FrmIndex1(int Pid, string ProjectNo, string PartNo, string PartName)
        {
            this.id = Pid;
            this.ProjectNo = ProjectNo;
            this.PartNo = PartNo;
            this.PartName = PartName;
            InitializeComponent();
            GvList.AutoGenerateColumns = false;

        }

        private void bind()
        {
            SqlParameter[] p = new SqlParameter[] {
                new SqlParameter("@MeasReportID",id)
            };

            var dbc = db.GetSqlStringCommand("SELECT [MeasItemID],[MeasReportID],[MeasItemNO] FROM [MeasurementItems] where MeasReportID=@MeasReportID");
            dbc.Parameters.AddRange(p);
            var ds = db.ExecuteDataSet(dbc);


            GvList.DataSource = ds.Tables[0];
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

        private void FrmIndex1_Load(object sender, EventArgs e)
        {
            label2.Text = string.Format("项目号：{0}，零件号：{1}，零件名称：{2}", ProjectNo, PartNo, PartName);

            bind();
        }




        private void GvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var id = GvList.Rows[e.RowIndex].Cells["Iid"].Value.ToString();

                FrmIndex2 _frmIndex2 = new FrmIndex2(int.Parse(id), 1);
                _frmIndex2.Show();

            }
        }
    }
}
