﻿using System;
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
            ContentParser contentParser = new ContentParser("Mision.txt");
            ViewBag.Message = contentParser.getContentFromFile();
            return View();
        }

        public ActionResult Vision()
        {
            ContentParser contentParser = new ContentParser("Vision.txt");
            ViewBag.Message = contentParser.getContentFromFile();

            return View();
        }

        public ActionResult ActivitiesDescription()
        {
            ViewBag.Message = "Our activities.";

            return View();
        }

        public  ActionResult Location()
        {
            ContentParser contentParser = new ContentParser("Parking.txt");
            ViewBag.Parking = contentParser.getContentFromFile();

            contentParser.fileName = "Transport.txt";
            ViewBag.Transport = contentParser.getContentFromFile();

            contentParser.fileName = "Schedule.txt";
            ViewBag.Schedule = contentParser.getContentFromFile();
           
            return View();
        }
    }
}