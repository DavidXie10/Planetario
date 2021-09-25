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
            connectionRoute = ConfigurationManager.ConnectionStrings["AGREGAR EN WEB.CONF"].ToString();
            connection = new SqlConnection(connectionRoute);
        }

        public bool CreateFrequentlyAskedQuestion(FrequentlyQuestionModel question) {
            string query = "INSERT INTO PreguntaFrecuente (pregunta, respuesta, topico, categoria) " +
                           "VALUES (@pregunta,@respuesta,@topico,@categoria";
            SqlCommand queryCommand = new SqlCommand(query, connection);

            queryCommand.Parameters.AddWithValue("@pregunta", question.Question);
            queryCommand.Parameters.AddWithValue("@respuesta", question.Answer);
            queryCommand.Parameters.AddWithValue("@topico", question.Topic);
            queryCommand.Parameters.AddWithValue("@categoria", question.Category);

            connection.Open();
            bool success = queryCommand.ExecuteNonQuery() >= 1;
            connection.Close();

            return success;
        }


        //create frequent question
        //poner atributos de frequent
    }
}