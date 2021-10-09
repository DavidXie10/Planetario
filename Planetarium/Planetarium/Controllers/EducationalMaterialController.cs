using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Handlers;
using Planetarium.Models;


namespace Planetarium.Controllers
{
    public class EducationalMaterialController : Controller
    {
        public ActionResult ListEducationalMaterial()
        {
            EducationalMaterialHandler dataAccess = new EducationalMaterialHandler();
            ViewBag.EducationalMaterials = dataAccess.GetAllEducationalMaterial();
            return View();
        }
    }
}