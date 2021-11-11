using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Planetarium.Handlers;
using Planetarium.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace Planetarium.Controllers {
    public class EducationalActivityController : Controller {
        public EducationalActivityHandler ActivityDataAccess { get; set; }
        public VisitorHandler VisitorDataAccess { get; set; }
        public ContentParser ContentParser { get; set; }

        private const int DEFAULT = 0;
        private const int NOT_REGISTERED = 1;
        private const int REGISTERED = 2;

        public EducationalActivityController() {
            ActivityDataAccess = new EducationalActivityHandler();
            VisitorDataAccess = new VisitorHandler();
            ContentParser = new ContentParser();
        }

        public JsonResult GetTopicsList(string category) {
            List<SelectListItem> topicsList = new List<SelectListItem>();

            List<string> topicsFromCategory = ActivityDataAccess.GetTopicsByCategory(category);

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
            ViewData["category"] = GetDropdown(ActivityDataAccess.GetAllCategories().ToArray());
            LoadDropDownList();
            return View();
        }

        [HttpPost]
        public ActionResult UploadEducationalActivity(EducationalActivityEventModel educationalActivity) {
            ActionResult view = RedirectToAction("Success", "Home");
            LoadEducationalActivityWithForm(educationalActivity);
            ViewBag.SuccessOnCreation = false;
            try {
                ViewBag.SuccessOnCreation = this.ActivityDataAccess.ProposeEducationalActivity(educationalActivity);
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
            ViewBag.activities = ActivityDataAccess.GetAllApprovedActivities();
            return View();
        }

        public ActionResult ActivitiesApprobation() {
            ViewBag.activities = ActivityDataAccess.GetAllOnRevisionActivities();
            return View();
        }

        [HttpPost]
        public ActionResult SubmitApprobation() {
            ActionResult view = RedirectToAction("ActivitiesApprobation", "EducationalActivity");
            int state = Convert.ToInt32(Request.Form["status"]);
            string title = Request.Form["myTitle"];

            ViewBag.SuccessOnCreation = false;
            try {   
                ViewBag.SuccessOnCreation = ActivityDataAccess.UpdateActivityState(title, state);
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

        private List<SelectListItem> LoadLanguages() {
            dynamic JsonContentCountries = ContentParser.ParseFromJSON("countries.json");
            string[] countriesFromJson = JsonContentCountries.CountrieNames.ToObject<string[]>();

            List<SelectListItem> countries = new List<SelectListItem>();
            foreach (string country in countriesFromJson) {
                countries.Add(new SelectListItem { Text = country, Value = country });
            }

            return countries;
        }

        private List<SelectListItem> LoadEducationalLevels() {
            dynamic JsonContentCountries = ContentParser.ParseFromJSON("EducationalActivity.json");
            string[] educationalLevelsFromJson = JsonContentCountries.NivelEducativo.ToObject<string[]>();

            List<SelectListItem> educationalLevels = new List<SelectListItem>();
            foreach (string educationalLevel in educationalLevelsFromJson) {
                educationalLevels.Add(new SelectListItem { Text = educationalLevel, Value = educationalLevel });
            }

            return educationalLevels;
        }

        public ActionResult ActivityInscription(string activityTitle, string activityDate, int register = DEFAULT) {
            Dictionary<int, string> errorMessages = new Dictionary<int, string>() {
                {DEFAULT, "" } , {NOT_REGISTERED, "Cédula no registrada. Por favor, intente de nuevo o regístrese"}, {REGISTERED, "Ya está registrado en la actividad" }
            };
            ViewBag.ActivityTitle = activityTitle;
            ViewBag.ActivityDate = activityDate;
            ViewBag.Register = register;
            ViewBag.ErrorMessages = errorMessages;

            return View();
        }

        [HttpPost]
        public ActionResult SubmitActivityInscription(VisitorModel visitor) {
            string date = Request.Form["date"];
            string title = Request.Form["title"];
            ActionResult view = RedirectToAction("ActivityInscription", "EducationalActivity", new { activityTitle = title, activityDate = date});
          
            ViewBag.SuccessOnCreation = false;
            TempData["Error"] = true;
            TempData["WarningMessage"] = "";

            try {
                int register = VisitorDataAccess.CheckVisitor(visitor.Dni) ? (VisitorDataAccess.CheckVisitor(visitor.Dni, title, date) ? REGISTERED : DEFAULT) : NOT_REGISTERED;
                 
                if (register == DEFAULT) {
                    ViewBag.SuccessOnCreation = VisitorDataAccess.InsertVisitor(visitor, title, date);
                    if (ViewBag.SuccessOnCreation) {
                        ModelState.Clear();
                        TempData["Error"] = false;
                        view = RedirectToAction("Success", "Home");
                    }    
                } else {
                    TempData["Error"] = false;
                    view = RedirectToAction("ActivityInscription", "EducationalActivity", new { activityTitle = title, activityDate = date, register = register });
                }
            } catch {
                TempData["WarningMessage"] = "Algo salió mal";
            }

            return view;
        }

        private List<SelectListItem> LoadGenders() {
            List<SelectListItem> genders = new List<SelectListItem>();
            genders.Add(new SelectListItem { Text = "Hombre", Value = "M" });
            genders.Add(new SelectListItem { Text = "Mujer", Value = "F" });
            genders.Add(new SelectListItem { Text = "Prefiero no decir", Value = "O" });
            return genders;
        }

        public ActionResult ActivityInscriptionForm(string activityTitle, string activityDate) {
            ViewBag.Countries = LoadLanguages();
            ViewBag.ActivityTitle = activityTitle;
            ViewBag.ActivityDate = activityDate;
            ViewBag.EducationalLevels = LoadEducationalLevels();
            ViewBag.GenderOptions = LoadGenders();

            return View();
        }

        [HttpPost]
        public ActionResult SubmitActivityInscriptionForm(VisitorModel visitor) {
            ActionResult view = RedirectToAction("ActivityInscription", "EducationalActivity");
            string date = Request.Form["date"];
            string title = Request.Form["title"];
            ViewBag.SuccessOnCreation = false;
            TempData["Error"] = true;
            TempData["WarningMessage"] = "";

            try {
                ViewBag.SuccessOnCreation = VisitorDataAccess.RegisterVisitor(visitor, title, date);
                if (ViewBag.SuccessOnCreation) {
                    TempData["Error"] = false;
                    ModelState.Clear();
                    view = RedirectToAction("Success", "Home");
                }
            } catch (Exception e) {
                TempData["WarningMessage"] = e;
            }

            return view;
        }

        public ActionResult EducationalActivity(string activityTitle, List<string> activityTopics) {
            ActionResult view;
            try {
                EducationalActivityEventModel educationalActivity = ActivityDataAccess.GetAllApprovedActivities().Find(smodel => String.Equals(smodel.Title, activityTitle));
                List<EducationalActivityEventModel> similarActivities = ActivityDataAccess.GetAllSimilarActivities(activityTitle, activityTopics);
                if (educationalActivity == null) {
                    view = RedirectToAction("ListActivities");
                } else {
                    ViewBag.Activity = educationalActivity;
                    ViewBag.SimilarActivities = similarActivities;
                    view = View(educationalActivity);
                }
            } catch {
                view = RedirectToAction("ListActivities");
            }
            return view;
        }
    }
}