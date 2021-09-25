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
            return View();
        }

        public ActionResult WhoWeAre()
        {
            ContentParser contentParser = new ContentParser();
            ViewBag.MissionMessage = contentParser.getContentFromFile("Mision.txt");
            ViewBag.VisionMessage = contentParser.getContentFromFile("Vision.txt");
            ViewBag.IdPhoto = "mauImg.png";
            ViewBag.Name = "Mauricio";
            ViewBag.LastName = "Rojas";
            ViewBag.AcademicDegree = "Bach.Computación";
            ViewBag.Occupation = "Ingeniero Software";
            ViewBag.Mail = "mauricio.rojassegnini@ucr.ac.cr";
            ViewBag.Phrase = "Todo lo puedo en Carlos que me fortalece ~(Mauricio,2021)";
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