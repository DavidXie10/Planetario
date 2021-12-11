using System.Collections.Generic;

namespace Planetarium.Models {
    public class SouvenirModel {
        public int SouvenirId { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public int Stock { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public List<string> ImagesRef { get; set; }

        public int SelectedCount { get; set; }
    }
}