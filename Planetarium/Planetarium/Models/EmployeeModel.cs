using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Planetarium.Models
{
    public class EmployeeModel
    {        
        [Required(ErrorMessage = "Es necesario que ingrese su nombre")]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese sus apellidos")]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        [Display(Name = "Correo")]
        public string Mail { get; set; }

        [Display(Name = "Seleccione el género")]
        public char Gender { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese el número de cédula")]
        [Display(Name = "Número de cédula")]
        public string Dni { get; set; }

        [Display(Name = "Teléfono")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Debe ingresar números")]
        public int PhoneNumber { get; set; }

        [Display(Name = "Área de expertiz")]
        public string ExpertiseArea { get; set; }

        [Display(Name = "Lugar de residencia")]
        public string Address { get; set; }

        [Display(Name = "Títulos académicos")]
        public string AcademicDegree { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese la ocupación")]
        [Display(Name = "Ocupación")]
        public string Occupation { get; set; }

        [Required(ErrorMessage = "Debe agregar un archivo (PNG, JPEG)")]
        [Display(Name = "Ingrese el archivo con su foto de perfil")]
        public HttpPostedFileBase PhotoFile { get; set; }

        public string PhotoFileType { get; set; }
    }
}