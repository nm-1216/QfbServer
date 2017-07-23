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

                    var url = ApiUrl + "/api/CreateJson?ids=" + ids + "&username=" + user;

                    Os.Brain.Net.UrlRequest.GetText(url);

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
