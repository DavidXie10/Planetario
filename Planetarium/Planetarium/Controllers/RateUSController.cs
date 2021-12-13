using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Models;

namespace Planetarium.Controllers
{
    public class RateUSController : Controller
    {
        public ActionResult SuccessProcessRate()
        {
            return View();
        }

        public ActionResult IndexRate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubmitRate() {
            ActionResult view = RedirectToAction("Index", "Home");
            string starChoice = "";
            string filename = "WebsiteRate.txt";
            if (Request.Cookies["rateStar"] != null) {
                starChoice = Request.Cookies["rateStar"].Value;
            }

            TxtContentParser txtParser = new TxtContentParser();
            string fileContent = txtParser.ExtractRawContent(filename);
            fileContent += starChoice + ",";
            txtParser.WriteToFile(fileContent, filename);

            return view;
        }

        [HttpPost]
        public ActionResult SubmitProcessesRate() {
            ActionResult view = RedirectToAction("Index", "Home");
            string emojiChoice = Request.Form["rateChoice"];
            
            /*
            TxtContentParser txtParser = new TxtContentParser();
            string fileContent = txtParser.ExtractRawContent(filename);
            fileContent += emojiChoice + ",";
            txtParser.WriteToFile(fileContent, filename);
            */
            return view;
        }

        public ActionResult UXEvaluation() {
            ViewBag.Link= "https://docs.google.com/forms/d/e/1FAIpQLScpghh7KECEEjpnpJHqG1l9Zr2a4gcnDCpcKVpN1C2xt1ZMHw/viewform?embedded=true";
            return View();
        }

        public ActionResult RateResults() {
            string filename = "WebsiteRate.txt";
            TxtContentParser txtParser = new TxtContentParser();
            string fileContent = txtParser.ExtractRawContent(filename);
            string[] evaluations = fileContent.Split(',');

            //star survey
            ViewBag.StarTotalResult = CountTotalElements(evaluations);
            SetResultsOfStars(evaluations);
            SetStarPercentages();

            //faces survey
            ViewBag.ExcelentResult = 0;
            ViewBag.VeryGoodResult = 0;
            ViewBag.GoodResult = 0;
            ViewBag.FineResult = 0;
            ViewBag.BadResult = 0;
            ViewBag.ReallyBadResult = 0;
            return View();
        }

        private int CountEvaluation(string value, string[] choices) {
            int counter = 0;
            foreach(string choice in choices) {
                if(choice == value) {
                    ++counter;
                }
            }

            return counter;
        }

        private int CountTotalElements(string[] choices) {
            int counter = 0;
            foreach (string choice in choices) {
                if (choice != "") {
                    ++counter;
                }
            }

            return counter;
        }

        private int GetPercentage(int starCounter, int totalStars) {
            return starCounter * 100 / totalStars;
        }

        private void SetResultsOfStars(string[] evaluations) {
            ViewBag.FiveStarResult = CountEvaluation("5", evaluations);
            ViewBag.FourStarResult = CountEvaluation("4", evaluations);
            ViewBag.ThreeStarResult = CountEvaluation("3", evaluations);
            ViewBag.TwoStarResult = CountEvaluation("2", evaluations);
            ViewBag.OneStarResult = CountEvaluation("1", evaluations);
        }

        private void SetStarPercentages() {
            ViewBag.FiveStarPercentage = GetPercentage(ViewBag.FiveStarResult, ViewBag.StarTotalResult);
            ViewBag.FourStarPercentage = GetPercentage(ViewBag.FourStarResult, ViewBag.StarTotalResult);
            ViewBag.ThreeStarPercentage = GetPercentage(ViewBag.ThreeStarResult, ViewBag.StarTotalResult);
            ViewBag.TwoStarPercentage = GetPercentage(ViewBag.TwoStarResult, ViewBag.StarTotalResult);
            ViewBag.OneStarPercentage = GetPercentage(ViewBag.OneStarResult, ViewBag.StarTotalResult);
        }

    }
}