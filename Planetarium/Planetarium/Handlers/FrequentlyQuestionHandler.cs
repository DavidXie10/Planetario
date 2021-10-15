using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Planetarium.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Planetarium.Handlers {
    public class FrequentlyQuestionHandler : DatabaseClassificationsHandler {

        public bool CreateFrequentlyAskedQuestion(FrequentlyQuestionModel faqQuestion) {
            bool success = false;
            string query = "INSERT INTO PreguntaFrecuente (pregunta, respuesta, cedulaFK) " +
                           "VALUES (@pregunta, @respuesta, '503250235')";
            SqlCommand queryCommand = new SqlCommand(query, connection);

            queryCommand.Parameters.AddWithValue("@pregunta", faqQuestion.Question);
            queryCommand.Parameters.AddWithValue("@respuesta", faqQuestion.Answer);

            success = DatabaseQuery(queryCommand);

            query = "SELECT IDENT_CURRENT('PreguntaFrecuente') ";
            DataTable resultingTable = CreateTableFromQuery(query);

            int questionId = Convert.ToInt32(resultingTable.Rows[0][0]);

            foreach (string topic in faqQuestion.Topics) {
                query = "INSERT INTO PreguntaFrecuentePerteneceATopico " +
                        "VALUES (" + questionId + ",'" + topic + "')";
                success = DatabaseQuery(query);
            }
            return success;
        }

        public List<FrequentlyQuestionModel> GetAllQuestions(Dictionary<string, List<FrequentlyQuestionModel>> questionsSortedByCategory) {
            List<FrequentlyQuestionModel> frequentlyAskedQuestions = new List<FrequentlyQuestionModel>();
            string query = "SELECT * FROM PreguntaFrecuente ";
            DataTable resultingTable = CreateTableFromQuery(query);
            foreach (DataRow column in resultingTable.Rows) {
                frequentlyAskedQuestions.Add(
                    new FrequentlyQuestionModel {
                        Question = Convert.ToString(column["pregunta"]),
                        Answer = Convert.ToString(column["respuesta"]),
                        QuestionId = Convert.ToInt32(column["idPreguntaPK"])
                    }
                );
            }

            LinkAllFeatureWithTopics(CreateDictionary(frequentlyAskedQuestions));

            LinkAllQuestionWithCategory(frequentlyAskedQuestions);

            return frequentlyAskedQuestions;
        }

        //private Dictionary<string[], List<string>> CreateDictionary<Feature>(List<Feature> featuresList) {
        //    Dictionary<string[], List<string>> tempDictionary = new Dictionary<string[], List<string>>();
        //    foreach (Feature featureInstance in featuresList) {
        //        tempDictionary.Add(new string[] { newsInstance.Title }, newsInstance.Topics = new List<string>());
        //    }
        //    return tempDictionary;
        //}

        private Dictionary<string[], List<string>> CreateDictionary(List<FrequentlyQuestionModel> frequentlyQuestionList) {
            Dictionary<string[], List<string>> tempDictionary = new Dictionary<string[], List<string>>();
            foreach (FrequentlyQuestionModel frequentlyQuestionInstance in frequentlyQuestionList) {
                tempDictionary.Add(new string[] { frequentlyQuestionInstance.QuestionId.ToString() }, frequentlyQuestionInstance.Topics = new List<string>());
            }
            return tempDictionary;
        }

        override protected DataTable GetFeatureWithTopicsTable(string[] keys) {
            string query = "SELECT * FROM PreguntaFrecuente PF " +
                        "INNER JOIN PreguntaFrecuentePerteneceATopico PFPAT ON PF.idPreguntaPK = PFPAT.idPreguntaFrecuentePKFK  " +
                        "WHERE idPreguntaPK = '" + keys[0] + "'";
            return CreateTableFromQuery(query);
        }

        private void LinkQuestionWithTopics(FrequentlyQuestionModel frequentQuestion, DataTable resultingTable) {
            frequentQuestion.Topics = new List<string>();
            foreach (DataRow column in resultingTable.Rows) {
                var tempTopic = Convert.ToString(column["nombreTopicoPKFK"]);
                frequentQuestion.Topics.Add(tempTopic);
            }
        }

        private void LinkAllQuestionWithCategory(List<FrequentlyQuestionModel> frequentQuestions) {
            foreach (FrequentlyQuestionModel frequentQuestion in frequentQuestions) {
                DataTable resultingTableOfQuestionWithTheirCategory = GetQuestionWithCategoryTable(frequentQuestion.Topics[0]);
                LinkQuestionWithCategory(frequentQuestion, resultingTableOfQuestionWithTheirCategory);
            }
        }

        private DataTable GetQuestionWithCategoryTable(string topic) {
            string query = "SELECT categoria FROM PreguntaFrecuentePerteneceATopico PFPAT " +
                        "INNER JOIN Topico T ON T.nombrePK = PFPAT.nombreTopicoPKFK  " +
                        "WHERE T.nombrePK = '" + topic + "'";
            return CreateTableFromQuery(query);
        }

        private void LinkQuestionWithCategory(FrequentlyQuestionModel question, DataTable resultingTable) {
            question.Category = Convert.ToString(resultingTable.Rows[0]["categoria"]);
        }



        /*public List<string> GetAllCategories() {
            List<string> categories = new List<string>();

            string query = "SELECT DISTINCT categoria FROM Topico";
            DataTable resultingTable = CreateTableFromQuery(query);
            foreach (DataRow column in resultingTable.Rows) {
                categories.Add(Convert.ToString(column["categoria"]));
            }

            return categories;
        }*/

        public List<string> GetTopicsByCategory(string category) {

            List<string> topics = new List<string>();

            string query = "SELECT nombrePK " +
                            "FROM Topico T " +
                            "WHERE T.categoria LIKE '%" + category + "%';";

            DataTable topicsDataTable = CreateTableFromQuery(query);

            foreach (DataRow column in topicsDataTable.Rows) {
                topics.Add(Convert.ToString(column["nombrePK"]));
            }

            return topics;
        }

        public List<string> GetQuestionsByCategory(string category) {

            List<string> topics = new List<string>();

            string query = "SELECT nombrePK " +
                            "FROM Topico T " +
                            "WHERE T.categoria LIKE '%" + category + "%';";

            DataTable topicsDataTable = CreateTableFromQuery(query);

            foreach (DataRow column in topicsDataTable.Rows) {
                topics.Add(Convert.ToString(column["nombrePK"]));
            }

            return topics;
        }
    }
}