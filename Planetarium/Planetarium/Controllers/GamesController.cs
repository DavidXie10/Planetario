﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planetarium.Controllers
{
    public class GamesController : Controller
    {
        // GET: Games
        public ActionResult HangManGame()
        {
            return View();
        }
    }
}