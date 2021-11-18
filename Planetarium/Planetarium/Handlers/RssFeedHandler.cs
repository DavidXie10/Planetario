using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;
using HtmlAgilityPack;
using Planetarium.Models;

namespace Planetarium.Handlers {
    public class RssFeedHandler : DatabaseHandler {

        static Dictionary<string, int> MONTH = new Dictionary<string, int> {
            {"Jan", 1},
            {"Feb", 2},
            {"March", 3},
            {"Mar", 3},
            {"April", 4},
            {"Apr", 4},
            {"May", 5},
            {"Jun", 6 },
            {"June",6},
            {"July", 7},
            {"Jul", 7},
            {"August", 8},
            {"Aug", 8},
            {"Sep", 9},
            {"Oct", 10},
            {"Nov", 11},
            {"Dec", 12}
        };

        static Dictionary<int, string> DAYS = new Dictionary<int, string> {
            {1, "01" },
            {2, "02" },
            {3, "03" },
            {4, "04" },
            {5, "05" },
            {6, "06" },
            {7, "07" },
            {8, "08" },
            {9, "09" }
        };


        public List<EventModel> GetRssFeed(string url = "defaultURL") {
            List<EventModel> feed = new List<EventModel>();
            try {
                XPathDocument doc = new XPathDocument("https://www.space.com/feeds/all");
                XPathNavigator navigator = doc.CreateNavigator();
                XPathNodeIterator nodes = navigator.Select("//item");
                while (nodes.MoveNext()) {
                    XPathNavigator node = nodes.Current;
                    feed.Add(InstanceEventModel(node));
                }
            }catch {
                feed = null;
                Console.WriteLine("No se pudo obtener datos RSS");
            }
            return feed;
        }

        private EventModel InstanceEventModel(XPathNavigator node) {
            return new EventModel {
                Title = node.SelectSingleNode("title").Value,
                Description = node.SelectSingleNode("description").Value,
                Link = node.SelectSingleNode("link").Value,
                Date = node.SelectSingleNode("pubDate").Value,
                ImgURL = node.SelectSingleNode("enclosure/@url").Value
            };
        }

        public List<EventModel> GetEventsFromFeed(string url = "https://www.timeanddate.com/astronomy/sights-to-see.html") {
            List<EventModel> events = new List<EventModel>();
            var webPage = new HtmlWeb();
            var DOM = webPage.Load(url);
            foreach (HtmlNode node in DOM.DocumentNode.SelectNodes("//article")) {
                string innerText = node.ChildNodes[0].InnerText;
                if (innerText.Contains(':')) {
                    AddEvent(events, innerText, node);
                }
            }
            return events;
        }

        private void AddEvent(List<EventModel> events, string innerText, HtmlNode node) {
            string date = FormatDate(innerText.Split(':')[0]);
            string title = innerText.Split(':')[1];
            string description = node.ChildNodes[2].InnerText;
            string link = "https://www.timeanddate.com/" + node.ChildNodes[0].ChildNodes[1].GetAttributeValue("href", string.Empty);
            if (!date.Contains("-")) {
                events.Add(new EventModel {
                    Title = title,
                    Description = description,
                    Date = date.Replace("/", "-"),
                    Link = link,
                    ImgURL = "",
                    Color = "#13967d"
                });
            }
        }

        public string FormatDate(string date) {
            string formatedDate = "";
            int currentMonth = Convert.ToInt32(DateTime.Now.ToString("MM"));
            int currentYear = Convert.ToInt32(DateTime.Now.Year.ToString());
            int month = MONTH[date.Split(' ')[0]];
            string day = date.Split(' ')[1];
            int dayToInt;

            if (day.Contains('/')) {
                dayToInt = Convert.ToInt32(day.Split('/')[1]);
            } else {
                dayToInt = Convert.ToInt32(day);
            }

            if(dayToInt < 10) {
                day = DAYS[dayToInt];
            } else {
                day = dayToInt.ToString();
            }

            if (month >= currentMonth) {
                formatedDate = currentYear + "/" + month + "/" + day.Replace("/", "-");
            }
            else {
                formatedDate = (currentYear + 1) + "/" + month + "/" + day.Replace("/", "-");
            }
            
            return formatedDate;
        } 
    }
}