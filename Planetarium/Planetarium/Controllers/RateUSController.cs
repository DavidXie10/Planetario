using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planetarium.Controllers
{
    public class RateUSController : Controller
    {
        // GET: RateUS
        public ActionResult SuccessProcessRate()
        {
            return View();
        }

        public ActionResult IndexRate()
        {
            return View();
        }

        public ActionResult UXEvaluation() {
            ViewBag.Link= "https://docs.google.com/forms/d/e/1FAIpQLScpghh7KECEEjpnpJHqG1l9Zr2a4gcnDCpcKVpN1C2xt1ZMHw/viewform?embedded=true";
            return View();
        }

        public ActionResult RateResults()
        {
            return View();
        }
    }
}