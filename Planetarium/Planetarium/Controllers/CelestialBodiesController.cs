using Planetarium.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planetarium.Controllers
{
    public class CelestialBodiesController : Controller
    {
        ContentParser parser { get; set; }


        public CelestialBodiesController() {
            this.parser = new ContentParser();
        }

        public ActionResult CeletialBodyComparator()
        {
            List<SelectListItem> liBodies = new List<SelectListItem>();
            List<CelestialBody> bodies = GetBodies();
            foreach (CelestialBody body in bodies) {
                liBodies.Add(new SelectListItem { Text = body.Name, Value = body.Name });
            }
            ViewData["bodies"] = liBodies;
            return View();
        }

        public JsonResult GetCelestialBodiesById(string ids) {
            List<string> idList = ids.Split(',').ToList();
            List<CelestialBody> bodies = GetBodies(idList);
            return Json(bodies, JsonRequestBehavior.AllowGet);
        }

        public List<CelestialBody> GetBodies() {
            dynamic jsonCollection = this.parser.ParseFromJSON("CelestialBodies.json");
            return this.parser.GetCelestialBodiesFromJson(jsonCollection);
        }

        public List<CelestialBody> GetBodies(List<string> ids) {
            dynamic jsonCollection = this.parser.ParseFromJSON("CelestialBodies.json");
            List<CelestialBody> bodies = this.parser.GetCelestialBodiesFromJson(jsonCollection);
            foreach(CelestialBody body in bodies) {
                if (!ids.Contains(body.Name)) {
                    bodies.Remove(body);
                }
            }
            return bodies;
        }
    }
}