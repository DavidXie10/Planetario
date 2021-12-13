using System.Web.Mvc;

namespace Planetarium.Controllers {
    public class CelestialBodiesController : Controller {
        public ActionResult SolarSystem3DModel() {
            //Modelo implementado por Julian Garnier
            return View();
        }

        public ActionResult CelestialBodyComparator() {
            return View();
        }
    }
}