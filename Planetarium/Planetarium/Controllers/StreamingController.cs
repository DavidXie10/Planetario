using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Models;

namespace Planetarium.Controllers {
    public class StreamingController : Controller {

        ContentParser parser = new ContentParser();

        public ActionResult Streaming() {
            
            return View();
        }

        public ActionResult InternetStreaming()
        {
            List<StreamingModel> streamings = parser.GetContentsFromJson<StreamingModel>("Streamings.json", parser.GetLiveStreamLinksFromJson);
            ViewBag.Streamings = streamings;
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
                ViewBag.SuccessOnCreation = parser.WriteToJsonFile<StreamingModel>("Streamings.json", streaming, parser.GetLiveStreamLinksFromJson);
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
