using System.ComponentModel.DataAnnotations;

namespace Planetarium.Models {
    public class QuizModel {
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Dificultad")]
        public string Difficulty { get; set; }

        [Display(Name = "Enlace")]
        public string Link { get; set; }
    }
}