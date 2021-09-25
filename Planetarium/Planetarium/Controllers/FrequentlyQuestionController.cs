using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Planetarium.Models;
using Planetarium.Handlers;
using System.Web.Mvc;



namespace Planetarium.Controllers {
    public class FrequentlyQuestionController : Controller {
        public ActionResult CreateFrequentlyAskedQuestion() {
            return View();
        }

        [HttpPost]
        public ActionResult CreateFrequentlyAskedQuestion(FrequentlyQuestionModel question) {
            ViewBag.SuccessOnCreation = false;
            try {
                if (ModelState.IsValid) {
                    FrequentlyQuestionHandler dataAccess = new FrequentlyQuestionHandler();
                    ViewBag.SuccessOnCreation = dataAccess.CreateFrequentlyAskedQuestion(question);
                    if (ViewBag.ExitoAlCrear) {
                        ViewBag.Message = "La pregunta " + "\"" + question.Question + " \" fue creada con éxito";
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