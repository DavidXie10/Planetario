using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Models;

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

        [HttpPost]
        public ActionResult SubmitRate() {
            ActionResult view = RedirectToAction("Index", "Home");
            string starChoice = "";
            string filename = "WebsiteRate.txt";
            if (Request.Cookies["rateStar"] != null) {
                starChoice = Request.Cookies["rateStar"].Value;
            }

            TxtContentParser txtParser = new TxtContentParser();
            string fileContent = txtParser.ExtractRawContent(filename);
            fileContent += starChoice + ",";
            txtParser.WriteToFile(fileContent, filename);

            return view;
        }

        public ActionResult UXEvaluation() {
            ViewBag.Link= "https://docs.google.com/forms/d/e/1FAIpQLScpghh7KECEEjpnpJHqG1l9Zr2a4gcnDCpcKVpN1C2xt1ZMHw/viewform?embedded=true";
            return View();
        }
    }
}