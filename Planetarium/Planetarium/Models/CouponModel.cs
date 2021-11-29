using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Planetarium.Models
{
    public class CouponModel
    {
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Código")]
        public string Code { get; set; }

        [Display(Name = "Antes de")]
        public string Date { get; set; }
    }
}