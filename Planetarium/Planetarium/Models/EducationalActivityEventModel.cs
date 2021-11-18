using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Planetarium.Models {
    public class EducationalActivityEventModel : EducationalActivityModel {
        [Required(ErrorMessage = "Es necesario que seleccione a fecha de inicio de la actividad")]
        [Display(Name = "Seleccione fecha de inicio")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public String StatisticsDate { get; set; }

        [Required(ErrorMessage = "Es necesario que seleccione un precio de la actividad")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Debe ingresar números")]
        [Display(Name = "Introduzca el precio en colones")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Es necesario que introduzca la capacidad máxima")]
        [Display(Name = "Capacidad máxima")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Debe ingresar un número")]
        public int MaximumCapacity { get; set; }

        [Required(ErrorMessage = "Es necesario que indique el tipo de asistencia")]
        [Display(Name = "Tipo de asistencia")]
        public String TypeOfAssistance { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese el enlace de la actividad")]
        [Display(Name = "Enlace a la actividad")]
        public string Link { get; set; }

        public string State { get; set; }

        public Dictionary<string, int> RegisteredParticipants { get; set; }
    }
}

