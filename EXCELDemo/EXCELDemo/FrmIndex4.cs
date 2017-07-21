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
    public partial class FrmIndex4 : Form
    {
        Microsoft.Practices.EnterpriseLibrary.Data.Database db = Microsoft.Practices.EnterpriseLibrary.Data.DatabaseFactory.CreateDatabase("conn");

        public FrmIndex4()
        {
            InitializeComponent();
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GvBind();

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

        private void FrmIndex4_Load(object sender, EventArgs e)
        {
            bind();
            BindUser();
            this.AcceptButton = btnSearch;
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

        private void BindUser()
        {
            string strSqlReport = "SELECT [username] text  FROM [Users] where [userType]=1";
            var ds = db.ExecuteDataSet(CommandType.Text, strSqlReport);

            if (null != ds)
            {
                var dt = ds.Tables[0];

                cboxUser.ValueMember = "text";
                cboxUser.DisplayMember = "text";
                cboxUser.DataSource = dt;
            }
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


        private void cboxAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < this.GvList.Rows.Count; i++)
            {                
                this.GvList["CheckBox", i].Value = cboxAll.Checked;
            }
        }


        public string ApiUrl = System.Configuration.ConfigurationManager.AppSettings["ApiUrl"].ToString();


        private void btnDown_Click(object sender, EventArgs e)
        {
           if (MessageBox.Show("生成新的JSON文件吗 ?", "Confirm Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    StringBuilder idList = new StringBuilder();

                    foreach (DataGridViewRow row in this.GvList.Rows)
                    {
                        DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)row.Cells["CheckBox"];
                        var id = row.Cells["id"].Value.ToString();

                        if (Convert.ToBoolean(checkCell.Value) == true)
                        {
                            idList.AppendFormat("{0},", id);
                        }
                    }

                    string ids = idList.ToString().Trim(',');
                    string user = cboxUser.SelectedValue.ToString().Trim();

                    if (string.IsNullOrEmpty(ids))
                    {
                        MessageBox.Show("请选择项目");
                        return;
                    }

                    if (string.IsNullOrEmpty(user))
                    {
                        MessageBox.Show("请选下发接收用户");
                        return;
                    }

                    Os.Brain.Net.UrlRequest.GetText(ApiUrl + "/api/CreateJson?ids=" + ids + "&username=" + user);

                    MessageBox.Show("生成成功");
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.Message);
                }
            }
        }
    }
}
