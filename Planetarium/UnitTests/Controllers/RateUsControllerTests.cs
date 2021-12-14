using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Planetarium.Controllers;
using Planetarium.Models;


namespace UnitTests.Controllers
{
    [TestClass]
    public class RateUsControllerTests
    {
        [TestMethod]
        public void TestSuccessProcessRateViewNotNull()
        {
            //Arrange
            RateUSController controller = new RateUSController();

            //Act
            ViewResult result = controller.SuccessProcessRate() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestSuccessProcessRateViewName()
        {
            //Arrange
            RateUSController controller = new RateUSController();
            ViewResult view = controller.SuccessProcessRate() as ViewResult;
            Assert.AreEqual("SuccessProcessRate", view.ViewName);
        }

        [TestMethod]
        public void TestIndexRateViewNotNull()
        {
            //Arrange
            RateUSController controller = new RateUSController();

            //Act
            ViewResult result = controller.IndexRate() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestIndexRateViewName()
        {
            //Arrange
            RateUSController controller = new RateUSController();
            ViewResult view = controller.IndexRate() as ViewResult;
            Assert.AreEqual("IndexRate", view.ViewName);
        }

       
        [TestMethod]
        public void TestUXEvaluationFileName()
        {
            //Arrange
            RateUSController controller = new RateUSController();
            string link = "https://docs.google.com/forms/d/e/1FAIpQLScpghh7KECEEjpnpJHqG1l9Zr2a4gcnDCpcKVpN1C2xt1ZMHw/viewform?embedded=true";
            ViewResult view = controller.UXEvaluation() as ViewResult;
            Assert.AreEqual(link, view.ViewBag.Link);
        }

        [TestMethod]
        public void TestUXEvaluationViewName()
        {
            //Arrange
            RateUSController controller = new RateUSController();
            ViewResult view = controller.UXEvaluation() as ViewResult;
            Assert.AreEqual("UXEvaluation", view.ViewName);
        }

    }
}
