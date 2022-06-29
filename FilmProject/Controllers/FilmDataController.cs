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
    public class FilmDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all films in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all actors in the database
        /// </returns>
        /// <example>
        /// GET: api/FilmData/ListFilms
        /// </example>
        [HttpGet]
        [ResponseType(typeof(FilmDto))]
        public IHttpActionResult ListFilms()
        {
            List<Film> Films = db.Films.ToList();
            List<FilmDto> FilmDtos = new List<FilmDto>();

            Films.ForEach(f => FilmDtos.Add(new FilmDto()
            {
                FilmId = f.FilmId,
                FilmName = f.FilmName,
                FilmYear = f.FilmYear,
                DirectorName = f.DirectorName,
                FilmPlot = f.FilmPlot,
                StudioId = f.Studio.StudioId,
                StudioName = f.Studio.StudioName
            }));

            return Ok(FilmDtos);
        }
        
        /// <summary>
        /// Content: all Films in the database,associated with Studios
        /// </summary>
        /// <returns>
        /// list of films associated with a studio
        /// </returns>
        /// <example>
        /// GET: api/FilmData/ListFilmsForStudios/2
        /// </example>

        [HttpGet]
        [ResponseType(typeof(FilmDto))]

        public IHttpActionResult ListFilmsForStudios(int id)
        {
            List<Film> Films = db.Films.Where(f=>f.Studio.StudioId==id).ToList();
            List<FilmDto> FilmDtos = new List<FilmDto>();

            Films.ForEach(f => FilmDtos.Add(new FilmDto()
            {
                FilmId = f.FilmId,
                FilmName = f.FilmName,
                FilmYear = f.FilmYear,
                DirectorName = f.DirectorName,
                FilmPlot = f.FilmPlot,
                StudioId = f.Studio.StudioId,
                StudioName = f.Studio.StudioName
            }));

            return Ok(FilmDtos);
        }

        /// <summary>
        /// Content: all Films in the database,associated with actors
        /// </summary>
        /// <returns>
        /// list of actors attached to a film
        /// </returns>
        /// <example>
        /// GET: api/FilmData/ListFilmsForActor/1
        /// </example>
        
        [HttpGet]
        [ResponseType(typeof(FilmDto))]

        public IHttpActionResult ListFilmsForActors(int id)
        {
            //All films where actors match with film id
            List<Film> Films = db.Films.Where(f => f.Actors.Any(
                a=>a.ActorId==id
                )).ToList();
            
            List<FilmDto> FilmDtos = new List<FilmDto>();

            Films.ForEach(f => FilmDtos.Add(new FilmDto()
            {
                FilmId = f.FilmId,
                FilmName = f.FilmName,
                FilmYear = f.FilmYear,
                DirectorName = f.DirectorName,
                FilmPlot = f.FilmPlot,
                StudioId = f.Studio.StudioId,
                StudioName = f.Studio.StudioName
            }));

            return Ok(FilmDtos);
        }
        
         /// <summary>
        /// Returns all films in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A film in the system matching up to the film id primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the film</param>
        /// <example>
        /// GET: api/FilmData/FindFilm/5
        /// </example>
        [ResponseType(typeof(Film))]
        [HttpGet]
        public IHttpActionResult FindFilm(int id)
        {
            Film Film = db.Films.Find(id);
            FilmDto FilmDto = new FilmDto()
            {
                FilmId = Film.FilmId,
                FilmName = Film.FilmName,
                FilmYear = Film.FilmYear,
                DirectorName = Film.DirectorName,
                FilmPlot = Film.FilmPlot,
                StudioId = Film.Studio.StudioId,
                StudioName= Film.Studio.StudioName
            };
            if (Film == null)
            {
                return NotFound();
            }

            return Ok(FilmDto);
        }

        /// <summary>
        /// Updates a particular film in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Film ID primary key</param>
        /// <param name="film">JSON FORM DATA of a film</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/FilmData/UpdateFilm/5
        /// FORM DATA: Film JSON Object
        /// </example>
        
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateFilm(int id, Film film)
        {
            Debug.WriteLine("I have reached the update film method");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != film.FilmId)
            {
                return BadRequest();
            }

            db.Entry(film).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilmExists(id))
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
        /// Adds a film to the system
        /// </summary>
        /// <param name="film">JSON FORM DATA of a film</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: film_id and film data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        // POST: api/FilmData/AddFilm
        /// FORM DATA: Film JSON Object
        /// </example>
        [ResponseType(typeof(Film))]
        [HttpPost]
        public IHttpActionResult AddFilm(Film film)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Films.Add(film);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = film.FilmId }, film);
        }
        
        /// <summary>
        /// Deletes a film from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the film</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/FilmData/DeleteFilm/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Film))]
        [HttpPost]
        public IHttpActionResult DeleteFilm(int id)
        {
            Film film = db.Films.Find(id);
            if (film == null)
            {
                return NotFound();
            }

            db.Films.Remove(film);
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

        private bool FilmExists(int id)
        {
            return db.Films.Count(e => e.FilmId == id) > 0;
        }
    }
}
