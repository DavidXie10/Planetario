using System;

namespace Planetarium.Models {
    public class TicketModel {
        public string ActivityTitle { get; set; }

        public DateTime StartActivityDay { get; set; }

        public int QuantitySold { get; set; }

        public double Income { get; set; }
    }
}