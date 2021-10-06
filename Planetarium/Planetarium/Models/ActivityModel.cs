using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planetarium.Models {
    public class ActivityModel {
        
        public string Title { get; set; }

        public string Body { get; set; }

        public DateTime Date { get; set; }

        public int Duration { get; set; }

        public string Capacity { get; set; }

        public float Price { get; set; }

        public string Complexity { get; set; }

        public int  State { get; set; }

        public string Type { get; set; }

        public string Link { get; set; }

        public string Description { get; set; }

        public string Publisher { get; set; }

        public string Category { get; set; }

        public string Language { get; set; }

        public List<string> Topics { get; set; }

    }
}