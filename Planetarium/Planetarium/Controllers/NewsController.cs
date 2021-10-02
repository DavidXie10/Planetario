using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Handlers;
using Planetarium.Models;

namespace Planetarium.Controllers {
    public class NewsController : Controller {

        public NewsHandler dataAccess { get; set; }
        public ContentParser contentParser { get; set; }
        public NewsController() {
            dataAccess = new NewsHandler();
            contentParser = new ContentParser();

        }


        public ActionResult ListNews() {
            NewsHandler dataAccess = new NewsHandler();
            ViewBag.News = dataAccess.GetAllNews();
            return View();
        }

        public ActionResult News() {
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


        [HttpPost]
        public ActionResult SubmitNews() {
            NewsModel news = new NewsModel();
            ActionResult successView = RedirectToAction("News", "ListNews");
            //faq.Category = Request.Form["Category"].Replace(" ", "_");
            //faq.Topics = contentParser.GetTopicsFromString(Request.Form["topicsString"]);
            //faq.Question = Request.Form["question"];
            //faq.Answer = Request.Form["answer"];
            //faq.QuestionId = -1;

            ViewBag.SuccessOnCreation = false;
            try {
                if (ModelState.IsValid) {
                    ViewBag.SuccessOnCreation = this.dataAccess.PublishNews(news);
                    if (ViewBag.SuccessOnCreation) {
                        ViewBag.Message = "La noticia " + "\"" + news.Title + " \" fue creada con éxito";
                        ModelState.Clear();
                    }
                }
                return successView;
            } catch {
                ViewBag.Message = "Algo salió mal y no fue posible crear la noticia";
                return successView;
            }
        }

    }
}