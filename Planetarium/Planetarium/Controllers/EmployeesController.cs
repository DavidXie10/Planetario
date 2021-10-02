using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Handlers;
using Planetarium.Models;

namespace Planetarium.Controllers
{
    public class EmployeesController : Controller
    {
        // GET: Employees
        public ActionResult ListEmployees()
        {
            EmployeesHandler dataAccess = new EmployeesHandler();
            ViewBag.employees = dataAccess.GetAllEmployees();
            return View();
        }

        public ActionResult CreateEmployee() 
        {
            List<SelectListItem> countries = new List<SelectListItem>();
            ContentParser contentParser = new ContentParser();
            dynamic JsonContent = contentParser.ParseFromJSON("countries.json");
            string[] countriesFromJson = JsonContent.CountrieNames.ToObject<string[]>();

            foreach (string country in countriesFromJson) {
                countries.Add(new SelectListItem { Text = country, Value = country });
            }

            ViewBag.Countries = countries;

            return View();
        }

        [HttpPost]
        public ActionResult CreateEmployee(EmployeeModel employee) {
            ActionResult view = RedirectToAction("Success", "Home"); 
            employee.Gender = Request.Form["gender"].ElementAt(0);
            employee.Lenguages = Request.Form["language"];
            employee.NativeCountry = Request.Form["contry"];
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
                ViewBag.Message = e.Message;
                //ViewBag.Message = "Algo salió mal y no fue posible crear el funcionario";
                return View(); 
            }
        }
    }
}