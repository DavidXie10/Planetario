using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Planetarium.Handlers;
using Planetarium.Models;

namespace Planetarium.Controllers {
    public class HomeController : Controller {
        ContentParser contentParser = new ContentParser();
        public ActionResult Index() {

            //Testing Cookies

            HttpCookie cookie = new HttpCookie("userIdentity");
            cookie.Value = "Carlos Espinoza";
            cookie.Expires = System.DateTime.Now.AddMinutes(2);
            Response.Cookies.Add(cookie);

            //End of Testing Cookies

            NewsHandler dataAccess = new NewsHandler();
            EducationalActivityHandler educationalActivityHandler = new EducationalActivityHandler();
            RssFeedHandler rssHandler = new RssFeedHandler();
            
            List<EventModel> feed = rssHandler.GetRssFeed();
            List<EventModel> eventFeed = rssHandler.GetEventsFromFeed("https://www.timeanddate.com/astronomy/sights-to-see.html");
            
            ViewBag.Events = feed;
            ViewBag.EventsToCal = eventFeed;

            List<StreamingModel> streamings = contentParser.GetContentsFromJson<StreamingModel>("Streamings.json", contentParser.GetStreamingsFromJson);
            ViewBag.Streamings = streamings;
            ViewBag.Us = WhoWeAre();
            ViewBag.Activities = educationalActivityHandler.GetAllApprovedActivities();
            ViewBag.News = dataAccess.GetAllNews();
            ViewBag.length = 3;
            return View();
        }
        
        public ActionResult FindUs() {
            dynamic jsonContent = contentParser.ParseFromJSON("Services.json");
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