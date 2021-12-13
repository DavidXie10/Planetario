using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Planetarium.Handlers;
using Planetarium.Models;
using System.IO;

namespace Planetarium.Controllers {
    public class NewsController : Controller {

        public NewsHandler DataAccess { get; set; }
        public ContentParser ContentParser { get; set; }

        public NewsController() {
            DataAccess = new NewsHandler();
            ContentParser = new ContentParser();
        }

        public ActionResult ListNews() {
            NewsHandler dataAccess = new NewsHandler();
            ViewBag.News = dataAccess.GetAllNews();
            RssFeedHandler rssHandler = new RssFeedHandler();
            List<EventModel> feed = rssHandler.GetRssFeed();
            ViewBag.NewsFromInternet = feed;
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
            List<string> topicsFromCategory = DataAccess.GetTopicsByCategory(category);

            foreach (string topic in topicsFromCategory) {
                topicsList.Add(new SelectListItem { Text = topic, Value = topic });
            }
            return Json(new SelectList(topicsList, "Value", "Text"));
        }
        
        private List<SelectListItem> LoadCategories() {
            List<string> categories = DataAccess.GetAllCategories();
            List<SelectListItem> dropdownCategories = new List<SelectListItem>();
            foreach (string category in categories) {
                dropdownCategories.Add(new SelectListItem { Text = category, Value = category });
            }
            return dropdownCategories;
        }

        public ActionResult SubmitNewsForm() {
            ViewData["category"] = LoadCategories();
            return View();
        }

        [HttpPost]
        public ActionResult PostNews(NewsModel news) {
            ActionResult view = RedirectToAction("Success", "Home");
            LoadNewsWithForm(news);
            ViewBag.SuccessOnCreation = false;
            try {
                ViewBag.SuccessOnCreation = this.DataAccess.PublishNews(news);
                if (ViewBag.SuccessOnCreation) {
                    ModelState.Clear();
                    view = RedirectToAction("Success", "Home");
                }
            } catch {
                TempData["Error"] = true;
                TempData["WarningMessage"] = "Algo salió mal";
                view = RedirectToAction("SubmitNewsForm", "News");
            }
            return view;
        }

        private void LoadNewsWithForm(NewsModel news) {
            news.Category = Request.Form["Category"].Replace(" ", "_");
            news.Topics = ContentParser.GetListFromString(Request.Form["inputTopicString"]);
            news.Title = Request.Form["title"];
            news.Description = Request.Form["description"];
            news.Content = Request.Form["content"];
            news.ImagesRef = ContentParser.GetListFromString(Request.Form["imagesString"]);
        }

        [HttpPost]
        public ActionResult UploadFiles(IEnumerable<HttpPostedFileBase> files) {
            foreach (var file in files) {
                string filePath = file.FileName.Replace("_", "-").Replace(" ", "-");
                file.SaveAs(Path.Combine(Server.MapPath("~/images/news"), filePath));
            }
            return Json("Files uploaded successfully");
        }
    }
}