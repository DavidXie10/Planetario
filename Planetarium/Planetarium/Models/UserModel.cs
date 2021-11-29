using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planetarium.Models {
    public class UserModel {

        public string Username { get; set; }
        public string Password { get; set; }
        public string Dni { get; set; }
        public int EmployeeFlag { get; set; }
        public int Rol { get; set; }

    }
}