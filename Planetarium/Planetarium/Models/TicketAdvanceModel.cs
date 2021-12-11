using System;

namespace Planetarium.Models {
    public class TicketAdvanceModel {
        public string ActivityTitle { get; set; }

        public DateTime StartActivityDay { get; set; }

        public int Children { get; set; }

        public int Juvenile { get; set; }

        public int Adult { get; set; }

        public int Senior { get; set; }

        public double Income { get; set; }
    }
}