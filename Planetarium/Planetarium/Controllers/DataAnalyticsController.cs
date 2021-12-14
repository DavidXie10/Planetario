using Planetarium.Handlers;
using Planetarium.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Planetarium.Controllers {
    public class DataAnalyticsController : Controller {
        public DataAnalyticsHandler AnalyticsAccess { get; set; }

        public DataAnalyticsController() {
            AnalyticsAccess = new DataAnalyticsHandler();
        }

        public ActionResult StoreReports() {
            return View();
        }

        public JsonResult GetSimpleItemReport(DateTime startDate, DateTime endDate) {
            List<ItemModel> items = AnalyticsAccess.GetSimpleItemReport(startDate, endDate);
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSimpleTicketReport(DateTime startDate, DateTime endDate) {
            List<TicketModel> items = AnalyticsAccess.GetSimpleTicketReport(startDate, endDate);
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAdvanceItemReport(DateTime startDate, DateTime endDate) {
            List<ItemAdvanceModel> items = AnalyticsAccess.GetAdvanceItemReport(startDate, endDate);
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAdvanceTicketReport(DateTime startDate, DateTime endDate) {
            List<TicketAdvanceModel> items = AnalyticsAccess.GetAdvanceTicketReport(startDate, endDate);
            return Json(items, JsonRequestBehavior.AllowGet);
        }
    }
}