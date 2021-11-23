using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Models;

namespace Planetarium.Controllers
{
    public class AccountController : Controller
    {
        
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register() {
            return View();
        }


        public ActionResult LoginUser(UserModel user) {
            ActionResult view = RedirectToAction("Index", "Home");
            return view;
        }

        [HttpPost]
        public ActionResult RegisterUser(UserModel user) {
            ActionResult view = RedirectToAction("Index", "Home");
            return view;
        }


    }
}