using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planetarium.Models {

    public class ActivityModel {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Date { get; set; }
    }
}