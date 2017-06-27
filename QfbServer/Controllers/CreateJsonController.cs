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
using System.Web.Script.Serialization;
using QfbServer.Models;

namespace QfbServer.Controllers
{
    public class CreateJsonController : ApiController
    {
        private QfbServerContext db = new QfbServerContext();

        List<string> PageName = new List<string>() { "TRIM", "SURFACE" };

        // GET: api/MeasureDatas
        public IQueryable<JsonProject> GetCreateJson()
        {
            var _JsonProject = db.MeasurementReport.GroupBy(b => b.ProjectNo).Select(b => new JsonProject()
            {
                project_id = b.Min(a => a.MeasReportID),
                project_name = b.Key,
                products = db.MeasurementReport.Where(c => c.ProjectNo == b.Key).Select(d => new JsonProducts()
                {
                    product_id = d.MeasReportID,
                    product_name = d.PartName+"("+d.PartNo+")",
                    targets = db.MeasurementItem.Where(f => f.MeasReportID == d.MeasReportID).Select(e => new JsonTargets()
                    {
                        target_id = e.MeasItemID,
                        target_name = e.MeasItemName,
                        value_type = PageName.Contains(e.MeasItemName.ToUpper())?"data": "OK,NG",
                        pages = db.MeasurementPage.Where(g => g.MeasItemID == e.MeasItemID).Select(i => new JsonPages()
                        {
                            page_id = i.MeasPageID,
                            page_name = i.MeasPageNo,
                            pictures = new List<string>() { (b.Key + "_" + d.PartNo + "_" + e.MeasItemName + "_" + i.MeasPageNo.ToString() + ".png") },
                            measure_points = db.MeasurementPoint.Where(j => j.MeasPageID == i.MeasPageID).Select(k => new JsonPoints()
                            {
                                direction = k.Direct,
                                point = k.PointNo
                            })

                        })
                    })
                })
            });

            
            JavaScriptSerializer jsonSerialize = new JavaScriptSerializer();
            var TEMP= jsonSerialize.Serialize(_JsonProject);



            var mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/res/project.json");

            if (File.Exists(mappedPath))
            {
                File.Delete(mappedPath);
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


            return _JsonProject;
        }

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
        public string point;
        public string direction;
    }

}