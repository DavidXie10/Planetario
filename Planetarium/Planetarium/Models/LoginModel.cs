using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planetarium.Models {
    public class LoginModel {

        [Display(Name = "Nombre de usuario")]
        [Required(ErrorMessage = "Debe ingresar un nombre de usuario")]
        public string Username { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "Debe ingresar una contraseña")]
        public string Password { get; set; }


    }
}