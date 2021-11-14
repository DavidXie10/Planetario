using System.Collections.Generic;
using System.Web.Mvc;
using Planetarium.Models;
using Planetarium.Handlers;
using System;

namespace Planetarium.Controllers {
    public class StreamingController : Controller {
        public EducationalActivityHandler ActivityDataAccess { get; set; }
        public ContentParser ContentParser { get; set; }

        public StreamingController() {
            ActivityDataAccess = new EducationalActivityHandler();
            ContentParser = new ContentParser();
        }
        public ActionResult Streaming() {
            
            return View();
        }

        public ActionResult InternetStreaming() {
            List<StreamingModel> streamings = ContentParser.GetContentsFromJson<StreamingModel>("Streamings.json", ContentParser.GetStreamingsFromJson);
            ViewBag.Streamings = streamings;
            return View();
        }

        public ActionResult StreamingForm() {
            ViewBag.DropDownActivitiesNames = ActivityDataAccess.GetAllActivitiesNames();

            return View();
        }

        public ActionResult ActivityStreaming(string activityTitle) {
            ViewBag.Streaming = ContentParser.GetContentsFromJson<StreamingModel>("Streamings.json", ContentParser.GetStreamingsFromJson).Find(smodel => String.Equals(smodel.ActivityTitle, activityTitle));

            return View();
        }

        [HttpPost]
        public ActionResult SubmitStreamingForm(StreamingModel streaming) {
            ActionResult view = RedirectToAction("StreamingForm", "Streaming");

            ViewBag.SuccessOnCreation = false;
            try {
                ViewBag.SuccessOnCreation = ContentParser.WriteToJsonFile<StreamingModel>("Streamings.json", streaming, ContentParser.GetStreamingsFromJson);
                if (ViewBag.SuccessOnCreation) {
                    view = RedirectToAction("Success", "Home");
                    ViewBag.Message = "El cuestionario fue agregado con exito";
                    ModelState.Clear();
                }
            } catch {
                ViewBag.Message = "Algo salió mal y no fue posible crear el formulario";
            }
            return view;
        }
    }
}
