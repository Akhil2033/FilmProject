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

namespace Film_Passion_Project.Controllers
{
    public class ActorController : Controller
    {
        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();

        static ActorController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44302/api/");
        }

        // GET: Actor/List
        public ActionResult List()
        {
            //OBJECTIVE: COMMUNICATE WITH OUR aCTOR DATA API TO RETRIEVE A LIST OF ACTORS
            //curl https://localhost:44302/api/ActorData/ListActors
            string url = "ActorData/ListActors";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<ActorDto> actors = response.Content.ReadAsAsync<IEnumerable<ActorDto>>().Result;

            return View(actors);
        }

        // GET: Actor/Details/5
        public ActionResult Details(int id)
        {
            DetailsActors ViewModel = new DetailsActors();
            //objective: communicate with Actor data api to retreive details of the actor
            //curl https://localhost:44302/api/ActorData/FindActor/{id}

            string url = "ActorData/FindActor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ActorDto selectedActor = response.Content.ReadAsAsync<ActorDto>().Result;

            ViewModel.SelectedActor = selectedActor;


            //show all films starring this actor
            url = "filmdata/listfilmsforactors/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<FilmDto> StarFilms = response.Content.ReadAsAsync<IEnumerable<FilmDto>>().Result;

            ViewModel.StarFilms = StarFilms;

            return View(ViewModel);

        }
        public ActionResult Error()
        {
            return View();
        }

        // GET: Actor/Create
        public ActionResult New()
        {
            return View();
        }

        // POST: Actor/Create
        [HttpPost]
        public ActionResult Create(Actor actor)
        {
            //objective: add a new actor into the system using api
            //curl -H "Content-Type:application/json" -d @Actor.json https://localhost:44302/api/ActorData/addactor
            string url = "ActorData/addActor";

            string jsonpayload = jss.Serialize(actor);

            HttpContent content = new StringContent(jsonpayload);
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

        // GET: Actor/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateActor ViewModel = new UpdateActor();

            string url = "actordata/findactor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ActorDto SelectedActor = response.Content.ReadAsAsync<ActorDto>().Result;
            ViewModel.SelectedActor = SelectedActor;
            return View(ViewModel);
        }

        // POST: Actor/Update/5
        [HttpPost]
        public ActionResult Update(int id, Actor actor)
        {
            string url = "ActorData/UpdateActor/" + id;
            string jsonpayload = jss.Serialize(actor);
            HttpContent content = new StringContent(jsonpayload);
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

        // GET: Actor/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "actordata/findactor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ActorDto selectedactor = response.Content.ReadAsAsync<ActorDto>().Result;
            return View(selectedactor);
        }

        // POST: Actor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "actordata/deleteactor/" + id;
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
