﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Planetarium.Handlers;
using Planetarium.Models;
using System.Linq;

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
                    ModelState.Clear();
                    view = RedirectToAction("Success", "Home");
                }
            } catch (Exception e) {
                TempData["Error"] = true;
                TempData["WarningMessage"] = "Algo salió mal";
                string noEducatorId = "transaction ended in the trigger";
                if (e.ToString().Contains(noEducatorId)) {
                    TempData["WarningMessage"] = "No tiene el permiso de educador para agregar actividad";
                }
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

        public ActionResult ListActivities() {
            List<CouponModel> coupons = ContentParser.GetContentsFromJson<CouponModel>("Coupons.json", ContentParser.GetCouponsFromJson);
            ViewBag.Coupons = coupons;

            RssFeedHandler rssHandler = new RssFeedHandler();
            List<EventModel> eventFeed = rssHandler.GetEventsFromFeed("https://www.timeanddate.com/astronomy/sights-to-see.html");
            ViewBag.EventsToCal = eventFeed;
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
                }
            } catch {
                TempData["Error"] = true;
                TempData["WarningMessage"] = "Algo salio mal";
            }
            return view;
        }

        private List<SelectListItem> LoadCountries() {
            dynamic JsonContentCountries = ContentParser.ParseFromJSON("countries.json");
            string[] countriesFromJson = JsonContentCountries.CountrieNames.ToObject<string[]>();

            List<SelectListItem> countries = new List<SelectListItem>();
            foreach (string country in countriesFromJson) {
                countries.Add(new SelectListItem { Text = country, Value = country });
            }

            return countries;
        }

        private List<SelectListItem> LoadEducationalLevels() {
            dynamic JsonContentEducationalLevels = ContentParser.ParseFromJSON("EducationalActivity.json");
            string[] educationalLevelsFromJson = JsonContentEducationalLevels.NivelEducativo.ToObject<string[]>();

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
        public ActionResult ConfirmSeat() {
            ActionResult view = null;
            string id = Request.Form["dni"];
            string title = Request.Form["title"];
            string date = Request.Form["date"];
            string seat = Request.Form["selectedSeatString"];
            double price = ActivityDataAccess.GetPrice(title, date);

            if (price > 0) {
                view = RedirectToAction("PayMethod", "EducationalActivity", new { dni = id, title = title, date = date, seat = seat });
            }else {
                view = RedirectToAction("Invoice", "Educationalactivity", new { dni = id, title = title, date = date, seat = seat, price = price });
            }
            return view;
        }

        public ActionResult PayMethod(string dni = "0", string title = "", string date = "", string seat = "0-0") {
            List<CouponModel> coupons = ContentParser.GetContentsFromJson<CouponModel>("Coupons.json", ContentParser.GetCouponsFromJson);
            ViewBag.Coupons = coupons;

            ViewBag.visitor = VisitorDataAccess.GetVisitorByDni(dni, true);

            ViewBag.title = title;
            ViewBag.date = date;
            ViewBag.seat = seat;
            ViewBag.Price = ActivityDataAccess.GetPrice(ViewBag.title, ViewBag.date);

            return View();
        }

        public ActionResult Invoice(string dni, string title, string date, string seat, double price) {
            VisitorModel visitor = VisitorDataAccess.GetVisitorByDni(dni, true);
            ViewBag.Visitor = visitor;
            ViewBag.Title = title;
            ViewBag.Date = date;
            ViewBag.Seat = seat;
            ViewBag.Price = price;
            
            ActionResult view = RedirectToAction("PayMethod", "EducationalActivity", new { dni = visitor.Dni, title = title, date = date, seat = seat });
            try {
                ViewBag.SuccessOnCreation = VisitorDataAccess.InsertVisitor(visitor.Dni, title, date);
                ViewBag.SuccessOnCreation = VisitorDataAccess.InsertAssignSeat(visitor.Dni, title, date, seat);
                if (ViewBag.SuccessOnCreation) {
                    view = View();
                }
            } catch {
                TempData["WarningMessage"] = "Algo salió mal";
            }

            return view;
        }

        public JsonResult GetOcuppiedSeats(string title, string date) {
            List<string> occupiedSeats = ActivityDataAccess.GetReservedSeats(title, date);
            return Json(occupiedSeats, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AssignSeat(string id = "0", string title = "X", string date = "X") {
            ViewBag.maxParticipants = ActivityDataAccess.GetMaxCapacity(title, date);
            ViewBag.id = id;
            ViewBag.title = title;
            ViewBag.date = date;

            return View();
        }


        [HttpPost]
        public ActionResult VerifyVisitorIdentity(VisitorModel visitor) {
            string date = Request.Form["date"];
            string title = Request.Form["title"];
            ActionResult view = RedirectToAction("ActivityInscription", "EducationalActivity", new { activityTitle = title, activityDate = date });

            ViewBag.SuccessOnCreation = false;
            TempData["Error"] = true;
            TempData["WarningMessage"] = "";

            try {
                int register = VisitorDataAccess.CheckVisitor(visitor.Dni) ? (VisitorDataAccess.CheckVisitor(visitor.Dni, title, date) ? REGISTERED : DEFAULT) : NOT_REGISTERED;

                if (register == DEFAULT) {
                    ViewBag.SuccessOnCreation = ActivityDataAccess.CheckCapacity(title, date);
                    if (ViewBag.SuccessOnCreation) {
                        ModelState.Clear();
                        TempData["Error"] = false;
                        view = RedirectToAction("AssignSeat", "EducationalActivity", new { id = visitor.Dni, title = title, date = date });
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
            ViewBag.Countries = LoadCountries();
            ViewBag.EducationalLevels = LoadEducationalLevels();
            ViewBag.GenderOptions = LoadGenders();
            ViewBag.ActivityTitle = activityTitle;
            ViewBag.ActivityDate = activityDate;

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
                ViewBag.SuccessOnCreation = ActivityDataAccess.CheckCapacity(title, date);
                if (ViewBag.SuccessOnCreation) {
                    TempData["Error"] = false;
                    ModelState.Clear();
                    view = RedirectToAction("AssignSeat", "EducationalActivity", new { id = visitor.Dni, title = title, date = date });
                }
            } catch {
                TempData["WarningMessage"] = "Algo salió mal";
            }

            return view;
        }

        public ActionResult EducationalActivity(string tempActivity) {
            ActionResult view;
            try {
                EducationalActivityEventModel activity = System.Web.Helpers.Json.Decode<EducationalActivityEventModel>(tempActivity);
                EducationalActivityEventModel educationalActivity = ActivityDataAccess.GetAllApprovedActivities().Find(smodel => String.Equals(smodel.Title, activity.Title));
                List<EducationalActivityEventModel> similarActivities = ActivityDataAccess.GetAllSimilarActivities(activity.Title, activity.Topics, activity.Category);
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
        public ActionResult ShowStatisticsInvolvement() {            
            Dictionary<string, int> categoriesRank = ActivityDataAccess.FillRank("categoria","Categoria");
            Dictionary<string, int> topicsRank = ActivityDataAccess.FillRank("nombrePK", "Topico");

            List<string> categories = categoriesRank.Keys.ToList<string>();
            Dictionary<string, string[]> topicsByCategory = new Dictionary<string, string[]>();
            string[] topics = {};
            foreach (string category in categories) {
                topics = ActivityDataAccess.GetTopicsByCategory(category).ToArray();
                topicsByCategory.Add(category, topics);
            }

            ViewBag.TopicsRank = topicsRank;
            ViewBag.CategoriesRank = categoriesRank;
            ViewData["category"] = GetDropdown(categories.ToArray());
            ViewBag.TopicsByCategory = topicsByCategory;

            return View();
        }

        public ActionResult ShowStatistics() {
            ViewBag.activities = ActivityDataAccess.GetAllActivitiesParticipants();
            return View();
        }
    }
}