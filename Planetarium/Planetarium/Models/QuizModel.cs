using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Planetarium.Models {
    public class QuizModel {

        [Display(Name = "Título")]
        public string Titulo { get; set; }
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        public string Dificultad { get; set; }
        public string Link { get; set; }

    }
}