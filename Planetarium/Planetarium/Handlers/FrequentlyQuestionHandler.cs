using System;
using System.Collections.Generic;
using Planetarium.Models;
using System.Data;
using System.Data.SqlClient;

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
                        QuestionId = Convert.ToInt32(column["idPK"])
                    }
                );
            }

            LinkAllFeatureWithTopics(CreateDictionary(frequentlyAskedQuestions));

            LinkAllQuestionsWithCategory(frequentlyAskedQuestions, questionsSortedByCategory);

            return frequentlyAskedQuestions;
        }

        private Dictionary<string[], List<string>> CreateDictionary(List<FrequentlyQuestionModel> frequentlyQuestionList) {
            Dictionary<string[], List<string>> tempDictionary = new Dictionary<string[], List<string>>();
            foreach (FrequentlyQuestionModel frequentlyQuestionInstance in frequentlyQuestionList) {
                tempDictionary.Add(new string[] { frequentlyQuestionInstance.QuestionId.ToString() }, frequentlyQuestionInstance.Topics = new List<string>());
            }
            return tempDictionary;
        }

        override protected DataTable GetFeatureWithTopicsTable(string[] keys) {
            string query = "SELECT * FROM PreguntaFrecuente PF " +
                        "INNER JOIN PreguntaFrecuentePerteneceATopico PFPAT ON PF.idPK = PFPAT.idPreguntaFrecuentePKFK  " +
                        "WHERE idPK = '" + keys[0] + "'";
            return CreateTableFromQuery(query);
        }

        private void LinkAllQuestionsWithCategory(List<FrequentlyQuestionModel> frequentQuestions, Dictionary<string, List<FrequentlyQuestionModel>> questionsSortedByCategory) {
            foreach (FrequentlyQuestionModel frequentQuestion in frequentQuestions) {
                DataTable resultingTableOfQuestionWithTheirCategory = GetQuestionWithCategoryTable(frequentQuestion.Topics[0]);
                SortQuestion(frequentQuestion, resultingTableOfQuestionWithTheirCategory, questionsSortedByCategory);
            }
        }

        private DataTable GetQuestionWithCategoryTable(string topic) {
            string query = "SELECT categoria FROM PreguntaFrecuentePerteneceATopico PFPAT " +
                        "INNER JOIN Topico T ON T.nombrePK = PFPAT.nombreTopicoPKFK  " +
                        "WHERE T.nombrePK = '" + topic + "'";
            return CreateTableFromQuery(query);
        }

        private void SortQuestion(FrequentlyQuestionModel question, DataTable resultingTable, Dictionary<string, List<FrequentlyQuestionModel>> questionsSortedByCategory) {
            questionsSortedByCategory[Convert.ToString(resultingTable.Rows[0][0])].Add(question);
        }
    }
}