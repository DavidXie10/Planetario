using Planetarium.Handlers;
using Planetarium.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planetarium.Controllers
{
    public class DataAnalyticsController : Controller
    {
        public DataAnalyticsHandler AnalyticsAccess { get; set; }

        public DataAnalyticsController() {
            AnalyticsAccess = new DataAnalyticsHandler();
        }

        public ActionResult StoreItemReport() {
            DateTime end = new DateTime(2100,12,31);
            DateTime begin = new DateTime(2000, 1, 1);
            ViewBag.ItemReport = AnalyticsAccess.GetItemReport(begin, end);
            return View();
        }

        [HttpPost]
        public ActionResult ItemReportByDate(DateTime begin, DateTime end) {
            ActionResult view = RedirectToAction("DataAnalytics", "StoreReports");

            ViewBag.ItemReport = AnalyticsAccess.GetItemReport(begin, end);
            return view;
        }

        public ActionResult StoreTicketReport() {
            DateTime end = new DateTime(2100, 12, 31);
            DateTime begin = new DateTime(2000, 1, 1);
            ViewBag.ItemReport = AnalyticsAccess.GetTicketReport(begin, end);
            return View();
        }

        [HttpPost]
        public ActionResult TicketReportByDateEvent(DateTime begin, DateTime end) {
            ActionResult view = RedirectToAction("DataAnalytics", "StoreReports");

            ViewBag.ItemReport = AnalyticsAccess.GetTicketReport(begin, end);
            return view;
        }

        public JsonResult GetSimpleReport(DateTime startDate, DateTime endDate) {
            List<ItemModel> items = AnalyticsAccess.GetItemReport(startDate, endDate);
            return Json(items, JsonRequestBehavior.AllowGet);
        }
    }
}