using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.ComponentModel.DataAnnotations;

namespace Planetarium.Models
{
    public class EducationalActivityModel
    {
        [Required(ErrorMessage = "Es necesario que tenga un título")]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Es necesario que seleccione una actividad")]
        [Display(Name = "Tipo de actividad")]
        public string ActivityType { get; set; }

        [Required(ErrorMessage = "Es necesario que seleccione el nivel de complejidad")]
        [Display(Name = "Nivel de complejidad")]
        public string ComplexityLevel { get; set; }

        [Required(ErrorMessage = "Es necesario que seleccione una categoría")]
        [Display(Name = "Seleccione la categoría")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Es necesario que seleccione un tópico y una categoría")]
        [Display(Name = "Seleccione el tópico")]
        public List<string> Topics { get; set; }

        [Required(ErrorMessage = "Es necesario que seleccione el público meta")]
        [Display(Name = "Público meta")]
        public List<string> TargetAudience { get; set; }

        [Required(ErrorMessage = "Es necesario que ponga la duración")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Debe ingresar números")]
        [Display(Name = "Duración (Minutos)")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Es necesario que seleccione la fecha de inicio de la actividad")]
        [Display(Name = "Seleccione fecha de inicio")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Es necesario que describa la actividad")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Es necesario que seleccione un precio de la actividad")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Debe ingresar números")]
        [Display(Name = "Introduzca el precio en colones")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Es necesario que introduzca la capacidad máxima")]
        [Display(Name = "Capacidad máxima")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Debe ingresar un número")]
        public int MaximumCapacity { get; set; }

        [Required(ErrorMessage = "Es necesario que seleccione el tipo de asistencia con la que va a contar")]
        [Display(Name = "Tipo de asistencia")]
        public string TypeOfAssistance { get; set; }

        public string State { get; set; }

        [Display(Name = "Enlace a la actividad")]
        public string Link { get; set; }

        public string Publisher { get; set; }

    }
}