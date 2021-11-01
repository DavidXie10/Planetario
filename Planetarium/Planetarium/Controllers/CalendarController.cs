using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Handlers;
using Planetarium.Models;

namespace Planetarium.Controllers
{
    public class CalendarController : Controller
    {

        public RssFeedHandler eventHandler = new RssFeedHandler();
        public EducationalActivityHandler activityHandler = new EducationalActivityHandler();

        public ActionResult Calendar()
        {
            return View();
        }

        public JsonResult GetEventsForCalendar() {
            List<EventModel> eventsList = new List<EventModel>();
            eventsList = eventHandler.TestHTML();
            eventsList = activityHandler.GetEventsForCalendar(eventsList);
            return Json(eventsList, JsonRequestBehavior.AllowGet);
        }
    }
}