using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Planetarium.Models {
    public class NewsModel {
        [Required(ErrorMessage = "Es necesario que tenga un titulo")]
        [Display(Name = "Título")]
        public string Title { get; set; }

        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Es necesario el resumen de la noticia")]
        [Display(Name = "Resumen")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Es necesario que la noticia tenga contenido")]
        [Display(Name = "Contenido de la noticia")]
        public string Content { get; set; }

        public string PublisherId { get; set; }

        [Required(ErrorMessage = "Es necesario que seleccione una categoría")]
        [Display(Name = "Seleccione la categoría")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Es necesario que seleccione un tópico y una categoría")]
        [Display(Name = "Seleccione el tópico")]
        public List<string> Topics { get; set; }

        public List<string> ImagesRef { get; set; }
    }
}