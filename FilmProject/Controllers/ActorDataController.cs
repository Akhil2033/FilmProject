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
using System.Diagnostics;

namespace FilmProject.Controllers
{
    public class ActorDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        /// <summary>
        /// Returns all actors in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all actors in the database
        /// </returns>
        /// <example>
        /// GET: api/FilmData/ListActors
        /// </example>

        // GET: api/ActorData/ListActors
        [HttpGet]
        public IEnumerable<ActorDto> ListActors()
        {
            List<Actor> Actors = db.Actors.ToList();
            List<ActorDto> ActorDtos = new List<ActorDto>();

            Actors.ForEach(a => ActorDtos.Add(new ActorDto()
            {
                ActorId = a.ActorId,
                ActorName = a.ActorName,
                ActorFee = a.ActorFee,
            }));

            return ActorDtos;
        }

        /// <summary>
        /// Content: all Actors in the database,associated with films
        /// </summary>
        /// <returns></returns>
        /// <example>
        /// GET: api/ActorData/ListActorsforFilm/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ActorDto))]
        public IHttpActionResult ListActorsForFilm(int id)
        {
            List<Actor> Actors = db.Actors.Where(
                a=>a.Films.Any(
                    f=>f.FilmId == id)
                ).ToList();
            List<ActorDto> ActorDtos = new List<ActorDto>();

            Actors.ForEach(a => ActorDtos.Add(new ActorDto()
            {
                ActorId = a.ActorId,
                ActorName = a.ActorName,
                ActorFee = a.ActorFee,
            }));

            return Ok(ActorDtos);
        }
        
        /// <summary>
        /// Returns all actors in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An actor in the system matching up to the actor ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the actor</param>
        /// <example>
        /// GET: api/ActorData/FindActor/5
        /// </example>
        [ResponseType(typeof(Actor))]
        [HttpGet]
        public IHttpActionResult FindActor(int id)
        {
            Actor Actor = db.Actors.Find(id);
            ActorDto ActorDto = new ActorDto()
            {
                ActorId = Actor.ActorId,
                ActorName = Actor.ActorName,
                ActorFee = Actor.ActorFee
            };

            if (Actor == null)
            {
                return NotFound();
            }

            return Ok(ActorDto);
        }

        /// <summary>
        /// Updates a particular actor in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Actor ID primary key</param>
        /// <param name="actor">JSON FORM DATA of an actor</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/ActorData/UpdateActor/5
        /// FORM DATA: Actor JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateActor(int id, Actor actor)
        {
            Debug.WriteLine("I have reached the update Actor method");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != actor.ActorId)
            {
                return BadRequest();
            }

            db.Entry(actor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActorExists(id))
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
        /// Adds an actor to the system
        /// </summary>
        /// <param name="actor">JSON FORM DATA of an actor</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Actor_id, Actor_name, actor_salary
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/ActorData/AddActor
        /// FORM DATA: Actor JSON Object
        /// </example>
        [ResponseType(typeof(Actor))]
        [HttpPost]
        public IHttpActionResult AddActor(Actor actor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Actors.Add(actor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = actor.ActorId }, actor);
        }
        
        /// <summary>
        /// Deletes an actor from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the actor</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/ActorData/DeleteActor/5
        /// FORM DATA: (empty)
        /// </example>

        [ResponseType(typeof(Actor))]
        [HttpPost]
        public IHttpActionResult DeleteActor(int id)
        {
            Actor actor = db.Actors.Find(id);
            if (actor == null)
            {
                return NotFound();
            }

            db.Actors.Remove(actor);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ActorExists(int id)
        {
            return db.Actors.Count(e => e.ActorId == id) > 0;
        }
    }
}
