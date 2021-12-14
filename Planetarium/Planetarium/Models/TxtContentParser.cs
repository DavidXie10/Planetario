using System;
using System.IO;
using System.Text;

namespace Planetarium.Models {
    public class TxtContentParser {
        public string ExtractRawContent(string fileName) {
            string fileContent = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data_Files/" + fileName));
            return fileContent;
        }

        public void WriteToFile(string content, string filename) {
            File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data_Files/" + filename), content, Encoding.UTF8);
        }
    }
}