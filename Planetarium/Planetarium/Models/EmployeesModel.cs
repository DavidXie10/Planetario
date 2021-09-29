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
        public string IdPhoto  { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese su nombre")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese sus apellidos")]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        [Display(Name = "Títulos académicos")]
        public string AcademicDegree { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese su ocupación")]
        [Display(Name = "Ocupación")]
        public string Occupation { get; set; }

        [Display(Name = "Correo")]
        public string Mail { get; set; }
        public string Phrase { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese el número de cédula")]
        [Display(Name = "Número de cédula")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Debe ingresar números")]
        public string Dni { get; set; }

        [Display(Name = "Teléfono")]
        public int PhoneNumber { get; set; }

        [Required(ErrorMessage = "Es necesario que seleccione una opción")]
        [Display(Name = "Género")]
        public char Gender { get; set; }

        [Display(Name = "Lugar de residencia")]
        public string Address { get; set; }
        public string ExpertiseArea { get; set; }
        public DateTime BirthDay { get; set; }
        public string[] Idioms { get; set; }

        [Required(ErrorMessage = "Debe agregar un archivo (JPEG, PNG)")]
        [Display(Name = "Ingrese el archivo con su foto de perfil")]
        public HttpPostedFileBase Photo { get; set; }
        public string FileType { get; set; }
    }
}