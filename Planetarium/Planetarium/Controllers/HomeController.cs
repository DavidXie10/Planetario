using System.Collections.Generic;
using System.Web.Mvc;
using Planetarium.Handlers;
using Planetarium.Models;

namespace Planetarium.Controllers {
    public class HomeController : Controller {
        public ContentParser ContentParser = new ContentParser();
        public AuthHandler AuthDataAccess = new AuthHandler();
        public CouponHandler CouponDataAccess = new CouponHandler();
        public VisitorHandler VisitorDataAccess = new VisitorHandler();

        public ActionResult Index() {

            NewsHandler dataAccess = new NewsHandler();
            EducationalActivityHandler educationalActivityHandler = new EducationalActivityHandler();
            RssFeedHandler rssHandler = new RssFeedHandler();
            CouponHandler couponHandler = new CouponHandler();

            List<EventModel> feed = rssHandler.GetRssFeed();
            List<EventModel> eventFeed = rssHandler.GetEventsFromFeed("https://www.timeanddate.com/astronomy/sights-to-see.html");
            
            ViewBag.Events = feed;
            ViewBag.EventsToCal = eventFeed;

            List<StreamingModel> streamings = ContentParser.GetContentsFromJson<StreamingModel>("Streamings.json", ContentParser.GetStreamingsFromJson);
            ViewBag.Streamings = streamings;

            if (Request.Cookies["userIdentity"] != null) {
                UserModel user = AuthDataAccess.GetUserByUsername(Request.Cookies["userIdentity"].Value);
                ViewBag.VisitorDni = VisitorDataAccess.GetVisitorByDni(user.Dni, true).Dni;
                ViewBag.Coupons = CouponDataAccess.GetAllCoupons(user.Dni);
            } 

            ViewBag.Us = WhoWeAre();
            ViewBag.Activities = educationalActivityHandler.GetAllApprovedActivities();
            ViewBag.News = dataAccess.GetAllNews();
            ViewBag.length = 3;
            return View();
        }
        
        public ActionResult FindUs() {
            dynamic jsonContent = ContentParser.ParseFromJSON("Services.json");
            string[] schedule = jsonContent.Horarios.ToObject<string[]>();
            string[] transportBuses = jsonContent.Buses.ToObject<string[]>();
            string[] transportTrains = jsonContent.Trenes.ToObject<string[]>();
            string[] parking = jsonContent.Parqueos.ToObject<string[]>();
            
            ViewBag.Parking = parking;
            ViewBag.TransportBuses = transportBuses;
            ViewBag.TransportTrains = transportTrains;
            ViewBag.Schedule = schedule;
            return View();
        }

        public ActionResult WhoWeAre() {
            ContentParser contentParser = new ContentParser();

            dynamic jsonContent = contentParser.ParseFromJSON("Planetario.json");
            string mision = jsonContent.Mision;
            string vision = jsonContent.Vision;

            ViewBag.MissionMessage = mision;
            ViewBag.VisionMessage = vision;
            return View();
        }

        public ActionResult Success() {
            ViewBag.Message = "Su información ha sido agregada exitosamente";
            ViewBag.Title = "Success";
            return View();
        }

    }
}