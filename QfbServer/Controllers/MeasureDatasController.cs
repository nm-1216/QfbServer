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

        // POST: api/MeasureDatas
        [ResponseType(typeof(MeasureData))]
        public IHttpActionResult PostMeasureData(MeasureData measureData)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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