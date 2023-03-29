using HospitalManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HospitalManagementSystem.Controllers
{
    public class ServiceController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ServiceController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/servicedata/");

        }

        // GET: Service/List
        public ActionResult List()
        {
            //objective: communicate with our Service data api to retrive a list of Services
            //curl: https://localhost:44316/api/servicedata/listservices

            string url = "listservices";
            HttpResponseMessage response = client.GetAsync(url).Result;


            Debug.WriteLine("the response code is: ");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<Service> services = response.Content.ReadAsAsync<IEnumerable<Service>>().Result;

            Debug.WriteLine("Number of Services recieved:");
            Debug.WriteLine(services.Count());

            return View(services);
        }

        // GET: Service/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our service data api to retrive one service
            //curl: https://localhost:44316/api/servicedata/findservice/{id}

            string url = "findservice/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;


            //Debug.WriteLine("the response code is: ");
            // Debug.WriteLine(response.StatusCode);

            Service SelectedService = response.Content.ReadAsAsync<Service>().Result;

            //Debug.WriteLine(" Service recieved:");
            // Debug.WriteLine(SelectedService.ServiceName);


            return View(SelectedService);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Service/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Service/Create
        [HttpPost]
        public ActionResult Create(Service service)
        {
            Debug.WriteLine("the json payload is:");
            // Debug.WriteLine(Service.ServiceName);
            //objective: add a new Service into our system using the API

            //curl -H "Content-Type:appliation/json" -d service.json https://localhost:44316/api/servicedata/addservice
            string url = "addservice";


            string jsonpayload = jss.Serialize(service);

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

        // GET: Service/Edit/5
        public ActionResult Edit(int id)
        {

            //the existing service information
            string url = "findservice/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Service SelectedService = response.Content.ReadAsAsync<Service>().Result;

            return View(SelectedService);
        }

        // POST: Service/Edit/5
        [HttpPost]
        public ActionResult Update(int id, Service service)
        {

            string url = "updateservice/"+id;
            string jsonpayload = jss.Serialize(service);
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

        // GET: Service/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "findservice/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Service SelectedService = response.Content.ReadAsAsync<Service>().Result;
            return View(SelectedService);
        }

        // POST: Service/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Service service)
        {
            string url = "deleteservice/"+id;
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
