using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Planetarium.Models;

namespace Planetarium.Handlers {
    public class EducationalActivityHandler {
        private SqlConnection connection;
        private string connectionRoute;

        public EducationalActivityHandler() {
            connectionRoute = ConfigurationManager.ConnectionStrings["PlanetariumConnection"].ToString();
            connection = new SqlConnection(connectionRoute);
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

        public bool ProposeEducationalActivity(EducationalActivityModel educationalActivity) {
            string query = "INSERT INTO ActividadEducativa (tituloPK, fechaInicioPK, duracion, capacidadMaxima, precio, " +
                           "descripcion, nivelComplejidad, estado, tipo, banderaVirtual, enlace, banderaPresencial, cedulaFK) " +
                           "VALUES(@tituloPK,@fechaInicioPK,@duracion,@capacidadMaxima,@precio,@descripcion,@nivelComplejidad," +
                           "0,@tipo,@banderaVirtual,@enlace,@banderaPresencial,'106260895')";
            SqlCommand queryCommand = new SqlCommand(query, connection);
            
            //TO-DO: Cambiar cedula quemada

            queryCommand.Parameters.AddWithValue("@tituloPK", educationalActivity.Title);
            queryCommand.Parameters.AddWithValue("@fechaInicioPK", educationalActivity.Date);
            queryCommand.Parameters.AddWithValue("@duracion", educationalActivity.Duration);
            queryCommand.Parameters.AddWithValue("@capacidadMaxima", educationalActivity.MaximumCapacity);
            queryCommand.Parameters.AddWithValue("@precio", educationalActivity.Price);
            queryCommand.Parameters.AddWithValue("@descripcion", educationalActivity.Description);
            queryCommand.Parameters.AddWithValue("@nivelComplejidad", educationalActivity.ComplexityLevel);
            queryCommand.Parameters.AddWithValue("@tipo", educationalActivity.TypeOfAssistance);

            // TODO: cambiar a enumeracion o constantes
            // 0 virtual
            int virtualFlag = educationalActivity.TypeOfAssistance == "Virtual" ? 1 : 0;
            int onSiteFlag = educationalActivity.TypeOfAssistance == "Presencial" ? 0 : 1;
            if (virtualFlag == 1) {
                queryCommand.Parameters.AddWithValue("@enlace", educationalActivity.Link);
            }
            queryCommand.Parameters.AddWithValue("@banderaVirtual", virtualFlag);
            queryCommand.Parameters.AddWithValue("@banderaPresencial", onSiteFlag);

            connection.Open();
            bool success = queryCommand.ExecuteNonQuery() >= 1;
            connection.Close();

            return success;
        }



    }
}