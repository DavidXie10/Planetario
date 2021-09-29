using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Handlers;
using Planetarium.Models;

namespace Planetarium.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            NewsHandler dataAccess = new NewsHandler();
            ViewBag.News = dataAccess.GetAllNews();
            ViewBag.length = 3;
            return View();
        }
        
        public ActionResult FindUs() {
            return View();
        }

        public ActionResult WhoWeAre()
        {
            ContentParser contentParser = new ContentParser();
            ViewBag.MissionMessage = contentParser.getContentFromFile("Mision.txt");
            ViewBag.VisionMessage = contentParser.getContentFromFile("Vision.txt");

            return View();
        }

        public ActionResult ActivitiesDescription()
        {
            ViewBag.Message = "Our activities.";

            return View();
        }

        public ActionResult FAQ() {
            return View();
        }

        public ActionResult Educative() {
            return View();
        }
    }
}