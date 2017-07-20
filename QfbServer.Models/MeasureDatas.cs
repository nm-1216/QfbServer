//----------------------------------------------------------------------------------------------------------------------------
// <copyright file="MeasureDatas.cs" company="Os.Brain">Copyright (c) Os.Brain. All rights reserved.</copyright>
// <author>Craze</author>
// <datetime>2017/07/13 14:44:34</datetime>
// <discription>
// </discription>
//----------------------------------------------------------------------------------------------------------------------------


namespace QfbServer.Dal
{
    using Os.Brain.Data.Dal;

    /// <summary>
    /// MeasureDatas dal接口
    /// </summary>
    internal partial interface IMeasureDatas : IBasicDal<QfbServer.Models.MeasureData>
    {
        /// <summary>
        /// Gets or sets Action 操作
        /// </summary>
        Os.Brain.Data.DataActions Action
        {
            get;
            set;
        }
    }
}
namespace QfbServer.Dal
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Diagnostics;

    using Microsoft.Practices.EnterpriseLibrary.Data;

    using Os.Brain.Data;
    using Os.Brain.Data.MsSQL;

    /// <summary>
    /// MeasureDatas dal数据处理层
    /// </summary>
    internal partial class MeasureDatas: IMeasureDatas
    {
        /// <summary>
        /// 私有 数据操作行为
        /// </summary>
        private DataActions _action;

        /// <summary>
        /// Gets or sets Action 数据操作行为
        /// </summary>
        public DataActions Action
        {
            get { return this._action; }
            set { this._action = value; }
        }

        /// <summary>
        /// Edit 编辑（新增，修改）一条数据
        /// </summary>
        /// <param name="model">model 实体</param>
        /// <returns>返回 实体主键</returns>
		public object Edit(QfbServer.Models.MeasureData model)
        {
            

            return null;
        }

        /// <summary>
        /// Delete 删除记录集
        /// </summary>
        /// <param name="ids">ids 编号集</param>
        /// <returns>返回 受影响条数</returns>
		public int Delete(string ids)
        {
            #region DEBUG
            Debug.WriteLine(string.Format(TSQL.DEBUG_START_LINE,"com.Dal.Delete START"));
            Debug.WriteLine(string.Format(TSQL.DELETE_LIST,"[dbo].[MeasureDatas]","[Id]","@Id"));
            Debug.WriteLine("@Id="+"," + ids.Trim(',') + ",");
            Debug.WriteLine(string.Format(TSQL.DEBUG_END_LINE,"com.Dal.Delete END"));
            #endregion

            if (DataActions.delete != this.Action)
            {
                return 0;
            }

            if (ids.Length <= 0)
            {
                return 0;
            }

            Database db = DatabaseFactory.CreateDatabase(QfbServer.Models.MeasureData.CONN);
            DbCommand dbc= db.GetSqlStringCommand(string.Format(TSQL.DELETE_LIST,"[dbo].[MeasureDatas]","[Id]","@Id"));

            db.AddInParameter(dbc, "@Id", DbType.String, "," + ids.Trim(',') + ",");

            return db.ExecuteNonQuery(dbc);
        }

        /// <summary>
        /// GetItem 获取单个实体
        /// </summary>
        /// <param name="ids">ids 主键值</param>
        /// <returns>返回 空或实体</returns>
		public QfbServer.Models.MeasureData GetItem(string ids)
        {
            #region DEBUG
            Debug.WriteLine(string.Format(TSQL.DEBUG_START_LINE,"com.Dal.GetItem START"));
            Debug.WriteLine(string.Format(TSQL.SELECT_ITEM,"[dbo].[MeasureDatas]","[Id]","@Id"));
            Debug.WriteLine(string.Format(TSQL.DEBUG_END_LINE,"com.Dal.GetItem END"));
            #endregion

            if (DataActions.select != this.Action)
            {
                return null;
            }

            QfbServer.Models.MeasureData _model = null;

            Database db = DatabaseFactory.CreateDatabase(QfbServer.Models.MeasureData.CONN);
            DbCommand dbc = db.GetSqlStringCommand(string.Format(TSQL.SELECT_ITEM,"[dbo].[MeasureDatas]","[Id]","@Id"));

            db.AddInParameter(dbc, "@Id", DbType.Int32, ids);

            using (IDataReader dr = db.ExecuteReader(dbc))
            {
                if (dr.Read())
                {
                    _model = new QfbServer.Models.MeasureData();

                    LoadFromReader(dr,_model);
                }
            }

            return _model;
        }

        /// <summary>
        /// GetList 获取数据集
        /// </summary>
        /// <param name="dataParams">dataParams 查询参数</param>
        /// <returns>返回 数据集</returns>
        public IList<QfbServer.Models.MeasureData> GetList(params DbParameter[] dataParams)
        {
            if (DataActions.select != this.Action)
            {
                return null;
            }

            IList<QfbServer.Models.MeasureData> returnList = new List<QfbServer.Models.MeasureData>();
            QfbServer.Models.MeasureData _model = null;

            Database db = DatabaseFactory.CreateDatabase(QfbServer.Models.MeasureData.CONN);
            DbCommand dbc = db.GetStoredProcCommand("MeasureDatas_Get");

            if (null != dataParams)
            {
                dbc.Parameters.AddRange(dataParams);
            }

            using (IDataReader dr = db.ExecuteReader(dbc))
            {
                while (dr.Read())
                {
                    _model = new QfbServer.Models.MeasureData();

                    LoadFromReader(dr,_model);

                    returnList.Add(_model);
                }
            }

            return returnList;
        }

        /// <summary>
        /// GetList 获取分页数据集
        /// </summary>
        /// <param name="pageSize">pageSize 每页条数</param>
        /// <param name="currPage">currPage 当前页码</param>
        /// <param name="recordCount">recordCount 总记录数</param>
        /// <param name="dataParams">dataParams 查询参数</param>
        /// <returns>返回 数据集</returns>
        public IList<QfbServer.Models.MeasureData> GetList(int pageSize, int currPage, out int recordCount, params DbParameter[] dataParams)
        {
            recordCount=0;
            if (DataActions.select != this.Action)
            {
                return null;
            }

            IList<QfbServer.Models.MeasureData> returnList = new List<QfbServer.Models.MeasureData>();
            QfbServer.Models.MeasureData _model = null;

            Database db = DatabaseFactory.CreateDatabase(QfbServer.Models.MeasureData.CONN);
            DbCommand dbc = db.GetStoredProcCommand("MeasureDatas_Get");

            if (null != dataParams)
            {
                dbc.Parameters.AddRange(dataParams);
            }

            db.AddInParameter(dbc, "@PageIndex", DbType.Int32, currPage);
            db.AddInParameter(dbc, "@PageSize", DbType.Int32, pageSize);
            db.AddOutParameter(dbc, "@RecordCount", DbType.Int32, 4);

            using (IDataReader dr = db.ExecuteReader(dbc))
            {
                while (dr.Read())
                {
                    _model = new QfbServer.Models.MeasureData();

                    LoadFromReader(dr,_model);

                    returnList.Add(_model);
                }
            }

            recordCount = (int)dbc.Parameters["@RecordCount"].Value;

            return returnList;
        }

        /// <summary>
        /// DataSet 获取数据集
        /// </summary>
        /// <param name="dataParams">dataParams 查询参数</param>
        /// <returns>返回 数据集</returns>
        public DataSet GetDataSet(params DbParameter[] dataParams)
        {
            if (DataActions.select != this.Action)
            {
                return null;
            }

            Database db = DatabaseFactory.CreateDatabase(QfbServer.Models.MeasureData.CONN);
            DbCommand dbc = db.GetStoredProcCommand("MeasureDatas_Get");

            if (null != dataParams)
            {
                dbc.Parameters.AddRange(dataParams);
            }

            return db.ExecuteDataSet(dbc);
        }

        /// <summary>
        /// GetDataSet 获取分页数据集
        /// </summary>
        /// <param name="pageSize">pageSize 每页条数</param>
        /// <param name="currPage">currPage 当前页码</param>
        /// <param name="recordCount">recordCount 总记录数</param>
        /// <param name="dataParams">dataParams 查询参数</param>
        /// <returns>返回 数据集</returns>
        public DataSet GetDataSet(int pageSize, int currPage, out int recordCount, params DbParameter[] dataParams)
        {
            recordCount=0;
            if (DataActions.select != this.Action)
            {
                return null;
            }

            Database db = DatabaseFactory.CreateDatabase(QfbServer.Models.MeasureData.CONN);
            DbCommand dbc = db.GetStoredProcCommand("MeasureDatas_Get");

            if (null != dataParams)
            {
                dbc.Parameters.AddRange(dataParams);
            }

            db.AddInParameter(dbc, "@PageIndex", DbType.Int32, currPage);
            db.AddInParameter(dbc, "@PageSize", DbType.Int32, pageSize);
            db.AddOutParameter(dbc, "@RecordCount", DbType.Int32, 4);

            var ds=db.ExecuteDataSet(dbc);

            recordCount = (int)dbc.Parameters["@RecordCount"].Value;

            return ds;
        }

        /// <summary>
        /// GetDataSet 获取分页数据集
        /// </summary>
        /// <param name="pageSize">pageSize 每页条数</param>
        /// <param name="currPage">currPage 当前页码</param>
        /// <param name="recordCount">recordCount 总记录数</param>
        /// <param name="dataParams">dataParams 查询参数</param>
        /// <returns>返回 数据集</returns>
        public DataSet GetDataSet(int pageSize, int currPage, out int recordCount,string strWhere, params DbParameter[] dataParams)
        {
            recordCount=0;
            if (DataActions.select != this.Action)
            {
                return null;
            }

            proc_common_GetRecord pcg = new proc_common_GetRecord("[dbo].[MeasureDatas]", "Id", "*", strWhere, string.Empty, pageSize, currPage);

            Database db = DatabaseFactory.CreateDatabase(QfbServer.Models.MeasureData.CONN);
            DbCommand dbc = db.GetSqlStringCommand(pcg.TSQL);

            if (null != dataParams)
            {
                dbc.Parameters.AddRange(dataParams);
            }

            var ds=db.ExecuteDataSet(dbc);

            recordCount = int.Parse(ds.Tables[0].Rows[0][0].ToString());

            return ds;
        }

        protected void LoadFromReader(IDataReader dr,QfbServer.Models.MeasureData model)
        {
        	if (dr != null && !dr.IsClosed)
        	{
        		if (!dr.IsDBNull(0)) model.Id = dr.GetInt32(0);
        		if (!dr.IsDBNull(1)) model.ProjectId = dr.GetInt32(1);
        		if (!dr.IsDBNull(2)) model.ProjectName = dr.GetString(2);
        		if (!dr.IsDBNull(3)) model.ProductId = dr.GetInt32(3);
        		if (!dr.IsDBNull(4)) model.ProductName = dr.GetString(4);
        		if (!dr.IsDBNull(5)) model.TargetId = dr.GetInt32(5);
        		if (!dr.IsDBNull(6)) model.TargetName = dr.GetString(6);
        		if (!dr.IsDBNull(7)) model.TargetType = dr.GetString(7);
        		if (!dr.IsDBNull(8)) model.PageId = dr.GetInt32(8);
        		if (!dr.IsDBNull(9)) model.MeasurePoint = dr.GetString(9);
        		if (!dr.IsDBNull(10)) model.Direction = dr.GetString(10);
        		if (!dr.IsDBNull(11)) model.Value1 = dr.GetString(11);
        		if (!dr.IsDBNull(12)) model.Value2 = dr.GetString(12);
        		if (!dr.IsDBNull(13)) model.Value3 = dr.GetString(13);
        		if (!dr.IsDBNull(14)) model.Value4 = dr.GetString(14);
        		if (!dr.IsDBNull(15)) model.Username = dr.GetString(15);
        		if (!dr.IsDBNull(16)) model.Timestamp = dr.GetInt64(16);
        		if (!dr.IsDBNull(17)) model.UpperTolerance = dr.GetString(17);
        		if (!dr.IsDBNull(18)) model.LowerTolerance = dr.GetString(18);
        		if (!dr.IsDBNull(19)) model.Value5 = dr.GetString(19);
        		if (!dr.IsDBNull(20)) model.Value6 = dr.GetString(20);
        		if (!dr.IsDBNull(21)) model.Value7 = dr.GetString(21);
        		if (!dr.IsDBNull(22)) model.Value8 = dr.GetString(22);
        		if (!dr.IsDBNull(23)) model.Value9 = dr.GetString(23);
        		if (!dr.IsDBNull(24)) model.Value10 = dr.GetString(24);
        	}
        }

        protected void LoadFromReader1(IDataReader dr,QfbServer.Models.MeasureData model)
        {
        	if (dr != null && !dr.IsClosed)
        	{
                model.Id = (int)dr["Id"];
                model.ProjectId = (int)dr["ProjectId"];
                model.ProjectName = dr["ProjectName"].ToString();
                model.ProductId = (int)dr["ProductId"];
                model.ProductName = dr["ProductName"].ToString();
                model.TargetId = (int)dr["TargetId"];
                model.TargetName = dr["TargetName"].ToString();
                model.TargetType = dr["TargetType"].ToString();
                model.PageId = (int)dr["PageId"];
                model.MeasurePoint = dr["MeasurePoint"].ToString();
                model.Direction = dr["Direction"].ToString();
                model.Value1 = dr["Value1"].ToString();
                model.Value2 = dr["Value2"].ToString();
                model.Value3 = dr["Value3"].ToString();
                model.Value4 = dr["Value4"].ToString();
                model.Username = dr["Username"].ToString();
                model.Timestamp = (long)dr["Timestamp"];
                model.UpperTolerance = dr["UpperTolerance"].ToString();
                model.LowerTolerance = dr["LowerTolerance"].ToString();
                model.Value5 = dr["Value5"].ToString();
                model.Value6 = dr["Value6"].ToString();
                model.Value7 = dr["Value7"].ToString();
                model.Value8 = dr["Value8"].ToString();
                model.Value9 = dr["Value9"].ToString();
                model.Value10 = dr["Value10"].ToString();
        	}
        }
    }
}
namespace QfbServer.Bll
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data;
    using System.Xml;

    /// <summary>
    /// MeasureDatas 业务逻辑层
    /// </summary>
    public partial class MeasureDatas
    {
        /// <summary>
        /// dal 获取接口 IMeasureDatas
        /// </summary>
        private readonly QfbServer.Dal.IMeasureDatas dal=DalFactory.CreateMeasureDatas();

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureDatas"/> class.
        /// </summary>
        /// <param name="action">action 操作符</param>
        public MeasureDatas(Os.Brain.Data.DataActions action)
        {
            this.dal.Action=action;
        }

        /// <summary>
        /// Edit 编辑（新增，修改）一条数据
        /// </summary>
        /// <param name="model">model 实体</param>
        /// <returns>returns 实体主键</returns>
		public object Edit(QfbServer.Models.MeasureData model)
        {
            return this.dal.Edit(model);
        }

        /// <summary>
        /// Delete 删除记录集
        /// </summary>
        /// <param name="ids">ids 编号集</param>
        /// <returns>returns 受影响条数</returns>
		public int Delete(string ids)
        {
            return this.dal.Delete(ids);
        }

        /// <summary>
        /// Read 获取单个实体
        /// </summary>
        /// <param name="ids">ids 主键值</param>
        /// <returns>returns 空或实体</returns>
		public QfbServer.Models.MeasureData Read(string ids)
        {
            return this.dal.GetItem(ids);
        }

        /// <summary>
        /// GetList 获取数据集
        /// </summary>
        /// <param name="dataParams">dataParams 查询参数</param>
        /// <returns>returns 数据集</returns>
		public IList<QfbServer.Models.MeasureData> GetList(DbParameter[] dataParams)
        {
            return this.dal.GetList(dataParams);
        }

        /// <summary>
        /// GetList 获取分页数据集
        /// </summary>
        /// <param name="pageSize">pageSize 每页条数</param>
        /// <param name="currPage">currPage 当前页码</param>
        /// <param name="recordCount">recordCount 总记录数</param>
        /// <param name="dataParams">dataParams 查询参数</param>
        /// <returns>returns 数据集</returns>
		public IList<QfbServer.Models.MeasureData> GetList(int pageSize, int currPage,out int recordCount, DbParameter[] dataParams)
        {
            return this.dal.GetList(pageSize, currPage,out recordCount,dataParams);
        }

        /// <summary>
        /// GetDataSet 获取数据集
        /// </summary>
        /// <param name="dataParams">dataParams 查询参数</param>
        /// <returns>returns 数据集</returns>
		public DataSet GetDataSet(DbParameter[] dataParams)
        {
            return this.dal.GetDataSet(dataParams);
        }

        /// <summary>
        /// GetDataSet 获取分页数据集
        /// </summary>
        /// <param name="pageSize">pageSize 每页条数</param>
        /// <param name="currPage">currPage 当前页码</param>
        /// <param name="recordCount">recordCount 总记录数</param>
        /// <param name="dataParams">dataParams 查询参数</param>
        /// <returns>returns 数据集</returns>
		public DataSet GetDataSet(int pageSize, int currPage,out int recordCount, DbParameter[] dataParams)
        {
            return this.dal.GetDataSet(pageSize, currPage,out recordCount,dataParams);
        }
    }
}
namespace QfbServer
{
    /// <summary>
    /// MeasureDatas 工厂
    /// </summary>
    internal sealed partial class DalFactory
    {
        /// <summary>
        /// CreateMeasureDatas 工厂构造器
        /// </summary>
        /// <returns>返回 实体</returns>
        public static QfbServer.Dal.IMeasureDatas CreateMeasureDatas()
        {
            return new QfbServer.Dal.MeasureDatas();
        }
    }
}





