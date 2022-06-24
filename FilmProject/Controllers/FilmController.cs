using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using FilmProject.Models;
using FilmProject.Models.ViewModels;
using System.Web.Script.Serialization;

namespace FilmProject.Controllers
{
    public class FilmController : Controller
    {
        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();


        static FilmController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44302/api/");
        }
        // GET: Film/List
        public ActionResult List()
        {
            //objective: communicate with our film data api to retrieve a list of film
            //curl https://localhost:44302/api/FilmData/ListFilms

            string url = "FilmData/ListFilms";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("the response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<FilmDto> films = response.Content.ReadAsAsync<IEnumerable<FilmDto>>().Result;
            //Debug.WriteLine("Number of Films received: ");
            //Debug.WriteLine(films.Count());


            return View(films);
        }

        // GET: Film/Details/5
        public ActionResult Details(int id)
        {
            DetailsFilm ViewModel = new DetailsFilm();
            //objective: communicate with our film data api to retrieve a list of film
            //curl https://localhost:44302/api/FilmData/FindFilm/{id}

            string url = "FilmData/FindFilm/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("the response code is ");
            Debug.WriteLine(response.StatusCode);

            FilmDto SelectedFilm = response.Content.ReadAsAsync<FilmDto>().Result;
            ViewModel.SelectedFilm = SelectedFilm;
            Debug.WriteLine("Film received: ");
            Debug.WriteLine(SelectedFilm.FilmName);

            //show associated actors with this film
            url = "actordata/listactorsforfilm/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<ActorDto> AssociatedActors = response.Content.ReadAsAsync<IEnumerable<ActorDto>>().Result;

            ViewModel.AssociatedActors = AssociatedActors;
            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Film/New
        public ActionResult New()
        {
            //information about all Studios in the system
            //Get api/studiodata/liststudio

            string url = "StudioData/ListStudios";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<StudioDto> StudioOptions = response.Content.ReadAsAsync<IEnumerable<StudioDto>>().Result;
            return View(StudioOptions);
        }

        // POST: Film/Create
        [HttpPost]
        public ActionResult Create(Film film)
        {
            Debug.WriteLine("the inputed Film Name is:");
            Debug.WriteLine(film.FilmName);
            //objective:add a new film into the system using api
            //curl -H "Content-Type:application/json" -d @film.json https://localhost:44302/api/FilmData/addfilm
            string url = "FilmData/addfilm";

            string jsonpayload = jss.Serialize(film);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Errors");
            }

        }

        // GET: Film/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateFilm ViewModel = new UpdateFilm();

            string url = "filmdata/findfilm/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            FilmDto SelectedFilm = response.Content.ReadAsAsync<FilmDto>().Result;
            ViewModel.SelectedFilm = SelectedFilm;
           
            url = "StudioData/ListStudios"; ;
            response = client.GetAsync(url).Result;
            IEnumerable<StudioDto> StudioOptions = response.Content.ReadAsAsync<IEnumerable<StudioDto>>().Result;
            ViewModel.StudioOptions = StudioOptions;
            return View(ViewModel);
        }

        // POST: Film/Update/5
        [HttpPost]
        public ActionResult Update(int id, Film film)
        {
            string url = "FIlmData/UpdateFilm/" + id;
            string jsonpayload = jss.Serialize(film);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Film/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "filmdata/findfilm/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            FilmDto selectedfilm = response.Content.ReadAsAsync<FilmDto>().Result;
            return View(selectedfilm);
        }

        // POST: Film/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "filmdata/deletefilm/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
