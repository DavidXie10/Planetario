using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planetarium.Controllers;
using System;
using System.Web.Mvc;

namespace UnitTests.Controllers {
    [TestClass]
    public class DataAnalyticsControllerTests {
        [TestMethod]
        public void TestStoreReportsViewNotNull() {
            //Arrange
            DataAnalyticsController controller = new DataAnalyticsController();

            //Act
            ViewResult result = controller.StoreReports() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}