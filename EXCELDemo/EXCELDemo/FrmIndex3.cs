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
    public partial class FrmIndex3 : Form
    {
        Microsoft.Practices.EnterpriseLibrary.Data.Database db = Microsoft.Practices.EnterpriseLibrary.Data.DatabaseFactory.CreateDatabase("conn");


        public FrmIndex3()
        {
            InitializeComponent();
        }

        private void bindBll()
        {
            cbbStatus.SelectedIndex = 0;
        }

        private void dateTimePicker1_CloseUp(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
        }

        private void dateTimePicker2_CloseUp(object sender, EventArgs e)
        {
            dateTimePicker2.CustomFormat = "yyyy-MM-dd";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = " ";
            dateTimePicker2.CustomFormat = " ";
            cbbStatus.SelectedIndex = 0;

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            bind();
        }

        private void bind()
        {

            string status = "";
            string PreWwarningTimeStart = "";
            string PreWwarningTimeEnd = "";

            if (!cbbStatus.SelectedItem.ToString().Contains("请选择"))
            {
                if (cbbStatus.SelectedItem.ToString().Contains("未处理"))
                {
                    status = "0";
                }
                else
                {
                    status = "1";
                }
            }

            if (!(dateTimePicker1.CustomFormat == " "))
            {
                PreWwarningTimeStart = dateTimePicker1.Value.ToString().Trim();
            }

            if (!(dateTimePicker2.CustomFormat == " "))
            {
                PreWwarningTimeEnd = dateTimePicker2.Value.ToString().Trim();
            }


            SqlParameter[] p = new SqlParameter[] {
                new SqlParameter("@PreWwarningTimeStart",PreWwarningTimeStart),
                new SqlParameter("@PreWwarningTimeEnd",PreWwarningTimeEnd),
                new SqlParameter("@status",status)
            };


            string sql = @"SELECT [id],[Message],[Filed1],[Filed2],[WarningType],[PreWwarningTime],[DoTime],case when status = 0 then '未处理' else '已处理' end as [status] FROM [dbo].[Warnings] 

            where 
            (''=@PreWwarningTimeStart or ''=@PreWwarningTimeEnd 
            or (PreWwarningTime>=@PreWwarningTimeStart and PreWwarningTime<=@PreWwarningTimeEnd)
            )

            
            
            and (''=@status or status=@status)

            ";

            var dbc = db.GetSqlStringCommand(sql);
            dbc.Parameters.AddRange(p);
            var ds = db.ExecuteDataSet(dbc);


            GvList.DataSource = ds.Tables[0];
        }

        private void FrmIndex3_Load(object sender, EventArgs e)
        {
            bindBll();
            GvList.AutoGenerateColumns = false;
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

                    if (temp.DefaultCellStyle.NullValue.ToString() == "处理")
                    {
                        if (MessageBox.Show("你确定要设置成已经处理吗！", "Confirm Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {

                            SqlParameter[] p = new SqlParameter[] {
                                new SqlParameter("@id",id),
                            };

                            var dbc = db.GetSqlStringCommand("UPDATE [dbo].[Warnings] SET [DoTime] = getdate(),[status] = 1 WHERE id=@id");
                            dbc.Parameters.AddRange(p);
                            var number = db.ExecuteNonQuery(dbc);
                            if (number > 0)
                            {
                                bind();
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
    }
}
