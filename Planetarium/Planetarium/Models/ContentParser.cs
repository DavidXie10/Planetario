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
        //Write Methods
        public bool WriteToJsonFile(string fileName, QuizModel quiz) {
            bool success = false;
            string jsonString = JoinNewData(fileName, quiz);
            try {
                File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data_Files/" + fileName), jsonString);
                success = true;
            } catch(Exception e) {
                Debug.WriteLine("Error occurred");
            }
            return success;
        }
               

        //Read Methods
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


        public List<QuizModel> GetQuizzes(string jsonFile) {
            List<QuizModel> quizzes = new List<QuizModel>();
            try {
                string[] rawContent = ExtractRawContent(jsonFile);
                string jsonString = ParseRawJson(rawContent);
                dynamic jsonCollection = JsonConvert.DeserializeObject(jsonString);
                quizzes = GetQuizzesFromJson(jsonCollection);
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
                quizzes = null;
            }
            return quizzes;
        }

        public List<QuizModel> GetQuizzesFromJson(dynamic jsonCollection) {
            List<QuizModel> quizzes = new List<QuizModel>();
            foreach(var element in jsonCollection) {
                quizzes.Add(new QuizModel {
                    Titulo = element.Titulo,
                    Descripcion = element.Descripcion,
                    Dificultad = element.Dificultad,
                    Link = element.Link
                });
            }
            return quizzes;
        }

        public string JoinNewData(string fileName, QuizModel quiz) {
            string resultingJson = "";
            try {
                //Extracting the old values from the file
                string[] rawJson = ExtractRawContent(fileName);
                string json = ParseRawJson(rawJson);
                dynamic jsonCollection = JsonConvert.DeserializeObject(json);
                List<QuizModel> previousQuizzes = GetQuizzesFromJson(jsonCollection);

                //Adding the new one
                previousQuizzes.Add(quiz);

                //Parsing the list to json
                resultingJson = JsonConvert.SerializeObject(previousQuizzes);

            } catch(Exception e) {
                
            }
            return resultingJson;
        }


    }
}