using Planetarium.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Planetarium.Controllers{
    public class QuizController : Controller{
        ContentParser parser = new ContentParser();

        public ActionResult Quizzes(){
            List<QuizModel> quizzes = parser.GetQuizzes("Quizzes.json");
            ViewBag.Quizzes = quizzes;
            return View();
        }

        public ActionResult Quiz(string url) {
            ViewBag.Link = url;
            return View();
        }

        public ActionResult NewQuiz() {
            return View();
        }

        [HttpPost]
        public ActionResult SubmitQuiz(QuizModel quiz) {
            ActionResult view = RedirectToAction("NewQuiz", "Quiz");
            quiz.Difficulty = Request.Form["dificulty"];
            ViewBag.SuccessOnCreation = false;
            try {
                ViewBag.SuccessOnCreation = parser.WriteToJsonFile("Quizzes.json", quiz);
                if (ViewBag.SuccessOnCreation) {
                    view = RedirectToAction("Success", "Home");
                    ViewBag.Message = "El cuestionario fue agregado con éxito";
                    ModelState.Clear();
                }
            } catch {
                ViewBag.Message = "Algo salió mal y no fue posible crear el cuestionario";
            }
            return view;
        }

    }
}