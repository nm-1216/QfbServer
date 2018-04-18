using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using QfbServer.Models;

namespace QfbServer.Controllers
{
    public class MeasureDatasController : ApiController
    {
        private QfbServerContext db = new QfbServerContext();
        Microsoft.Practices.EnterpriseLibrary.Data.Database _db = Microsoft.Practices.EnterpriseLibrary.Data.DatabaseFactory.CreateDatabase("conn");


        // GET: api/MeasureDatas
        public IQueryable<MeasureData> GetMeasureDatas()
        {
            return db.MeasureDatas;
        }

        // GET: api/MeasureDatas/5
        [ResponseType(typeof(MeasureData))]
        public IHttpActionResult GetMeasureData(int id)
        {
            MeasureData measureData = db.MeasureDatas.Find(id);
            if (measureData == null)
            {
                return NotFound();
            }

            return Ok(measureData);
        }

        // PUT: api/MeasureDatas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMeasureData(int id, MeasureData measureData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != measureData.Id)
            {
                return BadRequest();
            }

            db.Entry(measureData).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeasureDataExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        public long checktime(DateTime d)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            long timeStamp = (long)(d - startTime).TotalMilliseconds;
            return timeStamp;
        }

        // POST: api/MeasureDatas
        [ResponseType(typeof(MeasureData))]
        public IHttpActionResult PostMeasureData(MeasureData measureData)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var day1 = measureData.checktime.ToString("yyyy-MM-dd");

            //var t1 = checktime(day1);
            //var t2 = checktime(day2);

            string sql = @"
                delete from [TestData] where [TestData_PointSetting_ID] = {0} and TestData_User='{2}' and [TestData_Date] = '{1}'
            ";

            var ttt = string.Format(sql, measureData.PointId, day1, measureData.Username);

            var ds = _db.ExecuteNonQuery(CommandType.Text, ttt);


            string insert = @"
            INSERT INTO [dbo].[TestData]
            ([TestData_PointSetting_ID]
            ,[TestData_Value1]
            ,[TestData_Value2]
            ,[TestData_Value3]
            ,[TestData_Value4]
            ,[TestData_Value5]
            ,[TestData_Value6]
            ,[TestData_Value7]
            ,[TestData_Value8]
            ,[TestData_Value9]
            ,[TestData_Value10]
            ,[TestData_User]
            ,[TestData_SubmitTime]
            ,[TestData_Date]
            ,[TestData_UpperTolerance]
            ,[TestData_LowerTolerance]
            )
            VALUES
            ({0}
            ,'{1}'
            ,'{2}'
            ,'{3}'
            ,'{4}'
            ,'{5}'
            ,'{6}'
            ,'{7}'
            ,'{8}'
            ,'{9}'
            ,'{10}'
            ,'{11}'
            ,getdate()
            ,'{12}'
            ,{13}
            ,{14}
            );
            ";

            var temp = string.Format(insert, measureData.PointId,
                measureData.Value1,
                measureData.Value2,
                measureData.Value3,
                measureData.Value4,
                measureData.Value5,

                measureData.Value6,
                measureData.Value7,
                measureData.Value8,
                measureData.Value9,
                measureData.Value10,

                measureData.Username,
                measureData.checktime.ToString("yyyy-MM-dd"),
                string.IsNullOrWhiteSpace(measureData.UpperTolerance) ? "null" : measureData.UpperTolerance,
                string.IsNullOrWhiteSpace(measureData.LowerTolerance) ? "null" : measureData.LowerTolerance

                );

            _db.ExecuteNonQuery(CommandType.Text, temp);



            //var model = db.MeasureDatas.Where(
            //    b => b.ProjectName == measureData.ProjectName
            //    && b.ProductName == measureData.ProductName
            //    && b.TargetName == measureData.TargetName
            //    && b.MeasurePoint == measureData.MeasurePoint
            //    && b.Direction == measureData.Direction
            //    && b.Username == measureData.Username
            //    && b.Timestamp > t1
            //    && b.Timestamp < t2
            //    );

            //if (null != model)
            //{
            //    db.MeasureDatas.RemoveRange(model);
            //}

            //db.MeasureDatas.Add(measureData);
            //db.SaveChanges();

            ServiceReference1.WCFServiceClient client = new ServiceReference1.WCFServiceClient();
            client.StartStatic(ServiceReference1.SourceType.Standard);


            return CreatedAtRoute("DefaultApi", new { id = measureData.Id }, measureData);
        }

        // DELETE: api/MeasureDatas/5
        [ResponseType(typeof(MeasureData))]
        public IHttpActionResult DeleteMeasureData(int id)
        {
            MeasureData measureData = db.MeasureDatas.Find(id);
            if (measureData == null)
            {
                return NotFound();
            }

            db.MeasureDatas.Remove(measureData);
            db.SaveChanges();

            return Ok(measureData);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MeasureDataExists(int id)
        {
            return db.MeasureDatas.Count(e => e.Id == id) > 0;
        }
    }
}