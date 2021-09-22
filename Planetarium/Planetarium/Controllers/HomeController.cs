using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Models;

namespace Planetarium.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Mission()
        {
            ContentParser contentParser = new ContentParser();
            ViewBag.Message = contentParser.GetContentFromFile("Mision.txt");
            return View();
        }

        public ActionResult Vision()
        {
            ContentParser contentParser = new ContentParser();
            ViewBag.Message = contentParser.GetContentFromFile("Vision.txt");

            return View();
        }

        public ActionResult ActivitiesDescription()
        {
            ViewBag.Message = "Our activities.";

            return View();
        }

        public  ActionResult Location()
        {
            ContentParser contentParser = new ContentParser();
            ViewBag.Parking = contentParser.GetContentFromFile("Parking.txt");

            ViewBag.Transport = contentParser.GetContentFromFile("Transport.txt");

            ViewBag.Schedule = contentParser.GetContentFromFile("Schedule.txt");
           
            return View();
        }
    }
}