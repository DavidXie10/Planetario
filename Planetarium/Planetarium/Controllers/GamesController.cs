using System.Web.Mvc;

using Planetarium.Models;

namespace Planetarium.Controllers {
    public class GamesController : Controller {
        ContentParser contentParser;

        public GamesController() {
            contentParser = new ContentParser(); 
        }

        public ActionResult HangManGame() {
            return View("HangManGame");
        }

        public ActionResult TypingGame() {
            return View("TypingGame");
        }

        public ActionResult ListOfGames() {
            return View("ListOfGames");
        }

        public ActionResult SpaceGame()  {
            return View("SpaceGame");
        }

        public ActionResult OuterSpaceGame() {
            return View("OuterSpaceGame");
        }

        public ActionResult MemoryGame() {
            @ViewBag.Scientists = contentParser.GetScientistsFromJson();
            return View("MemoryGame");
        }
    }
}