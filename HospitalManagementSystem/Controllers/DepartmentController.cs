using HospitalManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Net.Http;
using System.Web.Script.Serialization;

namespace HospitalManagementSystem.Controllers
{
    public class DepartmentController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DepartmentController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/departmentdata/");

        }

        // GET: Department/List
        public ActionResult List()
        {
            //objective: communicate with our Department data api to retrive a list of Departments
            //curl: https://localhost:44316/api/departmentdata/listdepartments

            string url = "listdepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;


            Debug.WriteLine("the response code is: ");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<Department> departments = response.Content.ReadAsAsync<IEnumerable<Department>>().Result;

            Debug.WriteLine("Number of Departments recieved:");
            Debug.WriteLine(departments.Count());

            return View(departments);
        }

        // GET: Department/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our department data api to retrive one department
            //curl: https://localhost:44316/api/departmentdata/finddepartment/{id}

            string url = "finddepartment/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;


            //Debug.WriteLine("the response code is: ");
            // Debug.WriteLine(response.StatusCode);

            Department SelectedDepartment = response.Content.ReadAsAsync<Department>().Result;

            //Debug.WriteLine(" Department recieved:");
            // Debug.WriteLine(SelectedDepartment.DepartmentName);


            return View(SelectedDepartment);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Department/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Department/Create
        [HttpPost]
        public ActionResult Create(Department department)
        {
            Debug.WriteLine("the json payload is:");
            // Debug.WriteLine(Department.DepartmentName);
            //objective: add a new Department into our system using the API

            //curl -H "Content-Type:appliation/json" -d department.json https://localhost:44316/api/departmentdata/adddepartment
            string url = "adddepartment";


            string jsonpayload = jss.Serialize(department);

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

        // GET: Department/Edit/5
        public ActionResult Edit(int id)
        {

            //the existing department information
            string url = "finddepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Department SelectedDepartment = response.Content.ReadAsAsync<Department>().Result;

            return View(SelectedDepartment);
        }

        // POST: Department/Edit/5
        [HttpPost]
        public ActionResult Update(int id, Department department)
        {

            string url = "updatedepartment/"+id;
            string jsonpayload = jss.Serialize(department);
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

        // GET: Department/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "finddepartment/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Department SelectedDepartment = response.Content.ReadAsAsync<Department>().Result;
            return View(SelectedDepartment);
        }

        // POST: Department/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Department department)
        {
            string url = "deletedepartment/"+id;
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
