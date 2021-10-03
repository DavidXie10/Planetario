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

        public JsonResult GetTopicsList(string category) {
            List<SelectListItem> topicsList = new List<SelectListItem>();

            List<string> topicsFromCategory = dataAccess.GetTopicsByCategory(category);

            foreach (string topic in topicsFromCategory) {
                topicsList.Add(new SelectListItem { Text = topic, Value = topic });
            }

            return Json(new SelectList(topicsList, "Value", "Text"));
        }

        private List<SelectListItem> loadCategories() {
            List<string> categories = dataAccess.GetAllCategories();

            List<SelectListItem> liCategories = new List<SelectListItem>();
            foreach (string category in categories) {
                liCategories.Add(new SelectListItem { Text = category, Value = category });
            }

            return liCategories;
        }

        public ActionResult SubmitNewsForm() {
            ViewData["category"] = loadCategories();
            return View();
        }

        [HttpPost]
        public ActionResult PostNews(NewsModel news) {
            List<string> categories = dataAccess.GetAllCategories();

            List<SelectListItem> liCategories = new List<SelectListItem>();
            foreach (string category in categories) {
                liCategories.Add(new SelectListItem { Text = category, Value = category });
            }

            ViewData["category"] = liCategories;
            ActionResult view = RedirectToAction("Success", "Home");
            ActionResult view = RedirectToAction("Success", "Home");
            news.Category = Request.Form["Category"].Replace(" ", "_");
            news.Topics = contentParser.GetTopicsFromString(Request.Form["topicsString"]);
            news.Title = Request.Form["title"];
            // TODO: author puede ser nulo
            news.Author = Request.Form["author"];
            news.Description = Request.Form["description"];
            news.Content = Request.Form["content"];

            ViewBag.SuccessOnCreation = false;
            try {
                ViewBag.SuccessOnCreation = this.dataAccess.PublishNews(news);
                if (ViewBag.SuccessOnCreation) {
                    //ViewBag.Message = "La noticia " + "\"" + news.Title + " \" fue creada con éxito";
                    ModelState.Clear();
                    return view;
                } else {
                    ViewBag.Error = true;
                    ViewBag.WarningMessage = "El titulo de la noticia esta vacio o no agregaron tópicos";
                    //view = RedirectToAction("Failure", "Home");
                    return View();
                }
            } catch (Exception exeption) {
                ViewBag.Message = exeption.ToString();
                ViewBag.Error = true;
                ViewBag.WarningMessage = "El nombre de la noticia \"" + news.Title + "\" esta repetido";
                return View();
            }
        }
    }
}