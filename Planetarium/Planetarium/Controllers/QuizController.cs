using Planetarium.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planetarium.Controllers
{
    public class QuizController : Controller
    {
        ContentParser parser = new ContentParser();
        // GET: Quiz
        public ActionResult Quizzes()
        {
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
    }
}