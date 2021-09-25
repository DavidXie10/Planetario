using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Handlers;
using Planetarium.Models;

namespace Planetarium.Controllers
{
    public class ActivityController : Controller
    {
        public ActionResult ListActivities() {
            ActivityHandler dataAccess = new ActivityHandler();
            ViewBag.activities = dataAccess.GetAllActivities();
            return View();
        }
    }
}