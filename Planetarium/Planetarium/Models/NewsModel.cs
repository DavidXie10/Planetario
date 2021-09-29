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

        public string AuthorId { get; set; }

        public string Category { get; set; }

        public List<string> Topics { get; set; }
    }
}