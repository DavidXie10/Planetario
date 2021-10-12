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
    public class FrequentlyQuestionHandler : DatabaseHandler{

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

            foreach (FrequentlyQuestionModel questionInstance in frequentlyAskedQuestions) {
                query = "SELECT * FROM PreguntaFrecuente PF " +
                        "INNER JOIN PreguntaFrecuentePerteneceATopico PFPAT ON PF.idPreguntaPK = PFPAT.idPreguntaFrecuentePKFK  " +
                        "WHERE idPreguntaPK = '" + questionInstance.QuestionId + "'";
                resultingTable = CreateTableFromQuery(query);
                questionInstance.Topics = new List<string>();
                foreach (DataRow column in resultingTable.Rows) {
                    var tempTopic = Convert.ToString(column["nombreTopicoPKFK"]);
                    questionInstance.Topics.Add(tempTopic);
                }
            }

            foreach (FrequentlyQuestionModel questionInstance in frequentlyAskedQuestions) {
                query = "SELECT categoria FROM PreguntaFrecuentePerteneceATopico PFPAT " +
                        "INNER JOIN Topico T ON T.nombrePK = PFPAT.nombreTopicoPKFK  " +
                        "WHERE T.nombrePK = '" + questionInstance.Topics[0] + "'";
                resultingTable = CreateTableFromQuery(query);

                questionsSortedByCategory[Convert.ToString(resultingTable.Rows[0][0])].Add(questionInstance);
            }

            return frequentlyAskedQuestions;
        }

        public List<string> GetAllCategories() {
            List<string> categories = new List<string>();

            string query = "SELECT DISTINCT categoria FROM Topico";
            DataTable resultingTable = CreateTableFromQuery(query);
            foreach (DataRow column in resultingTable.Rows) {
                categories.Add(Convert.ToString(column["categoria"]));
            }

            return categories;
        }

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