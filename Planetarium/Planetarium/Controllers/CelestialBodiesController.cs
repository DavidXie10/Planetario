using Planetarium.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planetarium.Controllers
{
    public class CelestialBodiesController : Controller
    {
        ContentParser parser { get; set; }

        public CelestialBodiesController() {
            this.parser = new ContentParser();
        }

        public ActionResult SolarSystem3DModel()
        {
            //Modelo implementado por Julian Garnier
            return View();
        }
    }
}