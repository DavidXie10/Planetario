using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Handlers;
using Planetarium.Models;
using System.IO;

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
                //IsValid?
                ViewBag.SuccessOnCreation = this.DataAccess.ProposeEducationalActivity(educationalActivity);
                if (ViewBag.SuccessOnCreation) {
                    ModelState.Clear();
                    view = RedirectToAction("Success", "Home");
                }
            } catch (Exception e) {
                TempData["Error"] = true;
                TempData["WarningMessage"] = e.ToString();
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
    }
}