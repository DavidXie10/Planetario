using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetarium.Handlers;
using Planetarium.Models;

namespace Planetarium.Controllers
{
    public class NewsController : Controller
    {

        public ActionResult ListNews()
        {
            NewsHandler dataAccess = new NewsHandler();
            ViewBag.news = dataAccess.GetAllNews();
            return View();
        }

        public ActionResult News()
        {
            NewsModel news = new NewsModel
            {
                title = "Eclipse total de Sol este 2021",
                category = "Cuerpos celestes",
                topic = "Estrellas",
                date = "01 / 12 / 2021",
                description = "Consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt." +
                "Neque porro quisquam est,qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit," +
                "sed quia non numquam.Consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt.Neque" +
                " porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam. " +
                "Consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt." +
                "Neque porro quisquam est,qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit," +
                "sed quia non numquam.Consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt.Neque" +
                " porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam. " +
                "Consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt." +
                "Neque porro quisquam est,qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit," +
                "sed quia non numquam.Consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt.Neque" +
                " porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam. "
            };
            ViewBag.news = news;
            return View();
        }
    }
}