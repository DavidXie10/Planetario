using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Handlers;
using Planetarium.Models;

namespace Planetarium.Controllers {
    public class SouvenirController : Controller  {
        SouvenirHandler SouvenirHandler { get; set; }
        public AuthorizationController AuthController { get; set; }

        public SouvenirController() {
            SouvenirHandler = new SouvenirHandler();
            AuthController = new AuthorizationController();
        }

        public ActionResult Catalog() {
            List<SouvenirModel> souvenirs = SouvenirHandler.GetAllItems();
            ViewBag.Catalog = souvenirs;
            return View();
        }

        public ActionResult HomeDelivery() {

            return View();
        }

        public ActionResult ShoppingCart() {
            string cartCookieValue = Request.Form["cookieValue"];
            if (cartCookieValue != "") {
                cartCookieValue = cartCookieValue.Substring(0, cartCookieValue.LastIndexOf(",") - 1);
                string[] items = cartCookieValue.Split(',');
                Dictionary<string, int> selectedItems = GetItemsAndCount(items);

                ViewBag.SelectedSouvenirs = SouvenirHandler.GetSelectedSouvenirs(selectedItems);
            }

            return View();
        }

        private Dictionary<string, int> GetItemsAndCount(string[] items) {
            Dictionary<string, int> selectedItems = new Dictionary<string, int>();
            foreach (string item in items) {
                ++selectedItems[item];
            }

            return selectedItems;
        }
    }
}