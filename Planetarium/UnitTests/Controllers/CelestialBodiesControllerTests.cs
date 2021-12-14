using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planetarium.Controllers;
using System;
using System.Web.Mvc;

namespace UnitTests.Controllers {
    [TestClass]
    public class CelestialBodiesControllerTests {
        [TestMethod]
        public void TestSolarSystem3DModelViewNotNull() {
            CelestialBodiesController controller = new CelestialBodiesController();
            ViewResult result = controller.SolarSystem3DModel() as ViewResult;
            Assert.IsNotNull(result);
        }
    }
}
