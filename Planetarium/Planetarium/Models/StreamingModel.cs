using System.ComponentModel.DataAnnotations;

namespace Planetarium.Models {
    public class StreamingModel {
        
        [Required(ErrorMessage = "Es necesario que ingrese el enlace")]
        [Display(Name = "Enlace del evento")]
        public string Link { get; set; }
    }
}