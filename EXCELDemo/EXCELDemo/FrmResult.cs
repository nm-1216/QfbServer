using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ExcelUp
{
    public partial class FrmResult : Form
    {
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
                new SqlParameter("@PageId", string.IsNullOrEmpty(txtPage.Text)?null:txtPage.Text),
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
            label4.Text = string.Format("共 {0} 条数据", x);
            label5.Text = string.Format("第 {0} 页", currPage);

            var pageCound = PageCount(x, pageSize);

            label7.Text = string.Format("共 {0} 页", pageCound);

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
    }
}
