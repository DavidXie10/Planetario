using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Planetarium.Models {
    public class EducationalActivityModel {
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

        [Required(ErrorMessage = "Es necesario que describa la actividad")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        public string Publisher { get; set; }

        public List<string> RefEducationalMaterial { get; set; }

    }
}