using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using QfbServer.Models;

namespace ExcelUp
{
    public partial class FrmLogin : Form
    {
        Microsoft.Practices.EnterpriseLibrary.Data.Database db = Microsoft.Practices.EnterpriseLibrary.Data.DatabaseFactory.CreateDatabase("conn");


        public FrmLogin()
        {
            InitializeComponent();

            this.AcceptButton = btnOk;
            this.CancelButton = btnExit;
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text.Trim()) || string.IsNullOrWhiteSpace(txtPwd.Text))
            {
                MessageBox.Show("用户名，密码不能是空");
            }
            else
            {
                SqlParameter[] p = new SqlParameter[] {
                    new SqlParameter("@username",txtUser.Text.Trim()),
                    new SqlParameter("@userType",UserType.Application)
                };

                var dbc = db.GetSqlStringCommand("select * from Users where username=@username and userType=@userType");
                dbc.Parameters.AddRange(p);
                var ds= db.ExecuteDataSet(dbc);

                if (null != ds && ds.Tables[0].Rows.Count > 0)
                {
                    var pwd = ds.Tables[0].Rows[0]["password"].ToString();

                    if (txtPwd.Text.Trim() != pwd)
                    {
                        MessageBox.Show("用户名，密码不正确");
                    }
                    else
                    {
                        base.DialogResult = DialogResult.OK;
                    }
                }
                else
                {
                    MessageBox.Show("用户名，密码不正确");
                }
            }
        }
    }
}
