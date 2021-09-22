using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Planetarium.Models {
    public class ContentParser {

        string contentExtracted { get; set; }
        string fileName { get; set; }
        public ContentParser(string fileName) {
            this.fileName = fileName;
        }

        public string getContentFromFile() {
            try {
                var content = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data_Files/" + this.fileName));
                foreach(string line in content) {
                    this.contentExtracted += line + "\n";
                }
            } catch (Exception e) {
                Console.WriteLine("File not found\n" +  e.ToString());
                this.contentExtracted += "File not found";
            }
            return this.contentExtracted;
        }

    }
}