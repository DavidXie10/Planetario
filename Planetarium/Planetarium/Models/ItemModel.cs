using System;

namespace Planetarium.Models {
    public class ItemModel {
        public int ID { get; set; }

        public string Name { get; set; }

        public int QuantitySold { get; set; }

        public DateTime LastBoughtDate { get; set; }

        public string Category { get; set; }

        public double Income { get; set; }
    }
}