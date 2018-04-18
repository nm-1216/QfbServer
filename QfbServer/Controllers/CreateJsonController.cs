using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using QfbServer.Models;

namespace QfbServer.Controllers
{
    public class CreateJsonController : ApiController
    {
        private QfbServerContext db = new QfbServerContext();

        Microsoft.Practices.EnterpriseLibrary.Data.Database _db = Microsoft.Practices.EnterpriseLibrary.Data.DatabaseFactory.CreateDatabase("conn");


        List<string> PageName = new List<string>() { "TRIM", "SURFACE" };


        private JsonPages GetJsonPages(DataTable tab1, DataTable tab2, int id, int pid, string SheetName, string ProductName, string ProjectName)
        {

            string filter = "PointSetting_ProjectSetting_ID = " + id + " and PointSetting_PageNum= " + pid;
            var list = new List<JsonPoints>() { };

            DataRow[] drArr = tab2.Select(filter);

            foreach (var item in drArr)
            {
                list.Add(
                     new JsonPoints()
                     {
                         direction = item["PointSetting_Direct"].ToString(),
                         pointId = int.Parse(item["PointSetting_ID"].ToString()),
                         index = int.Parse(item["PointSetting_index"].ToString()),
                         point = item["PointSetting_PointNO"].ToString(),
                         lowerTolerance = item["PointSetting_LowerTolerance"].ToString().Length > 0 ? (decimal.Parse(item["PointSetting_LowerTolerance"].ToString())).ToString("0.#####") : item["PointSetting_LowerTolerance"].ToString(),
                         upperTolerance = item["PointSetting_UpperTolerance"].ToString().Length > 0 ? (decimal.Parse(item["PointSetting_UpperTolerance"].ToString())).ToString("0.#####") : item["PointSetting_UpperTolerance"].ToString(),
                     }

                    );
            }

            var model = new JsonPages()
            {
                page_id = id * 100 + pid,//保证PageId 唯一 乘以100 留出 100的空间，例如 1*100 与2*100 有100空挡 给页码
                page_name = pid.ToString(),
                pictures = new string[] { string.Format("{0}_{1}_{2}_{3}.png", ProjectName, ProductName, SheetName, pid) },
                measure_points = list.AsQueryable()
            };

            return model;
        }

        private JsonTargets GetJsonTargets(DataTable tab1, DataTable tab2, string name, string product, string project, int id)
        {
            DataTable MeasurementReport = tab2.DefaultView.ToTable(true, "PointSetting_ProjectSetting_ID", "PointSetting_PageNum", "PointSetting_PicResources_ID");

            string filter = "PointSetting_ProjectSetting_ID = " + id.ToString();

            var list = new List<JsonPages>() { };

            DataRow[] drArr = MeasurementReport.Select(filter);

            foreach (var item in drArr)
            {
                list.Add(GetJsonPages(tab1, tab2, id,
                    int.Parse(item["PointSetting_PageNum"].ToString()),
                    name,
                    product,
                    project
                    )
                );
            }


            var model = new JsonTargets()
            {
                target_id = id,
                target_name = name,
                value_type = PageName.Contains(name.ToUpper()) ? "data" : "OK,NG",
                pages = list.AsQueryable()
            };

            return model;
        }


        private JsonProducts GetJsonProducts(DataTable tab1, DataTable tab2, int index, string No, string name, string pname)
        {
            //string filter = "ProjectSetting_ProjectNO = '" + name + "'";
            string filter = "ProjectSetting_ProjectNO = '" + pname + "' and ProjectSetting_PartsNO = '" + No + "'";

            var jsonTargets = new List<JsonTargets>() { };

            DataRow[] drArr = tab1.Select(filter);

            foreach (var item in drArr)
            {
                jsonTargets.Add(GetJsonTargets(tab1, tab2, item["ProjectSetting_ItemNO"].ToString(), No, pname, int.Parse(item["ProjectSetting_ID"].ToString())));
            }


            var model = new JsonProducts()
            {
                product_id = index,
                product_name = name + "(" + No + ")",
                targets = jsonTargets.AsQueryable()
            };

            return model;
        }

        private JsonProject GetJsonProject(DataTable tab1, DataTable tab2, int index, string name)
        {
            DataTable MeasurementReport = tab1.DefaultView.ToTable(true, "ProjectSetting_ProjectNO", "ProjectSetting_PartsNO", "ProjectSetting_PartsName", "ProductId");

            string filter = "ProjectSetting_ProjectNO = '" + name + "'";
            var jsonProducts = new List<JsonProducts>() { };

            DataRow[] drArr = MeasurementReport.Select(filter);

            foreach (var item in drArr)
            {
                jsonProducts.Add(GetJsonProducts(tab1, tab2, int.Parse(item["ProductId"].ToString()), item["ProjectSetting_PartsNO"].ToString(), item["ProjectSetting_PartsName"].ToString(), name));
            }

            var model = new JsonProject()
            {
                project_id = index,
                project_name = name,
                products = jsonProducts.AsQueryable()
            };

            return model;
        }

        // GET: api/MeasureDatas
        public IQueryable<JsonProject> GetCreateJson(string ids, string username)
        {
            try
            {

                string sql = @"
                select a.* from [Vw_ProjectSettingWithId] a
                left join [Vw_ProjectSettingWithId] b on a.ProjectSetting_ProjectNO=b.ProjectSetting_ProjectNO and a.ProjectSetting_PartsNO=b.ProjectSetting_PartsNO
                where b.ProjectSetting_ID in ({0});
                select b.* from [PointSetting] b
                left join [ProjectSetting] c on b.PointSetting_ProjectSetting_ID=c.ProjectSetting_ID
                left join [ProjectSetting] d on c.ProjectSetting_ProjectNO=d.ProjectSetting_ProjectNO and c.ProjectSetting_PartsNO =d.ProjectSetting_PartsNO
                where d.ProjectSetting_ID in ({0}) and b.PointSetting_IsDeleted=0 order by b.PointSetting_Index asc ;
                select [UserInfo_RealName],[UserInfo_password] from [User_Info] where [UserInfo_Type]=2
            ";

                var ds = _db.ExecuteDataSet(CommandType.Text, string.Format(sql, ids));

                var tab1 = ds.Tables[0];
                var tab2 = ds.Tables[1];
                var tab3 = ds.Tables[2];

                var jsonProject = new List<JsonProject>() { };


                DataTable MeasurementReport = tab1.DefaultView.ToTable(true, "ProjectSetting_ProjectNO", "ProjectId");


                foreach (DataRow row in MeasurementReport.Rows)
                {
                    jsonProject.Add(GetJsonProject(tab1, tab2, int.Parse(row["ProjectId"].ToString()), row["ProjectSetting_ProjectNO"].ToString()));
                }


                List<JsonUsers> jsonUser = new List<JsonUsers>() { };
                foreach (DataRow row in tab3.Rows)
                {
                    jsonUser.Add(new JsonUsers()
                    {
                        user_id = 0,
                        username = row["UserInfo_RealName"].ToString(),
                        password = row["UserInfo_password"].ToString()
                    });
                }


                JavaScriptSerializer jsonSerialize = new JavaScriptSerializer();

                #region project
                var TEMP = jsonSerialize.Serialize(jsonProject);

                var mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/res/" + username + ".json");

                if (System.IO.File.Exists(mappedPath))
                {
                    System.IO.File.Delete(mappedPath);
                }

                FileStream fs = new FileStream(mappedPath, FileMode.Create);
                //获得字节数组
                //byte[] data = System.Text.Encoding.Default.GetBytes(TEMP.Replace("\\u0026", "&"));
                byte[] data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(TEMP.Replace("\\u0026", "&"));
                //开始写入
                fs.Write(data, 0, data.Length);
                //清空缓冲区、关闭流
                fs.Flush();
                fs.Close();

                #endregion

                #region User
                var tempUser = jsonSerialize.Serialize(jsonUser);

                var userpath = System.Web.Hosting.HostingEnvironment.MapPath("~/res/user.json");

                if (System.IO.File.Exists(userpath))
                {
                    System.IO.File.Delete(userpath);
                }

                FileStream fsUser = new FileStream(userpath, FileMode.Create);
                //获得字节数组
                //byte[] data = System.Text.Encoding.Default.GetBytes(TEMP.Replace("\\u0026", "&"));
                byte[] data1 = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(tempUser.Replace("\\u0026", "&"));
                //开始写入
                fsUser.Write(data1, 0, data1.Length);
                //清空缓冲区、关闭流
                fsUser.Flush();
                fsUser.Close();


                return jsonProject.AsQueryable();

                #endregion
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        //public IQueryable<JsonProject> GetCreateJson1(string ids, string username)
        //{

        //    var xxx = ids.Split(',');

        //    var intArray = Array.ConvertAll<string, int>(xxx, s => int.Parse(s));

        //    var _JsonProject = db.MeasurementReport.Where(b => intArray.Contains(b.MeasReportID)).GroupBy(b => b.ProjectNo).Select(b => new JsonProject()
        //    {
        //        project_id = b.Min(a => a.MeasReportID),
        //        project_name = b.Key,
        //        products = db.MeasurementReport.Where(c => c.ProjectNo == b.Key && intArray.Contains(c.MeasReportID)).Select(d => new JsonProducts()
        //        {
        //            product_id = d.MeasReportID,
        //            product_name = d.PartName + "(" + d.PartNo + ")",
        //            targets = db.MeasurementItem.Where(f => f.MeasReportID == d.MeasReportID).Select(e => new JsonTargets()
        //            {
        //                target_id = e.MeasItemID,
        //                target_name = e.MeasItemName,
        //                value_type = PageName.Contains(e.MeasItemName.ToUpper()) ? "data" : "OK,NG",
        //                pages = db.MeasurementPage.Where(g => g.MeasItemID == e.MeasItemID).Select(i => new JsonPages()
        //                {
        //                    page_id = i.MeasPageID,
        //                    page_name = i.MeasPageNo,
        //                    pictures = new List<string>() { (b.Key + "_" + d.PartNo + "_" + e.MeasItemName + "_" + i.MeasPageNo.ToString() + ".png") },
        //                    measure_points = db.MeasurementPoint.Where(j => j.MeasPageID == i.MeasPageID).Select(k => new JsonPoints()
        //                    {
        //                        direction = k.Direct,
        //                        pointId = k.MeasPointID,
        //                        point = k.PointNo,
        //                        lowerTolerance = k.LowerTol,
        //                        upperTolerance = k.UpperTol
        //                    })

        //                })
        //            })
        //        })
        //    });

        //    var user = db.Users.Where(b => b.userType == UserType.Pad).Select(b => new { user_id = b.Id, username = b.username, password = b.password });

        //    JavaScriptSerializer jsonSerialize = new JavaScriptSerializer();

        //    #region project
        //    var TEMP = jsonSerialize.Serialize(_JsonProject);

        //    var mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/res/" + username + ".json");

        //    if (File.Exists(mappedPath))
        //    {
        //        File.Delete(mappedPath);
        //    }

        //    FileStream fs = new FileStream(mappedPath, FileMode.Create);
        //    //获得字节数组
        //    //byte[] data = System.Text.Encoding.Default.GetBytes(TEMP.Replace("\\u0026", "&"));
        //    byte[] data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(TEMP.Replace("\\u0026", "&"));
        //    //开始写入
        //    fs.Write(data, 0, data.Length);
        //    //清空缓冲区、关闭流
        //    fs.Flush();
        //    fs.Close();

        //    #endregion

        //    #region User
        //    var tempUser = jsonSerialize.Serialize(user);

        //    var userpath = System.Web.Hosting.HostingEnvironment.MapPath("~/res/user.json");

        //    if (File.Exists(userpath))
        //    {
        //        File.Delete(userpath);
        //    }

        //    FileStream fsUser = new FileStream(userpath, FileMode.Create);
        //    //获得字节数组
        //    //byte[] data = System.Text.Encoding.Default.GetBytes(TEMP.Replace("\\u0026", "&"));
        //    byte[] data1 = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(tempUser.Replace("\\u0026", "&"));
        //    //开始写入
        //    fsUser.Write(data1, 0, data1.Length);
        //    //清空缓冲区、关闭流
        //    fsUser.Flush();
        //    fsUser.Close();

        //    #endregion


        //    return _JsonProject;
        //}


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }

    public class JsonProject
    {
        public int project_id;
        public string project_name;
        public IQueryable<JsonProducts> products;
    }

    public class JsonProducts
    {
        public int product_id;
        public string product_name;
        public IQueryable<JsonTargets> targets;
    }

    public class JsonTargets
    {
        public int target_id;
        public string target_name;
        public string value_type;
        public IQueryable<JsonPages> pages;

    }

    public class JsonPages
    {
        public int page_id;
        public string page_name;
        public IList<string> pictures;
        public IQueryable<JsonPoints> measure_points;
    }

    public class JsonPoints
    {
        public int pointId;
        public int index;
        public string point;
        public string direction;
        public string upperTolerance;
        public string lowerTolerance;
    }


    public class JsonUsers
    {
        public int user_id;
        public string username;
        public string password;
    }
}