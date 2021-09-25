﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Planetarium.Models {
    public class FrequentlyQuestionModel {
        [Required(ErrorMessage = "Es necesario que seleccione una categoría")]
        [Display(Name = "Seleccione la categoría")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Es necesario que seleccione un tópico")]
        [Display(Name = "Seleccione el tópico")]
        public string Topic { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese el texto de la pregunta")]
        [Display(Name = "Ingrese la pregunta")]
        public string Question { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese el texto de la respuesta")]
        [Display(Name = "Ingrese la respuesta")]
        public string Answer { get; set; }

        //TO-DO: public var cedulaFK de la persona que escribe
    }
}