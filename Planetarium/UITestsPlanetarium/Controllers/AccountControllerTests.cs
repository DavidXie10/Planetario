﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace Plenetarium.UITesting {
    [TestClass]
    public class AccountControllerTests {
        IWebDriver driver;
        [TestMethod]
        public void TestLoginCredentialsSuccess() {
            ///Arrange
            int waitingTime = 1000;
            driver = new ChromeDriver();
            string URL = "https://localhost:44363";
            driver.Manage().Window.Maximize();
            driver.Url = URL;

            ///Act
            LogIntoSolValle(waitingTime);

            //Assert
            string userIdentity = driver.Manage().Cookies.GetCookieNamed("userIdentity").Value;
            Assert.AreEqual(userIdentity, "solvalle");

            driver.Quit();
        }

        [TestMethod]
        public void TestEliminateSouvenirWithIdOne() {
            ///Arrange
            int waitingTime = 1000;
            driver = new ChromeDriver();
            string URL = "https://localhost:44363";

            driver.Manage().Window.Maximize();
            ///Act
            driver.Url = URL;

            ///Assert
            LogIntoSolValle(waitingTime);

            IWebElement souvenirNavbarOption = driver.FindElement(By.Id("souvenirNavbar"));
            souvenirNavbarOption.Click();
            Thread.Sleep(waitingTime);

            IWebElement cartItem = driver.FindElement(By.Id("1-card"));
            cartItem.Click();
            Thread.Sleep(waitingTime);

            IWebElement addToCartItem = driver.FindElement(By.Id("button-1"));
            addToCartItem.Click();
            Thread.Sleep(waitingTime);

            IWebElement shoppingCart = driver.FindElement(By.Id("cartImage"));
            shoppingCart.Click();
            Thread.Sleep(waitingTime);

            IWebElement eliminateCartItem = driver.FindElement(By.Id("1"));
            eliminateCartItem.Click();
            Thread.Sleep(waitingTime);

            IWebElement yesButton = driver.FindElement(By.Id("yesButton"));
            yesButton.Click();
            Thread.Sleep(waitingTime);

            string selectedCartItems = driver.Manage().Cookies.GetCookieNamed("itemsCart").Value;
            Assert.AreEqual(selectedCartItems, "");
            
            driver.Quit();
        }

        [TestMethod]
        private void LogIntoSolValle(int waitingTime) {
            IWebElement loginLink = driver.FindElement(By.Id("login"));

            Thread.Sleep(waitingTime);

            loginLink.Click();

            Thread.Sleep(waitingTime);

            IWebElement loginUsername = driver.FindElement(By.Id("Username"));
            IWebElement loginPassword = driver.FindElement(By.Id("Password"));
            IWebElement startSession = driver.FindElement(By.Id("signIn"));

            Thread.Sleep(waitingTime);

            loginUsername.SendKeys("solvalle");

            Thread.Sleep(waitingTime);

            loginPassword.SendKeys("Max");

            Thread.Sleep(waitingTime);

            startSession.Click();
        }




        [TestMethod]
        public void TestSearchStreetFighterVThenVerifyStreetFighterVIsDisplayed() {
            int waitingTime = 2000;
            By googleSearchBar = By.Name("q");
            By googleSearchButton = By.Name("btnK");
            By googleLuckyButton = By.Name("btnI");
            By googleResultText = By.XPath(".//h2//span[text()='Street Fighter V']");
            By googleLogo = By.TagName("img");

            IWebDriver webDriver = new ChromeDriver();

            webDriver.Navigate().GoToUrl("https://www.google.co.uk");

            webDriver.FindElement(googleSearchBar).SendKeys("Street Fighter V");

            webDriver.Manage().Window.Maximize();

            //Thread.Sleep(waitingTime);

            //webDriver.FindElement(googleLogo).Click();

            Thread.Sleep(waitingTime);

            webDriver.FindElement(googleSearchBar).Submit();

            Thread.Sleep(waitingTime);

            var actualResultText = webDriver.FindElement(googleResultText);

            Assert.IsTrue(actualResultText.Text.Equals("Street Fighter V"));

            webDriver.Quit();
        }



    }
}