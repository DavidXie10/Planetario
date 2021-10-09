using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Planetarium.Models
{
    public class EducationalMaterialModel
    {
        [Required(ErrorMessage = "Es necesario que tenga un autor")]
        [Display(Name = "Autor")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Es necesario que tenga un título")]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Es necesario que seleccione la actividad asociada al material")]
        [Display(Name = "Tipo de actividad")]
        public string ActivityTitle { get; set; }

        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Es necesario que seleccione una categoría")]
        [Display(Name = "Seleccione la categoría")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Es necesario que seleccione un tópico y una categoría")]
        [Display(Name = "Seleccione el tópico")]
        public List<string> Topics { get; set; }

        public List<string> EducationalMaterialFileNames { get; set; }
    }
}