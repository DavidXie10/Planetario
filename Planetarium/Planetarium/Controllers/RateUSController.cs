using System.Web.Mvc;
using Planetarium.Models;

namespace Planetarium.Controllers
{
    public class RateUSController : Controller{
        public ActionResult SuccessProcessRate() {
            return View("SuccessProcessRate");
        }

        public ActionResult IndexRate() {
            return View("IndexRate");
        }

        [HttpPost]
        public ActionResult SubmitRate() {
            ActionResult view = RedirectToAction("Index", "Home");
            string starChoice = "";

            if (Request.Cookies["rateStar"] != null) {
                starChoice = Request.Cookies["rateStar"].Value;
            } else {
                starChoice = "5";
            }

            string filename = "WebsiteRate.txt";
            UpdateRating(starChoice, filename);
            return view;
        }

        [HttpPost]
        public ActionResult SubmitProcessesRate() {
            ActionResult view = RedirectToAction("Index", "Home");
            string emojiChoice = Request.Form["rateProcessChoice"];
            string filename = "Processes.txt";
            UpdateRating(emojiChoice, filename);
            
            return view;
        }

        private void UpdateRating(string content, string filename) {
            TxtContentParser txtParser = new TxtContentParser();
            string fileContent = txtParser.ExtractRawContent(filename);
            fileContent += content + ",";
            txtParser.WriteToFile(fileContent, filename);
        }

        public ActionResult RateResults() {
            SetStarsChartData();
            SetProcessesChartData();

            return View();
        }

        private void SetStarsChartData() {
            string[] evaluations = GetEvaluations("WebsiteRate.txt");
            ViewBag.StarTotalResult = CountTotalElements(evaluations);
            SetResultsOfStars(evaluations);
            SetStarPercentages();
        }

        private void SetProcessesChartData() {
            string[] evaluations = GetEvaluations("Processes.txt");
            ViewBag.ProcessesTotalResult = CountTotalElements(evaluations);
            SetResultsOfProcesses(evaluations);
            SetProcessesPercentages();
        }

        private string[] GetEvaluations(string filename) {
            TxtContentParser txtParser = new TxtContentParser();
            string fileContent = txtParser.ExtractRawContent(filename);
            string[] evaluations = fileContent.Split(',');
            return evaluations;
        }

        private int CountEvaluation(string value, string[] choices) {
            int counter = 0;
            foreach (string choice in choices) {
                if (choice == value) {
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

        private void SetResultsOfProcesses(string[] evaluations) {
            ViewBag.ExcelentResult = CountEvaluation("5", evaluations);
            ViewBag.GoodResult = CountEvaluation("4", evaluations);
            ViewBag.FineResult = CountEvaluation("3", evaluations);
            ViewBag.BadResult = CountEvaluation("2", evaluations);
            ViewBag.ReallyBadResult = CountEvaluation("1", evaluations);
        }

        private void SetStarPercentages() {
            ViewBag.FiveStarPercentage = GetPercentage(ViewBag.FiveStarResult, ViewBag.StarTotalResult);
            ViewBag.FourStarPercentage = GetPercentage(ViewBag.FourStarResult, ViewBag.StarTotalResult);
            ViewBag.ThreeStarPercentage = GetPercentage(ViewBag.ThreeStarResult, ViewBag.StarTotalResult);
            ViewBag.TwoStarPercentage = GetPercentage(ViewBag.TwoStarResult, ViewBag.StarTotalResult);
            ViewBag.OneStarPercentage = GetPercentage(ViewBag.OneStarResult, ViewBag.StarTotalResult);
        }

        private void SetProcessesPercentages() {
            ViewBag.ExcelentResultPercentage = GetPercentage(ViewBag.ExcelentResult, ViewBag.StarTotalResult);
            ViewBag.GoodResultPercentage = GetPercentage(ViewBag.GoodResult, ViewBag.StarTotalResult);
            ViewBag.FineResultPercentage = GetPercentage(ViewBag.FineResult, ViewBag.StarTotalResult);
            ViewBag.BadResultPercentage = GetPercentage(ViewBag.BadResult, ViewBag.StarTotalResult);
            ViewBag.ReallyBadResultPercentage = GetPercentage(ViewBag.ReallyBadResult, ViewBag.StarTotalResult);
        }

        public ActionResult UXEvaluation() {
            ViewBag.Link= "https://docs.google.com/forms/d/e/1FAIpQLScpghh7KECEEjpnpJHqG1l9Zr2a4gcnDCpcKVpN1C2xt1ZMHw/viewform?embedded=true";
            return View("UXEvaluation");
        }
    }
}