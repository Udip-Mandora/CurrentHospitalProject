using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Web.Mvc;
using System.Net.Http;
using HospitalManagementSystem.Models;
using System.Web.Script.Serialization;

namespace HospitalManagementSystem.Controllers
{
    public class LocationController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static LocationController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/locationdata/");

        }

        // GET: Location/List
        public ActionResult List()
        {
            //objective: communicate with our location data api to retrive a list of locations
            //curl: https://localhost:44316/api/locationdata/listlocations

            string url = "listlocations";
            HttpResponseMessage response = client.GetAsync(url).Result;


            Debug.WriteLine("the response code is: ");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<Location> locations = response.Content.ReadAsAsync<IEnumerable<Location>>().Result;

            Debug.WriteLine("Number of Locations recieved:");
            Debug.WriteLine(locations.Count());

            return View(locations);
        }

        // GET: Location/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our location data api to retrive one location
            //curl: https://localhost:44316/api/locationdata/findloation/{id}

            string url = "findlocation/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;


            //Debug.WriteLine("the response code is: ");
            // Debug.WriteLine(response.StatusCode);

            Location SelectedLocation = response.Content.ReadAsAsync<Location>().Result;

            //Debug.WriteLine(" Location recieved:");
            // Debug.WriteLine(SelectedLocation.LocationName);


            return View(SelectedLocation);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Location/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Location/Create
        [HttpPost]
        public ActionResult Create(Location location)
        {
            Debug.WriteLine("the json payload is:");
            // Debug.WriteLine(location.LocationName);
            //objective: add a new loation into our system using the API

            //curl -H "Content-Type:appliation/json" -d location.json https://localhost:44316/api/locationdata/addlocation
            string url = "addlocation";


            string jsonpayload = jss.Serialize(location);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType= "application/json";

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

        // GET: Location/Edit/5
        public ActionResult Edit(int id)
        {
            //the existing location information
            string url = "findlocation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Location SelectedLocation = response.Content.ReadAsAsync<Location>().Result;

            return View(SelectedLocation);
        }

        // POST: Location/Edit/5
        [HttpPost]
        public ActionResult Update(int id, Location location)
        {
            string url = "updatelocation/"+id;
            string jsonpayload = jss.Serialize(location);
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
                return RedirectToAction("Error");
            }
        }

        // GET: Location/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "findlocation/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Location SelectedLocation = response.Content.ReadAsAsync<Location>().Result;
            return View(SelectedLocation);
        }

        // POST: Location/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "deletelocation/"+id;
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
