using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;

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

        public Dictionary<string, string[]> GetScientistsFromJson() {
            dynamic jsonCollection = ParseFromJSON("MemoryGame.json");
            Dictionary<string, string[]> scientists = new Dictionary<string, string[]>();

            foreach (var element in jsonCollection) {
                scientists.Add(element.Id.ToString(), new string[]{ element.Name, element.Description, element.ImageRef });
            }

            return scientists;
        }

        public List<QuizModel> GetQuizzesFromJson(dynamic jsonCollection) {
            List<QuizModel> quizzes = new List<QuizModel>();
            foreach(var element in jsonCollection) {
                quizzes.Add(new QuizModel {
                    Title = element.Title,
                    Description = element.Description,
                    Difficulty = element.Difficulty,
                    Link = element.Link
                });
            }
            return quizzes;
        }

        public List<CelestialBody> GetCelestialBodiesFromJson(dynamic jsonCollection) {
            List<CelestialBody> bodies = new List<CelestialBody>();
            foreach(var element in jsonCollection) {
                bodies.Add(new CelestialBody {
                    Name = element.Name,
                    Size = element.Size
                });
            }
            return bodies;
        }

        public List<Model> GetContentsFromJson<Model>(string jsonFile, Func<dynamic, List<Model>> GetModelsFromJson) {
            List<Model> models = new List<Model>();
            try {
                string[] rawContent = ExtractRawContent(jsonFile);
                string jsonString = ParseRawJson(rawContent);
                dynamic jsonCollection = JsonConvert.DeserializeObject(jsonString);
                models = GetModelsFromJson(jsonCollection);               
            } catch {
                models = null;
            }
            return models;
        }

        public List<StreamingModel> GetStreamingsFromJson(dynamic jsonCollection) {
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
                string[] rawJson = ExtractRawContent(fileName);
                string json = ParseRawJson(rawJson);
                dynamic jsonCollection = JsonConvert.DeserializeObject(json);
                List<Model> previousModels = GetModelsFromJson(jsonCollection);

                previousModels.Add(model);
                resultingJson = JsonConvert.SerializeObject(previousModels);
            } catch {
                Debug.WriteLine("Error occurred");
            }
            return resultingJson;
        }
    }
}