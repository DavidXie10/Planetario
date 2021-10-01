using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Planetarium.Models
{
    public class EmployeesModel
    {
        // public string IdPhoto  { get; set; }
        
        [Required(ErrorMessage = "Es necesario que ingrese su nombre")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese sus apellidos")]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        [Display(Name = "Correo")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese el número de cédula")]
        [Display(Name = "Número de cédula")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Debe ingresar números")]
        public string Dni { get; set; }

        [Display(Name = "Teléfono")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Debe ingresar números")]
        public int PhoneNumber { get; set; }

        /*
        [Display(Name = "Ocupación")]
        public string OriginCountry { get; set; }
        */

        [Display(Name = "Área de expertiz")]
        public string ExpertiseArea { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese la fecha de nacimiento")]
        [Display(Name = "Fecha de nacimiento")]
        public string BirthDay { get; set; }

        // Revisar que sea arreglo de strings
        // public string[] Idioms { get; set; }

        /*
        [Required(ErrorMessage = "Debe agregar un archivo (JPEG, PNG)")]
        [Display(Name = "Ingrese el archivo con su foto de perfil")]
        public HttpPostedFileBase Photo { get; set; }

        public string PhotoFileType { get; set; }
        */

        [Display(Name = "Lugar de residencia")]
        public string Address { get; set; }

        [Display(Name = "Títulos académicos")]
        public string AcademicDegree { get; set; }

        /*
        [Required(ErrorMessage = "Es necesario que seleccione una opción")]
        [Display(Name = "Género")]
        public char Gender { get; set; }
        */
    }
}