using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Planetarium.Handlers;
using Planetarium.Models;
using System.IO;

namespace Planetarium.Controllers {
    public class EmployeesController : Controller {

        public EmployeesHandler DataAccess { get; set; }
        public ContentParser ContentParser { get; set; }

        public EmployeesController() {
            DataAccess = new EmployeesHandler();
            ContentParser = new ContentParser();
        }

        public ActionResult Employee(String Dni) {
            ActionResult view;
            try {
                EmployeeModel employee = DataAccess.GetAllEmployees().Find(smodel => String.Equals(smodel.Dni, Dni));
                if (employee == null) {
                    view = RedirectToAction("ListEmployees");
                } else {
                    ViewBag.Employee = employee;
                    view = View(employee);
                }
            } catch {
                view = RedirectToAction("ListEmployees");
            }
            return view;
        }

        public ActionResult ListEmployees() {
            ViewBag.employees = DataAccess.GetAllEmployees();
            return View();
        }

        
        public ActionResult CreateEmployee() {
            List<SelectListItem> countries = new List<SelectListItem>();
            List<SelectListItem> languages = new List<SelectListItem>();

            dynamic JsonContentCountries = ContentParser.ParseFromJSON("countries.json");
            dynamic JsonContentLanguages = ContentParser.ParseFromJSON("Languages.json");

            string[] countriesFromJson = JsonContentCountries.CountrieNames.ToObject<string[]>();

            foreach (string country in countriesFromJson) {
                countries.Add(new SelectListItem { Text = country, Value = country });
            }

            foreach(var language in JsonContentLanguages) {
                string name = language.Value["name"].ToString();
                languages.Add(new SelectListItem { Value = name, Text = name });
            }

            ViewBag.Countries = countries;
            ViewBag.Languages = languages;

            return View();
        }

        public void UploadPhoto(HttpPostedFileBase file) {
            file.SaveAs(Path.Combine(Server.MapPath("~/images/EmployeesProfilePhotos"), file.FileName.Replace(" ", "-").Replace("_","-")));
        }

        [HttpPost]
        public ActionResult PostCreateEmployee(EmployeeModel employee) {
            
            ActionResult view = RedirectToAction("Success", "Home");
            employee.Gender = Request.Form["gender"].ElementAt(0);
            employee.Languages = ContentParser.GetListFromString(Request.Form["defaultInputString"]);
            ViewBag.SucessOnCreation = false;
            try {
                if (ModelState.IsValid) {
                    UploadPhoto(employee.PhotoFile);
                    ViewBag.SucessOnCreation = DataAccess.CreateEmployee(employee);
                    if (ViewBag.SucessOnCreation) {
                        ModelState.Clear();
                    }
                }
                return view;
            } catch {
                view = RedirectToAction("CreateEmployee", "Employees");
                ViewBag.Message = "Algo salió mal y no fue posible crear el funcionario";
                return view; 
            }
        }
    }
}