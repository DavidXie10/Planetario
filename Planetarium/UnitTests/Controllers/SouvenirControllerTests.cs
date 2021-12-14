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
            SouvenirController controller = new SouvenirController();
            ViewResult result = controller.Catalog() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestHomeDeliveryViewNotNull() {
            SouvenirController controller = new SouvenirController();
            ViewResult result = controller.HomeDelivery() as ViewResult;
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

        [TestMethod]
        public void TestCatalogContainsSouvenir() {
            int souvenir = 9;
            SouvenirController controller = new SouvenirController();

            ViewResult result = controller.Catalog() as ViewResult;
            List<SouvenirModel> catalog = result.ViewBag.Catalog;

            Assert.IsTrue(catalog.Exists(model => model.SouvenirId == souvenir));
        }

        [TestMethod]
        public void TestCatalogSouvenirsCountIsCorrect() {
            int souvenirsCount = 9;
            SouvenirController controller = new SouvenirController();

            ViewResult result = controller.Catalog() as ViewResult;
            List<SouvenirModel> souvenirs = result.ViewBag.Catalog;

            Assert.AreEqual(souvenirs.Count, souvenirsCount);
        }

        [TestMethod]
        public void TestGetItemsAndCountSouvenirCountIsCorrect() {
            string[] items = { "1", "1", "2", "1", "2", "3", "3", "4", "1", "1" };
            string selectedItem = "1";
            int expectedCount = 5;
            SouvenirController controller = new SouvenirController();

            Dictionary<string, int> selectedItems = controller.GetItemsAndCount(items);

            Assert.AreEqual(expectedCount, selectedItems[selectedItem]);
        }

        [TestMethod]
        public void TestGetItemsAndCountItemsCountIsCorrect() {
            string[] items = { "1", "1", "2", "1", "2", "3", "3", "4", "1", "1"};
            int expectedCount = 4;
            SouvenirController controller = new SouvenirController();

            Dictionary<string, int> selectedItems = controller.GetItemsAndCount(items);

            Assert.AreEqual(expectedCount, selectedItems.Count);
        }
    }
}