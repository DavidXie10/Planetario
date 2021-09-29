using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Planetarium.Models;
using Planetarium.Handlers;
using System.Web.Mvc;

namespace Planetarium.Controllers {
    public class FrequentlyQuestionController : Controller {
        dynamic JsonContent { get; set; }

        public FrequentlyQuestionController() {
            ContentParser contentParser = new ContentParser();
            this.JsonContent = contentParser.ParseFromJSON("CategoriesAndTopics.json");
        }

        
        public ActionResult CreateFrequentlyAskedQuestion() {
            string[] categories = JsonContent.Categories.ToObject<string[]>();
            string[] cuerposDelSistemaSolar = JsonContent.CuerposDelSistemaSolar.ToObject<string[]>();
            string[] objetosDeCieloProfundo = JsonContent.ObjetosDeCieloProfundo.ToObject<string[]>();
            string[] astronomía = JsonContent.Astronomía.ToObject<string[]>();
            string[] general = JsonContent.General.ToObject<string[]>();
            
            ViewBag.Categories = categories;
            ViewBag.CuerposDelSistemaSolar = cuerposDelSistemaSolar;
            ViewBag.ObjetosDeCieloProfundo = objetosDeCieloProfundo;
            ViewBag.Astronomía = astronomía;
            ViewBag.General = general;
            
            List<SelectListItem> CategoryTypes = new List<SelectListItem>();
            CategoryTypes.Add(new SelectListItem { Text = "Select Category", Value = "0", Selected = true });
            int value = 1;

            foreach (string categorie in categories) {
                CategoryTypes.Add(new SelectListItem { Text = categorie, Value = value.ToString() });
                ++value;
            }

            ViewBag.Category = CategoryTypes;

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
            string[] categories = JsonContent.Categories.ToObject<string[]>();
            string[] cuerposDelSistemaSolar = JsonContent.CuerposDelSistemaSolar.ToObject<string[]>();
            string[] objetosDeCieloProfundo = JsonContent.ObjetosDeCieloProfundo.ToObject<string[]>();
            string[] astronomía = JsonContent.Astronomía.ToObject<string[]>();
            string[] general = JsonContent.General.ToObject<string[]>();

            ViewBag.Categories = categories;
            ViewBag.CuerposDelSistemaSolar = cuerposDelSistemaSolar;
            ViewBag.ObjetosDeCieloProfundo = objetosDeCieloProfundo;
            ViewBag.Astronomía = astronomía;
            ViewBag.General = general;

            List<SelectListItem> CategoryTypes = new List<SelectListItem>();
            CategoryTypes.Add(new SelectListItem { Text = "Select Category", Value = "0", Selected = true });
            int value = 1;

            foreach (string categorie in categories) {
                CategoryTypes.Add(new SelectListItem { Text = categorie, Value = value.ToString() });
                ++value;
            }

            ViewBag.Category = CategoryTypes;
            ViewBag.SuccessOnCreation = false;
            try {
                if (ModelState.IsValid) {
                    FrequentlyQuestionHandler dataAccess = new FrequentlyQuestionHandler();
                    ViewBag.SuccessOnCreation = dataAccess.CreateFrequentlyAskedQuestion(faqQuestion);
                    if (ViewBag.SuccessOnCreation) {
                        ViewBag.Message = "La pregunta " + "\"" + faqQuestion.Question + " \" fue creada con éxito";
                        ModelState.Clear();
                    }
                }
                return View();
            } catch {
                ViewBag.Message = "Algo salió mal y no fue posible crear la pregunta";
                return View(); 
            }
        }
    }
}