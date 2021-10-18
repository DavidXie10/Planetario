using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Handlers;
using Planetarium.Models;


namespace Planetarium.Controllers {
    
    public class EducationalMaterialController : Controller {
        public EducationalMaterialHandler DataAccess { get; set; }
        public ContentParser ContentParser { get; set; }

        public EducationalMaterialController() {
            DataAccess = new EducationalMaterialHandler();
            ContentParser = new ContentParser();
        }

        public ActionResult ListEducationalMaterial() {
            ViewBag.EducationalMaterials = DataAccess.GetAllEducationalMaterial();
            return View();
        }

        private List<SelectListItem> LoadCategories() {
            List<string> categories = DataAccess.GetAllCategories();

            List<SelectListItem> dropdownCategories = new List<SelectListItem>();
            foreach (string category in categories) {
                dropdownCategories.Add(new SelectListItem { Text = category, Value = category });
            }

            return dropdownCategories;
        }

        public ActionResult SubmitEducationalMaterial() {
            ViewData["category"] = LoadCategories();
            ViewBag.EducationalMaterials = DataAccess.GetAllEducationalMaterial();
            ViewBag.DropDownActivitiesNames = LoadActivityNames();
            return View();
        }

        [HttpPost]
        public ActionResult SendEducationalMaterialForm(EducationalMaterialModel educationalMaterial) {
            ActionResult view = RedirectToAction("Success", "Home");
            educationalMaterial.EducationalMaterialFileNames = ContentParser.GetListFromString(Request.Form["filesString"]);
            for (int i = 0; i < educationalMaterial.EducationalMaterialFileNames.Count; i++) {
                educationalMaterial.EducationalMaterialFileNames[i] = educationalMaterial.EducationalMaterialFileNames[i].Replace(" ", "-").Replace(" ", "-");
            }
            educationalMaterial.Category = Request.Form["Category"].Replace(" ", "_");
            educationalMaterial.Topics = ContentParser.GetListFromString(Request.Form["inputTopicString"]);
            ViewBag.SuccessOnCreation = false;
            try {
                ViewBag.SuccessOnCreation = this.DataAccess.InsertEducationalMaterial(educationalMaterial);
                if (ViewBag.SuccessOnCreation) {
                    ModelState.Clear();
                    view = RedirectToAction("Success", "Home");
                }
            } catch {
                TempData["Error"] = true;
                TempData["WarningMessage"] = "Algo salio mal";
                view = RedirectToAction("SubmitEducationalMaterial", "EducationalMaterial");
            }

            return view;
        }

        public JsonResult GetTopicsList(string category){
            List<SelectListItem> topicsList = new List<SelectListItem>();

            List<string> topicsFromCategory = DataAccess.GetTopicsByCategory(category);

            foreach (string topic in topicsFromCategory) {
                topicsList.Add(new SelectListItem { Text = topic, Value = topic });
            }

            return Json(new SelectList(topicsList, "Value", "Text"));
        }

        private List<SelectListItem> LoadActivityNames() {
            List<string> activitiesNames = DataAccess.GetAllActivities();

            List<SelectListItem> dropdownActivities = new List<SelectListItem>();
            foreach (string activity in activitiesNames) {
                dropdownActivities.Add(new SelectListItem { Text = activity, Value = activity });
            }

            return dropdownActivities;
        }

        [HttpPost]
        public ActionResult UploadFiles(IEnumerable<HttpPostedFileBase> files) {
            foreach (var file in files) {
                string filePath = file.FileName.Replace("_", "-").Replace(" ", "-");
                file.SaveAs(Path.Combine(Server.MapPath("~/Educational_Material/"), filePath));
            }

            return Json("Files uploaded successfully");
        }
    }
}