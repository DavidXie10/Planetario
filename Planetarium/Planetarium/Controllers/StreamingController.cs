using System.Web.Mvc;
using Planetarium.Models;
using Planetarium.Handlers;

namespace Planetarium.Controllers {
    public class StreamingController : Controller {
        public EducationalActivityHandler ActivityDataAccess { get; set; }
        public ContentParser ContentParser { get; set; }

        public StreamingController() {
            ActivityDataAccess = new EducationalActivityHandler();
            ContentParser = new ContentParser();
        }

        public ActionResult Streaming(string activityTitle, string activityPublisher) {
            ViewBag.ActivityTitle = activityTitle;
            ViewBag.ActivityPublisher = activityPublisher;
            return View();
        }

        public ActionResult InternetStreaming() {
            ViewBag.Streamings = ContentParser.GetContentsFromJson<StreamingModel>("Streamings.json", ContentParser.GetStreamingsFromJson);
            return View();
        }

        public ActionResult StreamingForm() {
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
                    ViewBag.Message = "El streaming fue agregado con éxito";
                    ModelState.Clear();
                }
            } catch {
                ViewBag.Message = "Algo salió mal y no fue posible crear el streaming";
            }
            return view;
        }
    }
}
