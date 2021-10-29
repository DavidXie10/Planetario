using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Handlers;
using Planetarium.Models;
using System.IO;
using MailKit.Net.Smtp;
using MimeKit;

namespace Planetarium.Controllers {
    public class EducationalActivityController : Controller {
        public EducationalActivityHandler DataAccess { get; set; }
        public ContentParser ContentParser { get; set; }

        public EducationalActivityController() {
            DataAccess = new EducationalActivityHandler();
            ContentParser = new ContentParser();
        }

        public JsonResult GetTopicsList(string category) {
            List<SelectListItem> topicsList = new List<SelectListItem>();

            List<string> topicsFromCategory = DataAccess.GetTopicsByCategory(category);

            foreach (string topic in topicsFromCategory) {
                topicsList.Add(new SelectListItem { Text = topic, Value = topic });
            }

            return Json(new SelectList(topicsList, "Value", "Text"));
        }

        private List<SelectListItem> GetDropdown(string[] options) {
            List<SelectListItem> dropdown = new List<SelectListItem>();

            foreach (string option in options) {
                dropdown.Add(new SelectListItem { Text = option, Value = option });
            }
            return dropdown;
        }

        private void LoadDropDownList() {
            dynamic jsonContent = ContentParser.ParseFromJSON("EducationalActivity.json");

            string[] activitiesType = jsonContent.TipoActividad.ToObject<string[]>();
            string[] complexityLevels = jsonContent.NivelComplejidad.ToObject<string[]>();
            string[] assistances = jsonContent.Asistencia.ToObject<string[]>();
            string[] targetAudiences = jsonContent.PublicoMeta.ToObject<string[]>();

            ViewBag.ActivitiesTypes = GetDropdown(activitiesType);
            ViewBag.ComplexityLevels = GetDropdown(complexityLevels);
            ViewBag.Assistances = GetDropdown(assistances);
            ViewBag.TargetAudiences = GetDropdown(targetAudiences);
        }

        public ActionResult ProposeEducationalActivity() {
            ViewData["category"] = GetDropdown(DataAccess.GetAllCategories().ToArray());
            LoadDropDownList();
            return View();
        }

        [HttpPost]
        public ActionResult UploadEducationalActivity(EducationalActivityEventModel educationalActivity) {
            ActionResult view = RedirectToAction("Success", "Home");
            LoadEducationalActivityWithForm(educationalActivity);
            ViewBag.SuccessOnCreation = false;
            try {
                ViewBag.SuccessOnCreation = this.DataAccess.ProposeEducationalActivity(educationalActivity);
                if (ViewBag.SuccessOnCreation) {
                    SendEmail(0);
                    ModelState.Clear();
                    view = RedirectToAction("Success", "Home");
                }
            } catch {
                TempData["Error"] = true;
                TempData["WarningMessage"] = "Algo salió mal";
                view = RedirectToAction("ProposeEducationalActivity", "EducationalActivity");
            }

            return view;
        }

        private void LoadEducationalActivityWithForm(EducationalActivityEventModel educationalActivity) {
            educationalActivity.Duration = int.Parse(Request.Form["Duration"]);
            educationalActivity.TargetAudience = ContentParser.GetListFromString(Request.Form["inputAudienceString"]);
            educationalActivity.Topics = ContentParser.GetListFromString(Request.Form["inputTopicString"]);
            if (educationalActivity.Link == null) {
                educationalActivity.Link = "";
            }
        }

        private void SendEmail(int state) {
            const string BASE_MESSAGE_HTML = "<h1>¡Hola!</h1> <p>Su propuesta ha sido ";
            const string BASE_MESSAGE_TEXT = "¡Hola! Su propuesta ha sido ";
            const string BASE_SUBJECT = "Estado de la propuesta";
            MimeMessage message = new MimeMessage();

            MailboxAddress from = new MailboxAddress("Coordinador", "mauricio.rojassegnini@ucr.ac.cr");
            message.From.Add(from);
            MailboxAddress to = new MailboxAddress("Educador", "carlos.espinozaperaza@ucr.ac.cr");
            message.To.Add(to);

            message.Subject = BASE_SUBJECT;

            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = WordUsageDependingOnState(BASE_MESSAGE_HTML, state);
            bodyBuilder.TextBody = WordUsageDependingOnState(BASE_MESSAGE_TEXT, state);
            bodyBuilder.HtmlBody += "</p>";

            message.Body = bodyBuilder.ToMessageBody();

            SmtpClient client = new SmtpClient();
            client.Connect("smtp.ucr.ac.cr", 587);
            client.Authenticate("mauricio.rojassegnini@ucr.ac.cr", "mauuam1771.");

            client.Send(message);
            client.Disconnect(true);
            client.Dispose();
        }

        private string WordUsageDependingOnState(string baseMessage, int state) {
            return baseMessage +( (state == 0) ? "pasada a revisión." : (state == 1) ? "aprobada." : "rechazada.");    
        }

        public ActionResult ListActivities() {
            ViewBag.activities = DataAccess.GetAllApprovedActivities();
            return View();
        }

        public ActionResult ActivitiesApprobation() {
            ViewBag.activities = DataAccess.GetAllOnRevisionActivities();
            return View();
        }

        [HttpPost]
        public ActionResult SubmitApprobation() {
            ActionResult view = RedirectToAction("ActivitiesApprobation", "EducationalActivity");
            int state = Convert.ToInt32(Request.Form["status"]);
            string title = Request.Form["myTitle"];

            ViewBag.SuccessOnCreation = false;
            try {   
                ViewBag.SuccessOnCreation = DataAccess.UpdateActivityState(title, state);
                if (ViewBag.SuccessOnCreation) {
                    ModelState.Clear();
                    SendEmail(state);
                }
            } catch {
                TempData["Error"] = true;
                TempData["WarningMessage"] = "Algo salio mal";
            }
            return view;
        }

        private List<SelectListItem> loadLanguages() {
            dynamic JsonContentCountries = ContentParser.ParseFromJSON("countries.json");
            string[] countriesFromJson = JsonContentCountries.CountrieNames.ToObject<string[]>();

            List<SelectListItem> countries = new List<SelectListItem>();
            foreach (string country in countriesFromJson) {
                countries.Add(new SelectListItem { Text = country, Value = country });
            }

            return countries;
        }

        private List<SelectListItem> loadEducationalLevels() {
            dynamic JsonContentCountries = ContentParser.ParseFromJSON("EducationalActivity.json");
            string[] educationalLevelsFromJson = JsonContentCountries.NivelEducativo.ToObject<string[]>();

            List<SelectListItem> educationalLevels = new List<SelectListItem>();
            foreach (string educationalLevel in educationalLevelsFromJson) {
                educationalLevels.Add(new SelectListItem { Text = educationalLevel, Value = educationalLevel });
            }

            return educationalLevels;
        }

        public ActionResult ActivityInscription(string activityTitle, string activityDate) {
            ViewBag.Countries = loadLanguages();
            //ViewData["activity"] = activity;
            ViewBag.ActivityTitle = activityTitle;
            ViewBag.ActivityDate = activityDate;
            ViewBag.EducationalLevels = loadEducationalLevels();

            return View();
        }

        [HttpPost]
        public ActionResult SubmitActivityInscription(VisitorModel visitor) {
            ActionResult view = RedirectToAction("ActivityInscription", "EducationalActivity");
            visitor.Gender = Request.Form["gender"].ElementAt(0);
            string date = Request.Form["date"];
            string title = Request.Form["title"];
            ViewBag.SuccessOnCreation = false;
            ViewBag.Message = "Algo salió mal";

            try {
                ViewBag.SuccessOnCreation = DataAccess.RegisterVisitor(visitor, title, date);
                if (ViewBag.SuccessOnCreation) {
                    ModelState.Clear();
                    ViewBag.Message = "Inscripción exitosa";
                    view = RedirectToAction("Success", "Home");
                } 
            } catch {
                ViewBag.Message = "Algo salió mal";
            }

            return view;
        }



    }
}