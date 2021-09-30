using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planetarium.Models
{
    public class NewsModel
    {
        public string Title { get; set; }
    
        public DateTime Date { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public string PublisherId { get; set; }

        public string Author { get; set; }

        public string Category { get; set; }

        public List<string> Topics { get; set; }

        public string ImageRef { get; set; }
    }
}