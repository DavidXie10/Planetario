using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace Planetarium.Models
{
    public class EmployeesModel
    {
        public string IdPhoto  { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string AcademicDegree { get; set; }
        public string Occupation { get; set; }
        public string Mail { get; set; }
        public string Phrase { get; set; }
    }
}