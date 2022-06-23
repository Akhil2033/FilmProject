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
using FilmProject.Models;

namespace Film_Passion_Project.Controllers
{
    public class StudioDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/StudioData/ListStudios
        [HttpGet]
        public IEnumerable<StudioDto> ListStudios()
        {
            List<Studio> Studios = db.Studios.ToList();
            List<StudioDto> StudioDtos = new List<StudioDto>();

            Studios.ForEach(s => StudioDtos.Add(new StudioDto()
            {
                StudioId = s.StudioId,
                StudioName = s.StudioName,
                StudioDesc = s.StudioDesc
            }));

            return StudioDtos;
        }

        // GET: api/StudioData/FindStudio/5
        [ResponseType(typeof(Studio))]
        [HttpGet]
        public IHttpActionResult FindStudio(int id)
        {
            Studio Studio = db.Studios.Find(id);
            StudioDto StudioDto = new StudioDto()
            {
                StudioId = Studio.StudioId,
                StudioName = Studio.StudioName,
                StudioDesc = Studio.StudioDesc

            };
            if (Studio == null)
            {
                return NotFound();
            }

            return Ok(StudioDto);
        }

        // POST: api/StudioData/UpdateStudio/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateStudio(int id, Studio studio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != studio.StudioId)
            {
                return BadRequest();
            }

            db.Entry(studio).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudioExists(id))
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

        // POST: api/StudioData/AddStudio
        [ResponseType(typeof(Studio))]
        [HttpPost]
        public IHttpActionResult AddStudio(Studio studio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Studios.Add(studio);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = studio.StudioId }, studio);
        }

        // POST: api/StudioData/DeleteStudio/5
        [ResponseType(typeof(Studio))]
        [HttpPost]
        public IHttpActionResult DeleteStudio(int id)
        {
            Studio studio = db.Studios.Find(id);
            if (studio == null)
            {
                return NotFound();
            }

            db.Studios.Remove(studio);
            db.SaveChanges();

            return Ok(studio);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudioExists(int id)
        {
            return db.Studios.Count(e => e.StudioId == id) > 0;
        }
    }
}