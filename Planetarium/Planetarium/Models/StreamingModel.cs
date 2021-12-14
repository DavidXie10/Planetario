using System.ComponentModel.DataAnnotations;

namespace Planetarium.Models {
    public class StreamingModel {
        
        [Required(ErrorMessage = "Es necesario que ingrese el enlace")]
        [Display(Name = "Enlace del streaming")]
        public string Link { get; set; }

        [Display(Name = "Actividad Educativa")]
        public string EducationalActivityTitle { get; set; }
    }
}