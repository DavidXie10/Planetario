using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Planetarium.Models;

namespace Planetarium.Controllers {
    public class GamesController : Controller {
        ContentParser contentParser;

        public GamesController() {
            contentParser = new ContentParser(); 
        }

        public ActionResult HangManGame() {
            return View();
        }

        public ActionResult TypingGame() {
            return View();
        }

        public ActionResult ListOfGames() {
            return View();
        }

        public ActionResult SpaceGame()  {
            return View();
        }

        public ActionResult OuterSpaceGame() {
            return View();
        }

        public ActionResult MemoryGame() {
            @ViewBag.Scientists = contentParser.GetScientistsFromJson();

            return View();
        }
    }
}