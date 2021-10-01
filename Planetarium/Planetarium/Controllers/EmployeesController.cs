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
            return View();
        }

        [HttpPost]
        public ActionResult CreateEmployee(EmployeeModel employee) {
            ActionResult view = RedirectToAction("Success", "Home"); ;
            ViewBag.SucessOnCreation = false;
            try {
                if (ModelState.IsValid) {
                    EmployeesHandler dataAcess = new EmployeesHandler();
                    ViewBag.SucessOnCreation = dataAcess.CreateEmployee(employee);
                if (ViewBag.SucessOnCreation) {
                        //ViewBag.Message = "El funcionario" + " " + employee.FirstName + " " + employee.LastName  + " fue creado con éxito :)" ;
                        ModelState.Clear();
                    }
                }
                return view;
            } catch (Exception exception){
                ViewBag.Message = "Algo salió mal y no fue posible crear el funcionario";
                return View(); 
            }
        }
    }
}