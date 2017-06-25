using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;

namespace EXCELDemo
{
    public class SqlHelper
    {
        private string exePath = System.Environment.CurrentDirectory;//本程序所在路径
        //创建连接对象
        private string ConnectionString;
        public SqlHelper()
        {
            ConnectionString = "provider=Microsoft.ACE.OLEDB.12.0;data source=" + exePath + @"\MEASSYS.accdb;Persist Security Info=False";
        }

        /// <summary>
        /// 获取数据表/查询
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public DataTable FillDataTable(string strSql)
        {
            using (OleDbConnection conn = new OleDbConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    OleDbDataAdapter da = new OleDbDataAdapter(strSql, conn); //创建适配对象
                    DataTable dt = new DataTable(); //新建表对象
                    da.Fill(dt); //用适配对象填充表对象
                    return dt;
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        ///  增/删/改
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string strSql)
        {
            using (OleDbConnection conn = new OleDbConnection(ConnectionString))
            {
                conn.Open();
                OleDbCommand comm = new OleDbCommand(strSql, conn);
                int i = comm.ExecuteNonQuery();
                return i;
            }
        }

        /// <summary>
        ///  操作OLE对象
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string strSql, byte[] data)
        {
            using (OleDbConnection conn = new OleDbConnection(ConnectionString))
            {
                conn.Open();
                OleDbCommand command = new OleDbCommand();
                command.CommandText = strSql;
                command.Parameters.AddWithValue("@Picture", data);
                command.Connection = conn;
                int i = command.ExecuteNonQuery();
                return i;
            }
        }

    }
}
