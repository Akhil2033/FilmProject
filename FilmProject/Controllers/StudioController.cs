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
    public class StudioController : Controller
    {
        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();

        static StudioController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44302/api/");
        }

        // GET: Studio/List
        public ActionResult List()
        {
            //Objective: Communicate with our studio data api to retrieve a list of studios
            //curl https://localhost:44302/api/StudioData/ListStudios

            string url = "StudioData/ListStudios";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<StudioDto> studios = response.Content.ReadAsAsync<IEnumerable<StudioDto>>().Result;
            Debug.WriteLine("Number of Studios received: ");
            Debug.WriteLine(studios.Count());

            return View(studios);
        }

        // GET: Studio/Details/5
        public ActionResult Details(int id)
        {
            //Objective: Communicate with our studio data api to retrieve a details about one studio
            //curl https://localhost:44302/api/StudioData/FindStudio/{id}

            DetailsStudios ViewModel = new DetailsStudios();

            string url = "StudioData/findstudio/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is");
            Debug.WriteLine(response.StatusCode);

            StudioDto SelectedStudio = response.Content.ReadAsAsync<StudioDto>().Result;
            Debug.WriteLine("Studio received: ");
            Debug.WriteLine(SelectedStudio.StudioName);

            ViewModel.SelectedStudio = SelectedStudio;

            url = "FilmData/listfilmsforstudios/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<FilmDto> RelatedFilms = response.Content.ReadAsAsync<IEnumerable<FilmDto>>().Result;

            ViewModel.RelatedFilms = RelatedFilms;


            return View(ViewModel);
        }

        // GET: Studio/Create
        public ActionResult New()
        {
            return View();
        }

        // POST: Studio/Create
        [HttpPost]
        public ActionResult Create(Studio studio)
        {
            Debug.WriteLine("the inputed Film Name is:");
            Debug.WriteLine(studio.StudioName);
            //objective:add a new film into the system using api
            //curl -H "Content-Type:application/json" -d @Studio.json https://localhost:44302/api/StudioData/addstudio
            string url = "StudioData/addstudio";

            string jsonpayload = jss.Serialize(studio);

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

        // GET: Studio/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateStudio ViewModel = new UpdateStudio();

            string url = "studiodata/findstudio/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            StudioDto SelectedStudio = response.Content.ReadAsAsync<StudioDto>().Result;
            ViewModel.SelectedStudio = SelectedStudio;
            return View(ViewModel);

        }

        // POST: Studio/Edit/5
        [HttpPost]
        public ActionResult Update(int id, Studio studio)
        {
            string url = "studiodata/findstudio/" + id;
            string jsonpayload = jss.Serialize(studio);
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

        // GET: Studio/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "studiodata/findstudio/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            StudioDto selectedstudio = response.Content.ReadAsAsync<StudioDto>().Result;
            return View(selectedstudio);
        }

        // POST: Studio/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "studiodata/deletestudio/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.GetAsync(url).Result;

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
