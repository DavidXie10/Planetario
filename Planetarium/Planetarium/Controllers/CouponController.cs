using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planetarium.Controllers
{
    public class CouponController : Controller
    {
        // GET: Coupon
        public ActionResult Coupons()
        {
            return View();
        }
    }
}