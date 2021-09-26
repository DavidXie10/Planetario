using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Handlers;

namespace Planetarium.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        public ActionResult ListNews()
        {
            NewsHandler dataAccess = new NewsHandler();
            ViewBag.activities = dataAccess.GetAllNews();
            return View();
        }
    }
}