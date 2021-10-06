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

        private List<SelectListItem> LoadCategories() {
            List<string> categories = DataAccess.GetAllCategories();

            List<SelectListItem> dropdownCategories = new List<SelectListItem>();
            foreach (string category in categories) {
                dropdownCategories.Add(new SelectListItem { Text = category, Value = category });
            }

            return dropdownCategories;
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
            ViewData["category"] = LoadCategories();
            LoadDropDownList();
            return View();
        }

        [HttpPost]
        public ActionResult UploadEducationalActivity(EducationalActivityModel educationalActivity) {
            ActionResult view = RedirectToAction("Success", "Home");
            LoadEducationalActivityWithForm(educationalActivity);
            ViewBag.SuccessOnCreation = false;
            try {
                ViewBag.SuccessOnCreation = this.DataAccess.ProposeEducationalActivity(educationalActivity);
                if (ViewBag.SuccessOnCreation) {
                    SendEmail();
                    ModelState.Clear();
                    view = RedirectToAction("Success", "Home");
                }
            } catch {
                TempData["Error"] = true;
                TempData["WarningMessage"] = "Algo salio mal";
                view = RedirectToAction("ProposeEducationalActivity", "EducationalActivity");
            }

            return view;
        }

        private void LoadEducationalActivityWithForm(EducationalActivityModel educationalActivity) {
            educationalActivity.Duration = int.Parse(Request.Form["Duration"]);
            educationalActivity.TargetAudience = ContentParser.GetTopicsFromString(Request.Form["inputAudienceString"]);
            educationalActivity.Topics = ContentParser.GetTopicsFromString(Request.Form["inputTopicString"]);
            if (educationalActivity.Link == null) {
                educationalActivity.Link = "";
            }
        }

        private void SendEmail() {
            MimeMessage message = new MimeMessage();

            MailboxAddress from = new MailboxAddress("Coordinador","juan.pachecocastro@ucr.ac.cr");
            message.From.Add(from);

            MailboxAddress to = new MailboxAddress("Educador", "davidxieli@gmail.com");
            message.To.Add(to);

            message.Subject = "Su propuesta está en revisión";

            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = "<h1>¡Hola!</h1> <p>Actualmente se encuentra en revisión su propuesta</p>";
            bodyBuilder.TextBody = "¡Hola! Actualmente se encuentra en revisión su propuesta";
            message.Body = bodyBuilder.ToMessageBody();

            SmtpClient client = new SmtpClient();
            client.Connect("smtp.ucr.ac.cr", 587);
            // TODO: cambiar nombre y contraseña
            // Nombre: nombre.apellido
            // Contraseña: del correo ucr
            client.Authenticate("juan.pachecocastro", "password");

            client.Send(message);
            client.Disconnect(true);
            client.Dispose();
        }


    }
}