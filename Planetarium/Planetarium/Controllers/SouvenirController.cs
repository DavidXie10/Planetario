using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Handlers;

namespace Planetarium.Controllers {
    public class SouvenirController : Controller  {
        SouvenirHandler souvenirHandler { get; set; }

        public SouvenirController() {
            souvenirHandler = new SouvenirHandler();
        }

        public ActionResult Catalog() {
            ViewBag.Catalog = souvenirHandler.GetAllItems();
            return View();
        }
    }
}