using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Planetarium.Models {
    public class ContentParser {

        string contentExtracted { get; set; }

        private string _fileName;
        public string fileName {
            get {
                return _fileName;
            }
            set {
                _fileName = value;
            }
        }
        public ContentParser(string fileName) {
            this.fileName = fileName;
        }

        public string getContentFromFile() {
            this.contentExtracted = "";
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