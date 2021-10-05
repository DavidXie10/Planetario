using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using Planetarium.Models;

namespace Planetarium.Handlers {

    public class ActivityHandler {

        private SqlConnection connection;
        private string connectionRoute;

        public ActivityHandler() {
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

        public List<ActivityModel> GetAllActivities() {
            string query = "SELECT * FROM ActividadEducativa";
            DataTable resultingTable = CreateTableFromQuery(query);
            List<ActivityModel> activities = new List<ActivityModel>();
            foreach (DataRow column in resultingTable.Rows) {
                activities.Add(
                    new ActivityModel {
                        Title = Convert.ToString(column["tituloPK"]),
                        Description = Convert.ToString(column["descripcion"]),
                        Date = Convert.ToDateTime(column["fechaInicioPK"]),
                        Duration = Convert.ToInt32(column["duracion"]),
                        Capacity = Convert.ToString(column["capacidadMaxima"]),
                        Price = Convert.ToInt32(column["precio"]),
                        Complexity = Convert.ToString(column["nivelComplejidad"]),
                        State = Convert.ToInt32(column["estado"]),
                        Type = Convert.ToString(column["tipo"]),
                        Link = Convert.ToString(column["enlace"]),
                        PublisherId = Convert.ToString(column["cedulaFK"]),
                    }
                );
            }
            return activities;
        }

        private byte[] GetFileBytes(HttpPostedFileBase file) {
            byte[] bytes;
            BinaryReader reader = new BinaryReader(file.InputStream);
            bytes = reader.ReadBytes(file.ContentLength);
            return bytes;
        }
    }
}