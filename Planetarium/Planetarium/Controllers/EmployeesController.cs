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
            return View(new EmployeeModel());
        }

        [HttpPost]
        public ActionResult CreateEmployee(EmployeeModel employee) {
            ViewBag.SucessOnCreation = false;
            try {
                if (ModelState.IsValid) {
                    EmployeesHandler dataAcess = new EmployeesHandler();
                    ViewBag.SucessOnCreation = dataAcess.CreateEmployee(employee);
                if (ViewBag.SucessOnCreation) {
                        ViewBag.Message = "El funcionario" + " " + employee.FirstName + " " + employee.LastName  + " fue creado con éxito :)" ;
                        ModelState.Clear();
                    }
                }
                // TODO: Cambiar a que redireccione a otra vista
                return View();
            } catch (Exception e){
                ViewBag.Message = e.Message;
               // "Algo salió mal y no fue posible crear el funcionario :(";
                // TODO: Cambiar a que redireccione a otra vista
                return View(); 
            }
        }
    }
}