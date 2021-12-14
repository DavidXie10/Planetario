using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planetarium.Controllers;
using Planetarium.Models;
using System;
using System.Web.Mvc;

namespace UnitTests.Controllers
{
    [TestClass]
    public class AccountControllerTests
    {
        [TestMethod]
        public void TestLoginViewResultNotNull()
        {
            AccountController accountController = new AccountController();
            ActionResult view = accountController.Login();
            Assert.IsNotNull(view);
        }

        [TestMethod]
        public void TestLoginViewResult()
        {
            AccountController accountController = new AccountController();
            ViewResult view = accountController.Login() as ViewResult;
            Assert.AreEqual("Login", view.ViewName);
        }

        [TestMethod]
        public void TestIncorrectLoginCredentialsRedirect()
        {
            AccountController accountController = new AccountController();
            LoginModel loginCredentials = new LoginModel { Password = "Contraseña", Username = "lionelMessi" };
            RedirectToRouteResult view = accountController.CheckCredentials(loginCredentials) as RedirectToRouteResult;

            Assert.AreEqual("Login", view.RouteValues["action"]);
        }
    }
}