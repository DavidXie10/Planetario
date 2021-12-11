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
            ViewBag.ItemReport = AnalyticsAccess.GetSimpleItemReport(begin, end);
            return View();
        }

        [HttpPost]
        public ActionResult ItemReportByDate(DateTime begin, DateTime end) {
            ActionResult view = RedirectToAction("DataAnalytics", "StoreReports");

            ViewBag.ItemReport = AnalyticsAccess.GetSimpleItemReport(begin, end);
            return view;
        }

        public ActionResult StoreTicketReport() {
            DateTime end = new DateTime(2100, 12, 31);
            DateTime begin = new DateTime(2000, 1, 1);
            ViewBag.ItemReport = AnalyticsAccess.GetSimpleTicketReport(begin, end);
            return View();
        }

        [HttpPost]
        public ActionResult TicketReportByDateEvent(DateTime begin, DateTime end) {
            ActionResult view = RedirectToAction("DataAnalytics", "StoreReports");

            ViewBag.ItemReport = AnalyticsAccess.GetSimpleTicketReport(begin, end);
            return view;
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