using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Planetarium.Handlers;
using Planetarium.Models;

namespace Planetarium.Controllers {
    public class SouvenirController : Controller {
        public SouvenirHandler SouvenirHandler { get; set; }
        public VisitorHandler VisitorDataAccess { get; set; }
        public AuthHandler AuthDataAccess { get; set; }
        public AuthorizationController AuthController { get; set; }
        public CouponHandler CouponDataAccess { get; set; }

        public SouvenirController() {
            SouvenirHandler = new SouvenirHandler();
            VisitorDataAccess = new VisitorHandler();
            AuthDataAccess = new AuthHandler();
            AuthController = new AuthorizationController();
            CouponDataAccess = new CouponHandler();
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
            string cartCookieValue = "";
            if (Request.Cookies["itemsCart"] != null) {
                cartCookieValue = Request.Cookies["itemsCart"].Value;
            }

            if (cartCookieValue != null && cartCookieValue != "") {
                UserModel user = AuthDataAccess.GetUserByUsername(Request.Cookies["userIdentity"].Value);

                ViewBag.VisitorDni = VisitorDataAccess.GetVisitorByDni(user.Dni, true).Dni;
                ViewBag.SelectedSouvenirs = GetAllSelectedSouvenir(cartCookieValue);
            }

            return View();
        }

        public Dictionary<string, int> GetItemsAndCount(string[] items) {
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

        public List<SouvenirModel> GetAllSelectedSouvenir(string cartCookieValue) {
            cartCookieValue = cartCookieValue.Substring(0, cartCookieValue.LastIndexOf(","));
            string[] items = cartCookieValue.Split(',');
            Dictionary<string, int> selectedItems = GetItemsAndCount(items);
            List<SouvenirModel> selectedSouvenirs = SouvenirHandler.GetSelectedSouvenirs(selectedItems);

            return selectedSouvenirs;
        }

        public ActionResult PayMethod() {
            UserModel user = AuthDataAccess.GetUserByUsername(Request.Cookies["userIdentity"].Value);
            ViewBag.VisitorDni = VisitorDataAccess.GetVisitorByDni(user.Dni, true).Dni;
            ViewBag.Coupons = CouponDataAccess.GetAllCoupons(user.Dni);
            SetViewBagValues("itemsCart");
            ViewBag.Tax = 0.13 * ViewBag.Price;

            return View();
        }

        private void SetViewBagValues(string cookieName) {
            string cartCookieValue = Request.Cookies[cookieName].Value;

            if (cartCookieValue != null && cartCookieValue != "") {
                List<SouvenirModel> selectedSouvenirs = GetAllSelectedSouvenir(cartCookieValue);
                ViewBag.SelectedSouvenirs = selectedSouvenirs;
                ViewBag.Date = DateTime.Now;
                ViewBag.Price = CalculateTotal(ViewBag.SelectedSouvenirs);
            }
        }

        public double CalculateTotal(List<SouvenirModel> selectedSouvenirs) {
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
                ViewBag.Tax = 0.13 * ViewBag.Price;
                ViewBag.Discount = Convert.ToInt32(Request.Cookies["discount"].Value);
                ViewBag.FinalPrice = ViewBag.Price - ViewBag.Discount;

            try { 
                ViewBag.InvoiceNumber = SouvenirHandler.RegisterSale(ViewBag.SelectedSouvenirs as List<SouvenirModel>, ViewBag.Price, ViewBag.Date, user.Dni);
                ViewBag.SuccessOnCreation = SouvenirHandler.UpdateSelectedItemsStock(ViewBag.SelectedSouvenirs as List<SouvenirModel>);
                string couponCode = Request.Cookies["couponCode"].Value;

                if (ViewBag.Discount > 0 && CouponDataAccess.CheckCoupon(couponCode)) {
                    CouponDataAccess.DeleteCoupon(couponCode);
                }

                if(ViewBag.SuccessOnCreation) {
                    view = View();
                }
            } catch {
                TempData["Error"] = true;
                TempData["WarningMessage"] = "Algo salió mal y no se pudo generar el comprobante";
            }
            return view;
        }
    }
}