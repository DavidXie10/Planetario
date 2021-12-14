using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planetarium.Controllers;
using System.Web.Mvc;

namespace UnitTests.Controllers {
    [TestClass]
    public class StreamingControllerTests {

        [TestMethod]
        public void TestStreamingFormActivitiesCountIsCorrect() {
            int activitiesCount = 6;
            StreamingController controller = new StreamingController();
            
            ViewResult result = controller.StreamingForm() as ViewResult;

            Assert.AreEqual(activitiesCount, result.ViewBag.DropDownActivitiesNames.Count);
        }
    }
}
