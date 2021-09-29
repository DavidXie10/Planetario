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
    public class FrequentlyQuestionHandler {

        private SqlConnection connection;
        private string connectionRoute;

        public FrequentlyQuestionHandler() {
            connectionRoute = ConfigurationManager.ConnectionStrings["PiConnection"].ToString();
            connection = new SqlConnection(connectionRoute);
        }

        public bool CreateFrequentlyAskedQuestion(FrequentlyQuestionModel faqQuestion) {
            string query = "INSERT INTO PreguntaFrecuente (pregunta, respuesta, cedulaFK) " +
                           "VALUES (@pregunta, @respuesta, 103230738)";
            SqlCommand queryCommand = new SqlCommand(query, connection);

            queryCommand.Parameters.AddWithValue("@pregunta", faqQuestion.Question);
            queryCommand.Parameters.AddWithValue("@respuesta", faqQuestion.Answer);

            //Insertar la pregunta
            //idPregunta = Select [last].id
            //for(topico en topicos){
            //  Insertar en PreguntaFrecuentePerteneceATopico(idPregunta, topico)
            //}

            connection.Open();
            bool success = queryCommand.ExecuteNonQuery() >= 1;
            connection.Close(); 

            return success;
        }

        private DataTable CreateTableFromQuery(string query) {
            SqlCommand queryCommand = new SqlCommand(query, connection);
            SqlDataAdapter tableAdapter = new SqlDataAdapter(queryCommand);
            DataTable queryTable = new DataTable();
            connection.Open();
            tableAdapter.Fill(queryTable);
            connection.Close();
            return queryTable;
        }

        public List<FrequentlyQuestionModel> GetAllQuestions() {
            List<FrequentlyQuestionModel> frequentlyAskedQuestions = new List<FrequentlyQuestionModel>();
            string query = "SELECT * FROM PreguntaFrecuente ";
            DataTable resultingTable = CreateTableFromQuery(query);
            foreach (DataRow column in resultingTable.Rows) {
                frequentlyAskedQuestions.Add(
                    new FrequentlyQuestionModel {
                        Question = Convert.ToString(column["pregunta"]),
                        Answer = Convert.ToString(column["respuesta"]),
                        QuestionId = Convert.ToString(column["idPreguntaFrecuentePKFK"])
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
                query = "SELECT * FROM NoticiaPerteneceATopico " +
                        "INNER JOIN Topico ON Topico.nombrePK = NoticiaPerteneceATopico.nombreTopicoPKFK  " +
                        "WHERE Topico.nombrePK = '" + questionInstance.Topics[0] + "'";
                resultingTable = CreateTableFromQuery(query);

                foreach (DataRow column in resultingTable.Rows) {
                    questionInstance.Category = Convert.ToString(column["categoria"]);
                }
            }

            return frequentlyAskedQuestions;
        }

        public List<string> GetAllCategories() {
            List<string> categories = new List<string>();

            string query = "SELECT DISTINCT nombreTopicoPKFK FROM PreguntaFrecuentePerteneceATopico ";
            DataTable resultingTable = CreateTableFromQuery(query);
            foreach (DataRow column in resultingTable.Rows) {
                categories.Add(Convert.ToString(column["nombreTopicoPKFK"]));
            }

            return categories;
        }
    }
}