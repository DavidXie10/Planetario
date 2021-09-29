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

            connection.Open();
            bool success = queryCommand.ExecuteNonQuery() >= 1;
            connection.Close();

            return success;
        }


        //create frequent question
        //poner atributos de frequent
    }
}