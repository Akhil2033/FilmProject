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
    public class ActorDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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
        // GET: api/ActorData/ListActorsforFilm/1
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

        // GET: api/ActorData/FindActor/5
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

        // POST: api/ActorData/UpdateActor/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateActor(int id, Actor actor)
        {
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

        // POST: api/ActorData/AddActor
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

        // POST: api/ActorData/DeleteActor/5
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

            return Ok(actor);
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