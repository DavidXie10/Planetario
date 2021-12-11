using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Planetarium.Models {
    public class CouponModel {
        
        [Display(Name = "Código")]
        public string Code { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Antes de")]
        public DateTime Date { get; set; }

        [Display(Name = "Descuento")]
        public double Discount { get; set; }

        [Display(Name = "Dueño")]
        public string VisitorDni { get; set; }
    }
}