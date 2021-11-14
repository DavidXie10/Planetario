using System.ComponentModel.DataAnnotations;

namespace Planetarium.Models {
    public class StreamingModel {
        [Required(ErrorMessage = "Es necesario que seleccione el título de la actividad")]
        [Display(Name = "Actividad a la que pertenece")]
        public string ActivityTitle { get; set; }
        
        [Required(ErrorMessage = "Es necesario que ingrese el enlace")]
        [Display(Name = "Enlace del evento")]
        [RegularExpression("^((embed).)*$", ErrorMessage = "Debe ingresar un enlace válido")]
        public string Link { get; set; }
    }
}