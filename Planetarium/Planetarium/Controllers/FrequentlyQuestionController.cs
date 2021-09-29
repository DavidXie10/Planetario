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
        dynamic JsonContent { get; set; }

        public FrequentlyQuestionController() {
            ContentParser contentParser = new ContentParser();
            this.JsonContent = contentParser.ParseFromJSON("CategoriesAndTopics.json");
        }

        
        public ActionResult CreateFrequentlyAskedQuestion() {

            List<string> keysJson = new List<string>();
            List<string[]> valuesJson = new List<string[]>();

            foreach (var keys in this.JsonContent) {
                string keyFromJason = keys.Key;
                keyFromJason = keyFromJason.Replace("_", " ");
                keysJson.Add(keyFromJason);
                valuesJson.Add(keys.Value);
            }

            ViewBag.Categories = keysJson;
            ViewBag.Topics = valuesJson;

            return View();
        }

        public JsonResult GetTopics(string id) {
            List<SelectListItem> subCat = new List<SelectListItem>();

            subCat.Add(new SelectListItem { Text = "Select", Value = "0" });
            string[] cuerposDelSistemaSolar = JsonContent.CuerposDelSistemaSolar.ToObject<string[]>();
            string[] objetosDeCieloProfundo = JsonContent.ObjetosDeCieloProfundo.ToObject<string[]>();
            string[] astronomía = JsonContent.Astronomía.ToObject<string[]>();
            string[] general = JsonContent.General.ToObject<string[]>();
            int value = 1;
            switch (id) {
                case "1":
                    foreach (string topic in cuerposDelSistemaSolar) {
                        subCat.Add(new SelectListItem { Text = topic, Value = value.ToString() });
                        ++value;
                    }
                    break;
                case "2":
                    foreach (string topic in objetosDeCieloProfundo) {
                        subCat.Add(new SelectListItem { Text = topic, Value = value.ToString() });
                        ++value;
                    }
                    break;
                case "3":
                    foreach (string topic in astronomía) {
                        subCat.Add(new SelectListItem { Text = topic, Value = value.ToString() });
                        ++value;
                    }
                    break;
                case "4":
                    foreach (string topic in general) {
                        subCat.Add(new SelectListItem { Text = topic, Value = value.ToString() });
                        ++value;
                    }
                    break;
                default:
                    break;
            }

            return Json(new SelectList(subCat, "Value", "Text"));
        }

        [HttpPost]
        public ActionResult CreateFrequentlyAskedQuestion(FrequentlyQuestionModel faqQuestion) {

            FrequentlyQuestionModel faq = new FrequentlyQuestionModel();
            faq.Category = Request.Form["category"].Replace(" ", "_");
            faq.Topic = Request.Form["topic"];
            faq.Question = Request.Form["question"];
            faq.Answer = Request.Form["Answer"];

            ViewBag.SuccessOnCreation = false;
            try {
                if (ModelState.IsValid) {
                    FrequentlyQuestionHandler dataAccess = new FrequentlyQuestionHandler();
                    ViewBag.SuccessOnCreation = dataAccess.CreateFrequentlyAskedQuestion(faq);
                    if (ViewBag.SuccessOnCreation) {
                        ViewBag.Message = "La pregunta " + "\"" + faq.Question + " \" fue creada con éxito";
                        ModelState.Clear();
                    }
                }
                return View("/Home/Index");
            } catch {
                ViewBag.Message = "Algo salió mal y no fue posible crear la pregunta";
                return View("/Home/Index"); 
            }
        }
    }
}