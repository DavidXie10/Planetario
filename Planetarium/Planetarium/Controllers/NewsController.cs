using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Handlers;
using Planetarium.Models;
using System.IO;

namespace Planetarium.Controllers {
    public class NewsController : Controller {

        public NewsHandler dataAccess { get; set; }
        public ContentParser contentParser { get; set; }
        public List<string> ImagesNames { get; set; }

        public NewsController() {
            dataAccess = new NewsHandler();
            contentParser = new ContentParser();
            ImagesNames = new List<string>();
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
            List<string> imagesNames = new List<string>();
            ViewBag.ImagesNames = imagesNames;
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
            news.Category = Request.Form["Category"].Replace(" ", "_");
            news.Topics = contentParser.GetTopicsFromString(Request.Form["topicsString"]);
            news.Title = Request.Form["title"];
            // TODO: author puede ser nulo
            news.Author = Request.Form["author"];
            news.Description = Request.Form["description"];
            news.Content = Request.Form["content"];

            List<string> imagesString = contentParser.GetTopicsFromString(Request.Form["imagesString"]);
            news.ImagesRef = imagesString;

            ViewBag.SuccessOnCreation = false;
            try {
                ViewBag.SuccessOnCreation = this.dataAccess.PublishNews(news);
                if (ViewBag.SuccessOnCreation) {
                    //ViewBag.Message = "La noticia " + "\"" + news.Title + " \" fue creada con éxito";
                    ModelState.Clear();
                    view = RedirectToAction("Success", "Home");
                    return view;
                } else {
                    TempData["Error"] = true;
                    TempData["WarningMessage"] = "El titulo de la noticia esta vacio, no agregaron tópicos o no selecionó categoría";
                    view = RedirectToAction("SubmitNewsForm", "News");
                    //view = RedirectToAction("Failure", "Home");
                    return view;
                }
            } catch (Exception exeption) {
                ViewBag.Message = exeption.ToString();
                ViewBag.Error = true;
                ViewBag.WarningMessage = "El nombre de la noticia \"" + news.Title + "\" esta repetido";
                return view;
            }

        }

        [HttpPost]
        public ActionResult UploadFiles(IEnumerable<HttpPostedFileBase> files) {
            
            foreach (var file in files) {
                string filePath = file.FileName;
                file.SaveAs(Path.Combine(Server.MapPath("~/images/news"), filePath));

                ImagesNames.Add(filePath);
            }

            return Json("file uploaded successfully");
        }


    }
}