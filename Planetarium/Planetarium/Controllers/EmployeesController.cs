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

namespace Planetarium.Controllers
{
    public class EmployeesController : Controller
    {
        // GET: Employees
        ContentParser contentParser = new ContentParser();
        public ActionResult Employee(String Dni) {
            ActionResult view;
            try {
                EmployeesHandler dataAccess = new EmployeesHandler();
                EmployeeModel employee = dataAccess.GetAllEmployees().Find(smodel => String.Equals(smodel.Dni, Dni));
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

        public ActionResult ListEmployees()
        {
            EmployeesHandler dataAccess = new EmployeesHandler();
            ViewBag.employees = dataAccess.GetAllEmployees();
            return View();
        }

        
        public ActionResult CreateEmployee() {

            ContentParser contentParser = new ContentParser();

            List<SelectListItem> countries = new List<SelectListItem>();
            List<SelectListItem> languages = new List<SelectListItem>();

            dynamic JsonContentCountries = contentParser.ParseFromJSON("countries.json");
            dynamic JsonContentLanguages = contentParser.ParseFromJSON("Languages.json");

            string[] countriesFromJson = JsonContentCountries.CountrieNames.ToObject<string[]>();

            foreach (string country in countriesFromJson) {
                countries.Add(new SelectListItem { Text = country, Value = country });
            }

            foreach(var langage in JsonContentLanguages) {
                string name = langage.Value["name"].ToString();
                languages.Add(new SelectListItem { Value = name, Text = name });
            }

            ViewBag.Countries = countries;
            ViewBag.Languages = languages;

            return View();
        }

        [HttpPost]
        public ActionResult PostCreateEmployee(EmployeeModel employee) {
            
            ActionResult view = RedirectToAction("Success", "Home");
            employee.Gender = Request.Form["gender"].ElementAt(0);
            employee.Languages = contentParser.GetListFromString(Request.Form["defaultInputString"]);
            ViewBag.SucessOnCreation = false;
            try {
                if (ModelState.IsValid) {
                    EmployeesHandler dataAcess = new EmployeesHandler();
                    ViewBag.SucessOnCreation = dataAcess.CreateEmployee(employee);
                    if (ViewBag.SucessOnCreation) {
                        ModelState.Clear();
                    }
                }
                return view;
            } catch (Exception e)
            {
                Debug.WriteLine("Error en la insercion: " + e.ToString());
                view = RedirectToAction("CreateEmployee", "Employees");
                ViewBag.Message = "Algo salió mal y no fue posible crear el funcionario";
                return view; 
            }
        }
    }
}