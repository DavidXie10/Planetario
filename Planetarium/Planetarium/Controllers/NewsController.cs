using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Handlers;
using Planetarium.Models;

namespace Planetarium.Controllers {
    public class NewsController : Controller {
        public ActionResult ListNews() {
            NewsHandler dataAccess = new NewsHandler();
            ViewBag.News = dataAccess.GetAllNews();
            return View();
        }

        public ActionResult News() {
            return View();
        }

        public ActionResult EditNews() {
            return View();
        }

        [HttpGet]
        public ActionResult News(string title) {
            ActionResult view;
            try {
                NewsHandler dataAccess = new NewsHandler();
                NewsModel news = dataAccess.GetAllNews().Find(smodel => String.Equals(smodel.Title, title));
                if (news == null) { 
                    view = RedirectToAction("ListNews");
                } else {
                    ViewBag.News = news;
                    view = View(news);
                }
            } catch {
                view = RedirectToAction("ListNews");
            }
            return view;
        }
    }
}