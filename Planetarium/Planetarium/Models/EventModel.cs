using System.ComponentModel.DataAnnotations;

namespace Planetarium.Models { 
    public class EventModel {
        [Required(ErrorMessage = "Es necesario que ingrese el título")]
        [Display(Name = "Título")]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Es necesario que ingrese el tipo de eventoS")]
        [Display(Name = "-Tipo de evento-")]
        public string TypeOfEvent { get; set; }
        public string Link { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese la fecha")]
        [Display(Name = "Día del evento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string Date { get; set; }
        public string ImgURL { get; set; }

        public string Color { get; set; }
    }
}