using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Planetarium.Models {

    public class VisitorModel {
        [Required(ErrorMessage = "Es necesario que ingrese su nombre completo")]
        [Display(Name = "Nombre completo")]
        public string FullName { get; set; }

        [Display(Name = "Correo")]
        public string Mail { get; set; }

        [Display(Name = "Seleccione el género")]
        public char Gender { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese el número de cédula")]
        [Display(Name = "Número de cédula")]
        public string Dni { get; set; }

        [Display(Name = "País de origen")]
        public string NativeCountry { get; set; }

        [Display(Name = "Nivel educativo")]
        public string EducationalLevel { get; set; }

        [Required(ErrorMessage = "Debe ingresar fecha de nacimiento")]
        [Display(Name = "Fecha de nacimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

    }
}