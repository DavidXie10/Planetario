using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planetarium.Models
{
    public class EducationalMaterialModel
    {
        public string Author { get; set; }

        public string Title { get; set; }

        public string ActivityTitle { get; set; }

        public DateTime Date { get; set; }

        public string Category { get; set; }

        public List<string> Topics { get; set; }

        public List<string> EducationalMaterialFileNames { get; set; }
    }
}