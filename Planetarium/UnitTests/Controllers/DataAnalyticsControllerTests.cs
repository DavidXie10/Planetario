using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planetarium.Controllers;
using System;
using System.Web.Mvc;

namespace UnitTests.Controllers {
    [TestClass]
    public class DataAnalyticsControllerTests {
        [TestMethod]
        public void TestStoreReportsViewNotNull() {
            DataAnalyticsController controller = new DataAnalyticsController();
            ViewResult result = controller.StoreReports() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestGetSimpleItemReportNotNull() {
            DataAnalyticsController controller = new DataAnalyticsController();
            DateTime start = new DateTime(2000, 01, 01);
            DateTime end = new DateTime(2000, 01, 01);
            JsonResult result = controller.GetSimpleItemReport(start, end);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestGetSimpleTicketReportNotNull() {
            DataAnalyticsController controller = new DataAnalyticsController();
            DateTime start = new DateTime(2000, 01, 01);
            DateTime end = new DateTime(2000, 01, 01);
            JsonResult result = controller.GetSimpleTicketReport(start, end);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestGetAdvanceItemReportNotNull() {
            DataAnalyticsController controller = new DataAnalyticsController();
            DateTime start = new DateTime(2000, 01, 01);
            DateTime end = new DateTime(2000, 01, 01);
            JsonResult result = controller.GetAdvanceItemReport(start, end);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestGetAdvanceTicketReportNotNull() {
            DataAnalyticsController controller = new DataAnalyticsController();
            DateTime start = new DateTime(2000, 01, 01);
            DateTime end = new DateTime(2000, 01, 01);
            JsonResult result = controller.GetAdvanceTicketReport(start, end);
            Assert.IsNotNull(result);
        }
    }
}