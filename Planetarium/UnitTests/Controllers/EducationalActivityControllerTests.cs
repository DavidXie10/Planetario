using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Planetarium.Controllers;

namespace UnitTests.Controllers {
    [TestClass]
    public class EducationalActivityControllerTests {
        [TestMethod]
        public void TestAssignSeatActivityTitleIsCorrect() {
            string activityTitle = "Joyas del Firmamento";
            string activityDate = "2021-10-01";
            EducationalActivityController educationalActivityController = new EducationalActivityController();

            ViewResult view = educationalActivityController.AssignSeat("0", activityTitle, activityDate) as ViewResult;

            Assert.AreEqual(activityTitle, view.ViewBag.title);
        }
    }
}
