using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Handlers;
using Planetarium.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace Planetarium.Controllers
{
    public class CollaboratorController : Controller
    {
        // GET: Collaborator
        public ActionResult ActivitiesApprobation()
        {
            ActivityHandler dataAccess = new ActivityHandler();
            ViewBag.activities = dataAccess.GetAllActivities();
            return View();
        }


        private void SendEmail() {
            MimeMessage message = new MimeMessage();

            MailboxAddress from = new MailboxAddress("Coordinador", "mauricio.rojassegnini@ucr.ac.cr");
            message.From.Add(from);

            MailboxAddress to = new MailboxAddress("Educador", "davidxieli@gmail.com");
            message.To.Add(to);

            message.Subject = "Su propuesta está en revisión";

            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = "<h1>¡Hola!</h1> <p>Su propuesta ha sido aprobada</p>";
            bodyBuilder.TextBody = "¡Hola! Su propuesta ha sido aprobada";
            message.Body = bodyBuilder.ToMessageBody();

            SmtpClient client = new SmtpClient();
            client.Connect("smtp.ucr.ac.cr", 587);
            // TODO: cambiar nombre y contraseña
            // Nombre: nombre.apellido
            // Contraseña: del correo ucr
            client.Authenticate("mauricio.rojassegnini", "mauuam1771.");

            client.Send(message);
            client.Disconnect(true);
            client.Dispose();
        }


        [HttpPost]
        public ActionResult SubmitApprobation() {
            ActionResult view = RedirectToAction("ActivitiesApprobation", "Collaborator");
            int state = Convert.ToInt32(Request.Form["status"]);
            string title = Request.Form["myTitle"];

            ActivityHandler dataAccess = new ActivityHandler();

            ViewBag.SuccessOnCreation = false;
            try {
                ViewBag.SuccessOnCreation = dataAccess.UpdateActivityState(title, state);
                if (ViewBag.SuccessOnCreation) {
                    ModelState.Clear();
                    SendEmail();
                }
            } catch{
                TempData["Error"] = true;
                TempData["WarningMessage"] = "Algo salio mal";
            }

            return view;
        }


    }

}