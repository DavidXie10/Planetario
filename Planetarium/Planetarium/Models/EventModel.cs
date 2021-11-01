using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planetarium.Models {
    public class EventModel {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; } 
        public string Date { get; set; }
        public string ImgURL { get; set; }
    }
}