using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Handlers;
using Planetarium.Models;

namespace Planetarium.Controllers {
    public class SouvenirController : Controller  {
        SouvenirHandler souvenirHandler { get; set; }

        public SouvenirController() {
            souvenirHandler = new SouvenirHandler();
        }

        public ActionResult Catalog() {
            List<SouvenirModel> souvenirs = souvenirHandler.GetAllItems();
            ViewBag.Catalog = souvenirs;
            return View();
        }

        public ActionResult HomeDelivery() {

            return View();
        }
    }
}