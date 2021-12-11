using System;

namespace Planetarium.Models {
    public class ItemAdvanceModel {
        public int ID { get; set; }

        public string Name { get; set; }

        public int Children { get; set; }

        public int Juvenile { get; set; }

        public int Adult { get; set; }

        public int Senior { get; set; }

        public double Income { get; set; }
    }
}