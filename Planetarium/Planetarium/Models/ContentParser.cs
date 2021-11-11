﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace Planetarium.Models {
    public class ContentParser {
        private string[] ExtractRawContent(string fileName) {
            return File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data_Files/" + fileName));
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

        public List<string> GetListFromString(string content) {
            List<string> listFromContent = new List<string>();
            string[] contentList = content.Split('|');
            foreach (string contentInList in contentList) {
                if (contentInList != "") {
                    listFromContent.Add(contentInList.Replace("_", " "));
                }
            }
            return listFromContent;
        }
        
        public string GetStringFromList(List<string> listContent) {
            string content = "";

            foreach (string element in listContent) {
                content += element.Replace(" ", "_") + "|";
            }

            return content;
        }

        public List<Model> GetContentsFromJson<Model>(string jsonFile, Func<dynamic, List<Model>> GetModelsFromJson) {
            List<Model> models = new List<Model>();
            try
            {
                string[] rawContent = ExtractRawContent(jsonFile);
                string jsonString = ParseRawJson(rawContent);
                dynamic jsonCollection = JsonConvert.DeserializeObject(jsonString);
                models = GetModelsFromJson(jsonCollection);               
            } catch{
                models = null;
            }
            return models;
        }

        public List<StreamingModel> GetLiveStreamLinksFromJson(dynamic jsonCollection) {
            List<StreamingModel> streamings = new List<StreamingModel>();
            foreach (var element in jsonCollection) {
                streamings.Add(new StreamingModel {
                    Link = element.Link
                });
            }
            return streamings;
        }

        public bool WriteToJsonFile<Model>(string fileName, Model model, Func<dynamic, List<Model>> GetModelsFromJson) {
            bool success = false;
            string jsonString = JoinNewData<Model>(fileName, model, GetModelsFromJson);
            try {
                File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data_Files/" + fileName), jsonString);
                success = true;
            } catch {
                Debug.WriteLine("Error occurred");
            }
            return success;
        }

        public string JoinNewData<Model>(string fileName, Model model, Func<dynamic, List<Model>> GetModelsFromJson) {
            string resultingJson = "";
            try {
                //Extracting the old values from the file
                string[] rawJson = ExtractRawContent(fileName);
                string json = ParseRawJson(rawJson);
                dynamic jsonCollection = JsonConvert.DeserializeObject(json);
                List<Model> previousModels = GetModelsFromJson(jsonCollection);

                //Adding the new one
                previousModels.Add(model);

                //Parsing the list to json
                resultingJson = JsonConvert.SerializeObject(previousModels);

            } catch {

            }
            return resultingJson;
        }

    }
}