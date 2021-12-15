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
            int waitingTime = 1000;
            driver = new ChromeDriver();
            string URL = "https://localhost:44363";
            driver.Manage().Window.Maximize();
            driver.Url = URL;

            ///Act
            LogIntoSolValle(waitingTime);

            //Assert
            string userIdentity = driver.Manage().Cookies.GetCookieNamed("userIdentity").Value;
            Assert.AreEqual("solvalle", userIdentity);

            driver.Quit();
        }

        [TestMethod]
        public void TestLoginAuthorizationLevel() {
            ///Arrange
            int waitingTime = 1000;
            driver = new ChromeDriver();
            string URL = "https://localhost:44363";
            driver.Manage().Window.Maximize();
            driver.Url = URL;

            ///Act
            LogIntoSolValle(waitingTime);

            //Assert
            string userIdentity = driver.Manage().Cookies.GetCookieNamed("authCookie").Value;
            Assert.AreEqual("6", userIdentity);

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
        public void TestEvaluateSite() {
            int waitingTime = 500;
            driver = new ChromeDriver();
            string URL = "https://localhost:44363";

            driver.Manage().Window.Maximize();

            driver.Url = URL;

            LogIntoSolValle(waitingTime);

            IWebElement rateUsNavbarOption = driver.FindElement(By.Id("navbarDropdownRateUs"));
            rateUsNavbarOption.Click();
            Thread.Sleep(waitingTime);

            IWebElement rateResults = driver.FindElement(By.Id("rateResults"));
            rateResults.Click();
            Thread.Sleep(waitingTime);

            IWebElement threeStarResult = driver.FindElement(By.Id("threeStarResult"));
            var previousResult = threeStarResult.Text;
            Thread.Sleep(waitingTime);

            driver.Url = URL;
            Thread.Sleep(waitingTime);

            rateUsNavbarOption = driver.FindElement(By.Id("navbarDropdownRateUs"));
            rateUsNavbarOption.Click();
            Thread.Sleep(waitingTime);

            IWebElement websiteExpierence = driver.FindElement(By.Id("websiteExperience"));
            websiteExpierence.Click();
            Thread.Sleep(waitingTime);

            IWebElement rating3 = driver.FindElement(By.Id("rating3Test"));
            rating3.Click();
            Thread.Sleep(waitingTime);

            IWebElement sendRate = driver.FindElement(By.ClassName("btn-primary"));
            sendRate.Click();
            Thread.Sleep(waitingTime);

            rateUsNavbarOption = driver.FindElement(By.Id("navbarDropdownRateUs"));
            rateUsNavbarOption.Click();
            Thread.Sleep(waitingTime);

            rateResults = driver.FindElement(By.Id("rateResults"));
            rateResults.Click();
            Thread.Sleep(waitingTime);

            threeStarResult = driver.FindElement(By.Id("threeStarResult"));
            var resultWithAddition = threeStarResult.Text;
            Thread.Sleep(waitingTime);

            Assert.AreNotEqual(previousResult.ToString(), resultWithAddition.ToString());

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
    }
}
