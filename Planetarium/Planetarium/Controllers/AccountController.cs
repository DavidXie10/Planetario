using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Models;
using Planetarium.Handlers;

namespace Planetarium.Controllers
{
    public class AccountController : Controller
    {
        public AuthHandler AuthDataAccess { get; set; }
        public AuthorizationController AuthController { get; set; }

        public AccountController() {
            AuthDataAccess = new AuthHandler();
            AuthController = new AuthorizationController();
        }

        [HttpPost]
        public ActionResult CheckCredentials(LoginModel loginCredentials) {
            ActionResult view = RedirectToAction("Login", "Account");
            bool isRegistered = AuthDataAccess.Authenticate(loginCredentials);
            TempData["Error"] = true;
            TempData["WarningMessage"] = "";

            //Proceso de crear la cookie
            if (isRegistered) {
                TempData["Error"] = false;

                UserModel user = AuthDataAccess.GetUserByUsername(loginCredentials.Username);
                HttpCookie authCookie = AuthController.UpdateCookie("authCookie", user.Rol.ToString());
                HttpCookie userIdentityCookie = AuthController.UpdateCookie("userIdentity", user.Username);
                
                if(!(authCookie == null)) {
                    Response.Cookies.Add(authCookie);
                }

                if (!(userIdentityCookie == null)) {
                    Response.Cookies.Add(userIdentityCookie);
                }


                view = RedirectToAction("Index", "Home");
                
            } else {
                TempData["WarningMessage"] = "Algo salió mal";
            }


            return view;
        }

        public ActionResult Login(){
            return View();
        }

        public ActionResult Register() {
            return View();
        }


        public ActionResult LoginUser(string username) {
            ActionResult view = RedirectToAction("Index", "Home");
            return view;
            //bool loginSuccessful = false;
            //return Json(loginSuccessful, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RegisterUser(UserModel user) {
            ActionResult view = RedirectToAction("Index", "Home");
            bool registerSuccessful = false;
            return Json(registerSuccessful, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllUserNames() {
            List<string> userNames = AuthDataAccess.GetAllVisitorsUserNames();
            return Json(userNames, JsonRequestBehavior.AllowGet);
        }

        

    }
}