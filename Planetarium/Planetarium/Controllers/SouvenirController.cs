using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Handlers;
using Planetarium.Models;

namespace Planetarium.Controllers {
    public class SouvenirController : Controller {
        public SouvenirHandler SouvenirHandler { get; set; }
        public VisitorHandler VisitorDataAccess { get; set; }
        public AuthHandler AuthDataAccess { get; set; }
        public AuthorizationController AuthController { get; set; }

        public SouvenirController() {
            SouvenirHandler = new SouvenirHandler();
            VisitorDataAccess = new VisitorHandler();
            AuthDataAccess = new AuthHandler();
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
            string cartCookieValue = Request.Cookies["itemsCart"].Value;

            if (cartCookieValue != null && cartCookieValue != "") {
                UserModel user = AuthDataAccess.GetUserByUsername(Request.Cookies["userIdentity"].Value);

                ViewBag.VisitorDni = VisitorDataAccess.GetVisitorByDni(user.Dni, true).Dni;
                ViewBag.SelectedSouvenirs = GetAllSelectedSouvenir(cartCookieValue);
            }

            return View();
        }

        private Dictionary<string, int> GetItemsAndCount(string[] items) {
            Dictionary<string, int> selectedItems = new Dictionary<string, int>();

            foreach (string item in items) {
                if (item != "") {
                    if (selectedItems.ContainsKey(item)) {
                        selectedItems[item] += 1;
                    } else {
                        selectedItems.Add(item, 1);
                    }
                }
            }

            return selectedItems;
        }

        private List<SouvenirModel> GetAllSelectedSouvenir(string cartCookieValue) {
            cartCookieValue = cartCookieValue.Substring(0, cartCookieValue.LastIndexOf(","));
            string[] items = cartCookieValue.Split(',');
            Dictionary<string, int> selectedItems = GetItemsAndCount(items);
            List<SouvenirModel> selectedSouvenirs = SouvenirHandler.GetSelectedSouvenirs(selectedItems);

            return selectedSouvenirs;
        }

        public ActionResult PayMethod() {
            UserModel user = AuthDataAccess.GetUserByUsername(Request.Cookies["userIdentity"].Value);
            ViewBag.VisitorDni = VisitorDataAccess.GetVisitorByDni(user.Dni, true).Dni;

            SetViewBagValues("itemsCart");

            return View();
        }

        private void SetViewBagValues(string cookieName) {
            string cartCookieValue = Request.Cookies[cookieName].Value;

            if (cartCookieValue != null && cartCookieValue != "") {
                List<SouvenirModel> selectedSouvenirs = GetAllSelectedSouvenir(cartCookieValue);
                ViewBag.SelectedSouvenirs = selectedSouvenirs;
                ViewBag.Price = CalculateTotal(selectedSouvenirs);
            }
        }

        private double CalculateTotal(List<SouvenirModel> selectedSouvenirs) {
            double total = 0;

            foreach (SouvenirModel souvenir in selectedSouvenirs) {
                total += souvenir.Price * souvenir.SelectedCount;
            }

            return total;
        }

        public ActionResult Invoice() {
            ActionResult view = RedirectToAction("PayMethod", "Souvenir", new { });

            UserModel user = AuthDataAccess.GetUserByUsername(Request.Cookies["userIdentity"].Value);
            SetViewBagValues("checkoutCookie");
            ViewBag.Date = DateTime.Now;
            ViewBag.Tax = 0.13 * ViewBag.Price;

            try { 
                ViewBag.SuccessOnCreation = SouvenirHandler.RegisterSale(ViewBag.SelectedSouvenirs as List<SouvenirModel>, ViewBag.Price, ViewBag.Date, user.Dni);
                ViewBag.SuccessOnCreation = SouvenirHandler.UpdateSelectedItemsStock(ViewBag.SelectedSouvenirs as List<SouvenirModel>);
                if(ViewBag.SuccessOnCreation) {
                    view = View();
                }
            } catch(Exception e) {
                TempData["Error"] = true;
                TempData["WarningMessage"] = "Algo salió mal:" + e;
            }
            return view;
        }
    }

}