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
        
        public ActionResult FindUs() {
            ContentParser contentParser = new ContentParser();
            ViewBag.Parking = contentParser.GetContentFromFile("Parking.txt");

            ViewBag.Transport = contentParser.GetContentFromFile("Transport.txt");

            ViewBag.Schedule = contentParser.GetContentFromFile("Schedule.txt");
            return View();
        }

        public ActionResult WhoWeAre()
        {
            ContentParser contentParser = new ContentParser();
            ViewBag.MissionMessage = contentParser.GetContentFromFile("Mision.txt");
            ViewBag.VisionMessage = contentParser.GetContentFromFile("Vision.txt");

            return View();
        }

        public ActionResult ActivitiesDescription()
        {
            ViewBag.Message = "Our activities.";

            return View();
        }



        public ActionResult Educative() {
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