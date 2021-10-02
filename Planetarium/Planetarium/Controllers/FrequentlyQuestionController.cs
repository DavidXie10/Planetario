using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Planetarium.Models;
using Planetarium.Handlers;
using System.Web.Mvc;
using System.Diagnostics;

namespace Planetarium.Controllers {
    public class FrequentlyQuestionController : Controller {
        public FrequentlyQuestionHandler dataAccess {  get; set;  }

        public FrequentlyQuestionController() {
            dataAccess = new FrequentlyQuestionHandler();

        }


        public ActionResult CreateFrequentlyAskedQuestion() {

            List<string> categories = dataAccess.GetAllCategories();

            List<SelectListItem> liCategories = new List<SelectListItem>();
            foreach(string category in categories) {
                liCategories.Add(new SelectListItem { Text = category, Value = category });
            }

            ViewData["category"] = liCategories;


            return View();
        }

        public JsonResult GetTopicsList(string category) {
            List<SelectListItem> topicsList = new List<SelectListItem>();

            List<string> topicsFromCategory = dataAccess.GetTopicsByCategory(category);

            foreach(string topic in topicsFromCategory) {
                topicsList.Add(new SelectListItem { Text = topic, Value = topic });
            }

            return Json(new SelectList(topicsList, "Value", "Text"));
        }

        [HttpPost]
        public ActionResult CreateFrequentlyAskedQuestionRawHTML() {

            FrequentlyQuestionModel faq = new FrequentlyQuestionModel();
            faq.Category = Request.Form["category"].Replace(" ", "_");
            faq.Topics = new List<string>();
            faq.Topics.Add(Request.Form["topic"]);
            faq.Question = Request.Form["question"];
            faq.Answer = Request.Form["Answer"];
            faq.QuestionId = -1;

            ViewBag.SuccessOnCreation = false;
            try {
                if (ModelState.IsValid) {
                    ViewBag.SuccessOnCreation = this.dataAccess.CreateFrequentlyAskedQuestion(faq);
                    if (ViewBag.SuccessOnCreation) {
                        ViewBag.Message = "La pregunta " + "\"" + faq.Question + " \" fue creada con éxito";
                        ModelState.Clear();
                    }
                }
                return View("/FrequentlyQuestion/FrequentlyAskQuestions");
            } catch {
                ViewBag.Message = "Algo salió mal y no fue posible crear la pregunta";
                return View("/FrequentlyQuestion/FrequentlyAskQuestions"); 
            }
        }

        public ActionResult FrequentlyAskQuestions() {
            List<string> categories = this.dataAccess.GetAllCategories();
            Dictionary<string, List<FrequentlyQuestionModel>> questionsSortedByCategory = new Dictionary<string, List<FrequentlyQuestionModel>>();
            foreach (string category in categories) {
                questionsSortedByCategory[category] = new List<FrequentlyQuestionModel>();
            }
            this.dataAccess.GetAllQuestions(questionsSortedByCategory);
            ViewBag.QuestionsSortedByCategories = questionsSortedByCategory;
            ViewBag.Categories = categories;
            return View();
        }

    }
}