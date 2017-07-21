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
using QfbServer.Models;

namespace ExcelUp
{
    public partial class FrmUser : Form
    {
        public FrmUser()
        {
            InitializeComponent();
            this.dgvList.AutoGenerateColumns = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {


            if (MessageBox.Show("确定要添加新用户吗 ?", "Confirm Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(txtUser.Text.Trim()) || string.IsNullOrEmpty(txtPwd.Text.Trim()))
                {
                    MessageBox.Show("用户名密码不能是空");
                    return;
                }
                var bll = new QfbServer.Bll.Users(Os.Brain.Data.DataActions.select);

                SqlParameter[] p = new SqlParameter[] {
                new SqlParameter("@username", txtUser.Text)
            };

                var list = bll.GetList(p);

                if (null != list && list.Count > 0)
                {
                    MessageBox.Show("该用户已经存在");
                    return;
                }

                var temp = new User() { username = txtUser.Text.Trim(), password = txtPwd.Text.Trim(), userType = rbtnPad.Checked ? QfbServer.Models.UserType.Pad : QfbServer.Models.UserType.Application };

                try
                {
                    temp.Insert();
                    bind();

                    //MessageBox.Show("添加成功");

                    txtPwd.Text = string.Empty;
                    txtUser.Text = string.Empty;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }


        }

        private void bind()
        {
            var bll = new QfbServer.Bll.Users(Os.Brain.Data.DataActions.select);

            dgvList.DataSource = bll.GetDataSet(null).Tables[0];
        }

        private void FrmUser_Load(object sender, EventArgs e)
        {
            bind();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除用户吗 ?", "Confirm Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                var temp = dgvList.SelectedRows;
                if (temp.Count == 0)
                {
                    MessageBox.Show("请选择要删除的行");
                    return;
                }

                var id = dgvList.SelectedRows[0].Cells[0].Value.ToString();

                var bll = new QfbServer.Bll.Users(Os.Brain.Data.DataActions.select);

                var model=bll.Read(id);

                if (null != model)
                {
                    model.Delete();
                    bind();
                    //MessageBox.Show("删除成功");

                }
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
                    case 0xF012:
                    case 0xF010:
                        m.WParam = IntPtr.Zero;
                        break;

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

        private void rbtnPad_CheckedChanged(object sender, EventArgs e)
        {
            rBtnApplication.Checked = !rbtnPad.Checked;
        }

        private void rBtnApplication_CheckedChanged(object sender, EventArgs e)
        {
            rbtnPad.Checked = !rBtnApplication.Checked;
        }
    }
}
