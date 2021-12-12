using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Planetarium.Controllers;
using Planetarium.Models;

namespace UnitTests.Controllers {
    [TestClass]
    public class AccountControllerTest {
        [TestMethod]
        public void TestLoginViewResultNotNull() {
            AccountController accountController = new AccountController();
            ActionResult view = accountController.Login();
            Assert.IsNotNull(view);
        }

        [TestMethod]
        public void TestLoginViewResult() {
            AccountController accountController = new AccountController();
            ViewResult view = accountController.Login() as ViewResult;
            Assert.AreEqual("Login", view.ViewName);
        }

        [TestMethod]
        public void TestCorrectLoginCredentials() {
            AccountController accountController = new AccountController();
            LoginModel loginCredentials = new LoginModel { Password = "Mau", Username = "solvalle" };
            RedirectToRouteResult view = accountController.CheckCredentials(loginCredentials) as RedirectToRouteResult;

            Assert.AreEqual("Index", view.RouteValues["action"]);
        }

        [TestMethod]
        public void TestIncorrectLoginCredentials() {

        }
    }
}
