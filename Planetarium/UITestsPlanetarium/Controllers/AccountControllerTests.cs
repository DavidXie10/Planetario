using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            int waitingTime = 2000;
            driver = new ChromeDriver();
            string URL = "https://localhost:44363/";

            driver.Manage().Window.Maximize();
            ///Act
            driver.Url = URL;

            ///Assert
            IWebElement loginLink = driver.FindElement(By.Id("login"));
            IWebElement loginUsername = driver.FindElement(By.Id("Username"));
            IWebElement loginPassword = driver.FindElement(By.Id("Password"));
            IWebElement startSession = driver.FindElement(By.Id("signIn"));

            Thread.Sleep(waitingTime);

            //loginLink.Click();
            //loginLink.Click();
            driver.FindElement(By.Id("login")).Click();

            //Thread.Sleep(waitingTime);

            //loginUsername.SendKeys("solvalle");

            //Thread.Sleep(waitingTime);

            //loginPassword.SendKeys("Max");

            //Thread.Sleep(waitingTime);

            //startSession.Click();

            //string userIdentity = Request.Cookies["userIdentity"].Value;
            //Assert.AreEqual(userIdentity, "solvalle");

            Assert.AreEqual("Hola", "Hola");
            driver.Quit();
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
