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
        public EducationalActivityHandler DataAccess { get; set; }
        public ContentParser ContentParser { get; set; }

        public EducationalMaterialController() {
            DataAccess = new EducationalActivityHandler();
            ContentParser = new ContentParser();
        }

        public ActionResult SubmitEducationalMaterial() {
            ViewBag.DropDownActivitiesNames = LoadActivityNames();
            return View();
        }

        
        [HttpPost]
        public ActionResult SendEducationalMaterialForm(EducationalActivityModel educationalActivity) {
            ActionResult view = RedirectToAction("Success", "Home");
            educationalActivity.RefEducationalMaterial = ContentParser.GetListFromString(Request.Form["filesString"]);
            for (int i = 0; i < educationalActivity.RefEducationalMaterial.Count; i++) {
                educationalActivity.RefEducationalMaterial[i] = educationalActivity.RefEducationalMaterial[i].Replace(" ", "-").Replace(" ", "-");
            }
            ViewBag.SuccessOnCreation = false;
            try {
                ViewBag.SuccessOnCreation = this.DataAccess.InsertEducationalMaterial(educationalActivity);
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