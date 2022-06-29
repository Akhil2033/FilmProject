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

namespace FilmProject.Controllers
{
    public class StudioDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        /// <summary>
        /// Returns all studios in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all studios in the database
        /// </returns>
        /// <example>
        /// GET: api/StudioData/ListStudios
        /// </example>
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
        
        /// <summary>
        /// Returns all studios in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A studio in the system matching up to the studio id primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the studio</param>
        /// <example>
        /// GET: api/StudioData/FindStudio/5
        /// </example>
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
        
        /// <summary>
        /// Updates a particular studio in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Studio ID primary key</param>
        /// <param name="film">JSON FORM DATA of a studio</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/StudioData/UpdateStudio/5
        /// FORM DATA: Actor JSON Object
        /// </example>
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
        
        /// <summary>
        /// Adds a Studio to the system
        /// </summary>
        /// <param name="film">JSON FORM DATA of a studio</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: studio_id and studio data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/StudioData/AddStudio
        /// FORM DATA: Actor JSON Object
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

        /// <summary>
        /// Deletes a studio from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the studio</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        // POST: api/StudioData/DeleteStudio/5
        /// FORM DATA: (empty)
        /// </example>
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
