using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planetarium.Controllers
{
    public class CollaboratorController : Controller
    {
        // GET: Collaborator
        public ActionResult ActivitiesApprobation()
        {
            return View();
        }
    }
}