using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ExcelUp
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FrmLogin login = new FrmLogin();
            DialogResult loginDialogResult = login.ShowDialog();
            if (loginDialogResult == DialogResult.OK)
            {
                Application.Run(new FrmIndex());
            }
        }
    }
}
