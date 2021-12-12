using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Planetarium.Controllers;

namespace Planetarium.Tests.Controllers {
    [TestClass]
    public class SouvenirControllerTests {
        [TestMethod]
        public void TestCatalogViewNotNull() {
            //Arrange
            SouvenirController controller = new SouvenirController();

            //Act
            ViewResult result = controller.Catalog() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestHomeDeliveryViewNotNull() {
            //Arrange
            SouvenirController controller = new SouvenirController();

            //Act
            ViewResult result = controller.HomeDelivery() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
