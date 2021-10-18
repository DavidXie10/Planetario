using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Handlers;
using Planetarium.Models;

namespace Planetarium.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            NewsHandler dataAccess = new NewsHandler();
            EducationalActivityHandler educationalActivityHandler = new EducationalActivityHandler();
            ViewBag.Us = WhoWeAre();
            ViewBag.Activities = educationalActivityHandler.GetAllApprovedActivities();
            ViewBag.News = dataAccess.GetAllNews();
            ViewBag.length = 3;
            return View();
        }
        
        public ActionResult FindUs() {
            ContentParser contentParser = new ContentParser();
            dynamic jsonContent = contentParser.ParseFromJSON("Services.json");
            string[] schedule = jsonContent.Horarios.ToObject<string[]>();
            string[] transportBuses = jsonContent.Buses.ToObject<string[]>();
            string[] transportTrains = jsonContent.Trenes.ToObject<string[]>();
            string[] parking = jsonContent.Parqueos.ToObject<string[]>();
            
            ViewBag.Parking = parking;
            ViewBag.TransportBuses = transportBuses;
            ViewBag.TransportTrains = transportTrains;
            ViewBag.Schedule = schedule;
            return View();
        }

        public ActionResult WhoWeAre() {
            ContentParser contentParser = new ContentParser();

            dynamic jsonContent = contentParser.ParseFromJSON("Planetario.json");
            string mision = jsonContent.Mision;
            string vision = jsonContent.Vision;

            ViewBag.MissionMessage = mision;
            ViewBag.VisionMessage = vision;
            return View();
        }

        public ActionResult Success() {
            ViewBag.Message = "Su información ha sido agregada exitosamente";
            ViewBag.Title = "Success";
            return View();
        }
    }
}