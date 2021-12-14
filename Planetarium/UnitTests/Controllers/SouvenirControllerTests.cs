using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Planetarium.Controllers;
using Planetarium.Models;
using System.Collections.Generic;

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

        [TestMethod]
        public void TestGetAllSelectedSouvenirCountIsCorrect() {
            SouvenirController souvenirController = new SouvenirController();
            string selectedItemsIds = "1,2,4,8,7,";
            int totalItemsCount = 5;

            List<SouvenirModel> selectedSouvenirs = souvenirController.GetAllSelectedSouvenir(selectedItemsIds);

            Assert.AreEqual(selectedSouvenirs.Count, totalItemsCount);
        }

        [TestMethod]
        public void TestCalculateTotalOfSelectedSouvenirsIsCorrect() {
            string selectedItemsIds = "1,4,3,";
            SouvenirController souvenirController = new SouvenirController();
            List<SouvenirModel> selectedSouvenirs = souvenirController.GetAllSelectedSouvenir(selectedItemsIds);

            double expectedTotal = 6000;
            double actualTotal = souvenirController.CalculateTotal(selectedSouvenirs);

            Assert.AreEqual(expectedTotal, actualTotal);
        }
    }
}