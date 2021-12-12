using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planetarium.Controllers;
using System;
using System.Web.Mvc;

namespace UnitTests.Controllers {
    [TestClass]
    public class CelestialBodiesControllerTests {
        [TestMethod]
        public void TestSolarSystem3DModelViewNotNull() {
            //Arrange
            CelestialBodiesController controller = new CelestialBodiesController();

            //Act
            ViewResult result = controller.SolarSystem3DModel() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
