using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Handlers;
using Planetarium.Models;

namespace Planetarium.Controllers  {
    public class CalendarController : Controller {

        public RssFeedHandler eventHandler = new RssFeedHandler();
        public EducationalActivityHandler activityHandler = new EducationalActivityHandler();

        public ActionResult Calendar() {
            return View();
        }

        public JsonResult GetEventsForGeneralCalendar() {
            List<EventModel> eventsList = eventHandler.GetEventsFromFeed();

            eventsList.AddRange(activityHandler.GetAllCalendarActivitiesFromState(1));

            return Json(eventsList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEventsForPhenomenonCalendar() {
            List<EventModel> eventsList = new List<EventModel>();
            eventsList = eventHandler.GetEventsFromFeed();
            return Json(eventsList, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult CreateEvent() {
            return View();
        }

        [HttpPost]
        public ActionResult PostCreateEvent(EducationalActivityEventModel educationalActivity) {
            ActionResult view = RedirectToAction("Calendar", "Calendar");
            ViewBag.SuccessOnCreation = false;
            educationalActivity.ActivityType = Request.Form["typeOfEvent"];

            try {
                ViewBag.SuccessOnCreation = this.activityHandler.ProposeEducationalActivity(educationalActivity);
                ModelState.Clear();
            } catch {
                TempData["Error"] = true;
                TempData["WarningMessage"] = "Algo salió mal";
                view = RedirectToAction("CreateEvent", "Calendar");
            }
            return view;
        }

    }
}