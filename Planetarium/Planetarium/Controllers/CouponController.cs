using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Models;
using Planetarium.Handlers;

namespace Planetarium.Controllers {
    public class CouponController : Controller {
        public CouponHandler couponHandler { get; set; }

        public CouponController() {
            couponHandler = new CouponHandler();
        }

        public ActionResult Coupons() {
            ViewBag.Coupons = couponHandler.GetAllCoupons("402540855"); //TODO: cambiar por la persona del rol
            return View();
        }
    }
}