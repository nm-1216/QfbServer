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


            var day1 = DateTime.Parse(measureData.checktime.ToString("yyyy-MM-dd 00:00:01"));
            var day2 = DateTime.Parse(measureData.checktime.AddDays(1).ToString("yyyy-MM-dd 00:00:01"));

            var t1 = checktime(day1);
            var t2 = checktime(day2);


            var model = db.MeasureDatas.Where(
                b => b.ProjectName == measureData.ProjectName
                && b.ProductName == measureData.ProductName
                && b.TargetName == measureData.TargetName
                && b.MeasurePoint == measureData.MeasurePoint
                && b.Direction == measureData.Direction
                && b.Username == measureData.Username
                && b.Timestamp > t1
                && b.Timestamp < t2
                );

            if (null != model)
            {
                db.MeasureDatas.RemoveRange(model);
            }


            db.MeasureDatas.Add(measureData);
            db.SaveChanges();

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