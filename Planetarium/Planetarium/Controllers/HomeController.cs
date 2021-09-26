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
        
        public ActionResult FindUs() {
            ContentParser contentParser = new ContentParser();
            dynamic jsonContent = contentParser.ParseFromJSON("Services.json");
            string[] schedule = jsonContent.Horarios;
            string[] transportBuses = jsonContent.Buses;
            string[] transportTrains = jsonContent.Trenes;
            string[] parking = jsonContent.Parqueos;

            ViewBag.Parking = parking;
            ViewBag.TransportBuses = transportBuses;
            ViewBag.TransportTrains = transportTrains;
            ViewBag.Schedule = schedule;
            return View();
        }

        public ActionResult WhoWeAre()
        {
            ContentParser contentParser = new ContentParser();

            dynamic jsonContent = contentParser.ParseFromJSON("Planetario.json");
            string mision = jsonContent.Mision;
            string vision = jsonContent.Vision;

            ViewBag.MissionMessage = mision;
            ViewBag.VisionMessage = vision;
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
            dynamic jsonContent = contentParser.ParseFromJSON("Services.json");
            string[] schedule = jsonContent.Horarios;
            string[] transportBuses = jsonContent.Buses;
            string[] transportTrains = jsonContent.Trenes;
            string[] parking = jsonContent.Parqueos;

            ViewBag.Parking = parking;
            ViewBag.TransportBuses = transportBuses;
            ViewBag.TransportTrains = transportTrains;
            ViewBag.Schedule = schedule;
           
            return View();
        }
    }
}