using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Planetarium.Models {
    public class ContentParser {

        /*string contentExtracted { get; set; }*/

        private string fileName { get; set; }

        public ContentParser() {
            this.fileName = "";
        }

        public string GetContentFromFile(string fileName) {
            string contentExtracted = "";
            try {
                var content = extractRawContent();
                contentExtracted = parseContent(content);
            } catch (Exception e) {
                Console.WriteLine("File not found\n" +  e.ToString());
                contentExtracted += "File not found";
            }
            return contentExtracted;
        }

        private string[] ExtractRawContent() {
            return File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data_Files/" + this.fileName));
        }

        private string ParseContent(string[] content) {
            string contentExtracted = "";
            foreach (string line in content) {
                if (IsStringEmpty(line)) {
                    contentExtracted += "<br/>";
                } else {
                    contentExtracted += line + "\n";
                }
            }
            return contentExtracted;
        }
        private bool IsStringEmpty(String line) {
            return line == "" ? true : false;
        }

    }
}