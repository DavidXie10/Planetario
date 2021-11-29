using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Models;

namespace Planetarium.Controllers
{
    public class CouponController : Controller
    {
        ContentParser parser = new ContentParser();
        // GET: Coupon
        public ActionResult Coupons()
        {
            List<CouponModel> coupons = parser.GetContentsFromJson<CouponModel>("Coupons.json", parser.GetCouponsFromJson);
            ViewBag.Coupons = coupons;
            return View();
        }
    }
}