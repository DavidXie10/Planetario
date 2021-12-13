using System.Web.Mvc;
using Planetarium.Handlers;

namespace Planetarium.Controllers {
    public class CouponController : Controller {
        public CouponHandler CouponHandler { get; set; }

        public CouponController() {
            CouponHandler = new CouponHandler();
        }

        public ActionResult Coupons() {
            ViewBag.Coupons = CouponHandler.GetAllCoupons("402540855"); //TODO: cambiar por la persona del rol
            return View();
        }
    }
}