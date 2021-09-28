using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace Planetarium.Models {
    public class ContentParser {

        public ContentParser() {
        }

        public string GetContentFromFile(string fileName) {
            string contentExtracted = "";
            try {
                var content = ExtractRawContent(fileName);
                contentExtracted = ParseContent(content);
            } catch (Exception e) {
                Console.WriteLine("File not found\n" +  e.ToString());
                contentExtracted += "File not found";
            }
            return contentExtracted;
        }

        private string[] ExtractRawContent(string fileName) {
            return File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data_Files/" + fileName));
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

        public string ParseRawJson(string[] content) {
            string contentExtracted = "";
            foreach (string line in content) {
                contentExtracted += line + "\n";
            }
            return contentExtracted;
        }

        public dynamic ParseFromJSON(string fileName) {
            dynamic parsedContent = "";
            try {
                string[] rawData = ExtractRawContent(fileName);
                string contentReadyToParse = ParseRawJson(rawData);
                parsedContent = JsonConvert.DeserializeObject(contentReadyToParse);
            } catch (Exception e) {
                string error = "Error while parsing JSON raw data \n" + e.ToString();
                Debug.WriteLine(error);
            }
            return parsedContent;
        }
    }
}