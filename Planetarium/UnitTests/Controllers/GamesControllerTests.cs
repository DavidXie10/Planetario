using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Planetarium.Controllers;
using Planetarium.Models;

namespace UnitTests.Controllers
{
    [TestClass]
    public class GamesControllerTests
    {
        [TestMethod]
        public void TestHangManGameNotNull()
        {
            //Arrange
            GamesController controller = new GamesController();

            //Act
            ViewResult result = controller.HangManGame() as ViewResult;

            // Assert
            Assert.IsNotNull(result);

        }

        [TestMethod]
        public void TestMemoryGameNotNull()
        {
            //Arrange
            GamesController controller = new GamesController();

            //Act
            ViewResult result = controller.MemoryGame() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestTypingGameNotNull()
        {
            //Arrange
            GamesController controller = new GamesController();

            //Act
            ViewResult result = controller.TypingGame() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestRocketGameNotNull()
        {
            //Arrange
            GamesController controller = new GamesController();

            //Act
            ViewResult result = controller.OuterSpaceGame() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestSpaceGameNotNull()
        {
            //Arrange
            GamesController controller = new GamesController();

            //Act
            ViewResult result = controller.SpaceGame() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        
        public void TestListOfGamesNotNull()
        {
            //Arrange
            GamesController controller = new GamesController();

            //Act
            ViewResult result = controller.ListOfGames() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestMemoryViewBagNotNull()
        {
            //Arrange
            GamesController controller = new GamesController();
            ViewResult view = controller.MemoryGame() as ViewResult;
            Assert.IsNotNull(view.ViewBag.Scientists);
        }

        [TestMethod]
        public void TestHangManGameViewNameResult()
        {
            //Arrange
            GamesController controller = new GamesController();
            ViewResult view = controller.HangManGame() as ViewResult;
            Assert.AreEqual("HangManGame", view.ViewName);
        }

        [TestMethod]
        public void TestMemoryGameViewNameResult()
        {
            //Arrange
            GamesController controller = new GamesController();
            ViewResult view = controller.MemoryGame() as ViewResult;
            Assert.AreEqual("MemoryGame", view.ViewName);
        }

        [TestMethod]
        public void TestTypingGameViewNameResult()
        {
            //Arrange
            GamesController controller = new GamesController();
            ViewResult view = controller.TypingGame() as ViewResult;
            Assert.AreEqual("TypingGame", view.ViewName);
        }

        [TestMethod]
        public void TestSpaceGameViewNameResult()
        {
            //Arrange
            GamesController controller = new GamesController();
            ViewResult view = controller.SpaceGame() as ViewResult;
            Assert.AreEqual("SpaceGame", view.ViewName);
        }

        [TestMethod]
        public void TestOuterSpaceGameViewNameResult()
        {
            //Arrange
            GamesController controller = new GamesController();
            ViewResult view = controller.OuterSpaceGame() as ViewResult;
            Assert.AreEqual("OuterSpaceGame", view.ViewName);
        }

        [TestMethod]
        public void TestListOfGamesViewNameResult()
        {
            //Arrange
            GamesController controller = new GamesController();
            ViewResult view = controller.ListOfGames() as ViewResult;
            Assert.AreEqual("ListOfGames", view.ViewName);
        }

    }
}
