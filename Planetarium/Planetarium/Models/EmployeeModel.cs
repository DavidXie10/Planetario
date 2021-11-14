using System;
using System.Collections.Generic;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Planetarium.Models {
    public class EmployeeModel {
        [Required(ErrorMessage = "Es necesario que ingrese su nombre")]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese sus apellidos")]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        [Display(Name = "Correo")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Es necesario que seleccione una opción")]
        [Display(Name = "Género")]
        public char Gender { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese su número de cédula")]
        [Display(Name = "Número de cédula")]
        public string Dni { get; set; }

        [Display(Name = "Teléfono")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Debe ingresar números")]
        [Required(ErrorMessage = "Es necesario que ingrese su número de teléfono")]
        public int PhoneNumber { get; set; }

        [Display(Name = "Área de expertiz")]
        public string ExpertiseArea { get; set; }

        [Display(Name = "Títulos académicos")]
        public string AcademicDegree { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese su ocupación")]
        [Display(Name = "Ocupación")]
        public string Occupation { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese su país de origen")]
        [Display(Name = "País de origen")]
        public string NativeCountry { get; set; }

        [Display(Name = "Idiomas que habla")]
        public List<string> Languages { get; set; }

        public string PhotoPath { get; set; }

        [Required(ErrorMessage = "Es necesario que agregue una foto de perfil (PNG, JPEG)")]
        [Display(Name = "Ingrese el archivo con su foto de perfil")]
        public HttpPostedFileBase PhotoFile { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese su fecha de nacimiento")]
        [Display(Name = "Fecha de nacimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

    }
}