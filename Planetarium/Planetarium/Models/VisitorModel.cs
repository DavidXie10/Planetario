using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Planetarium.Models {

    public class VisitorModel {
        [Required(ErrorMessage = "Es necesario que ingrese su nombre completo")]
        [Display(Name = "Nombre completo")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Por favor, solo ingrese letras")]
        public string FullName { get; set; }

        [Display(Name = "Correo")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese su género")]
        [Display(Name = "Seleccione el género")]
        public char Gender { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese el número de cédula")]
        [Display(Name = "Número de cédula")]
        [MaxLength(15, ErrorMessage = "La cédula debe tener un máximo de 15 caracteres")]
        [RegularExpression("^[a-zA-Z0-9]*", ErrorMessage = "Por favor, solo ingrese letras o números")]
        public string Dni { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese el país de origen")]
        [Display(Name = "País de origen")]
        public string NativeCountry { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese su nivel educativo")]
        [Display(Name = "Nivel educativo")]
        public string EducationalLevel { get; set; }

        [Required(ErrorMessage = "Debe ingresar fecha de nacimiento")]
        [Display(Name = "Fecha de nacimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Nombre de usuario")]
        [Required(ErrorMessage = "Debe ingresar un nombre de usuario")]
        public string Username { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "Debe ingresar una contraseña")]
        public string Password { get; set; }
    }
}