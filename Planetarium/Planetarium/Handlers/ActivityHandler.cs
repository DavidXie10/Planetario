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
            connectionRoute = ConfigurationManager.ConnectionStrings["AGREGAR EN WEB.CONF"].ToString();
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
            List<ActivityModel> activities = new List<ActivityModel>();
            string query = "SELECT * FROM Activity ";
            DataTable resultingTable = CreateTableFromQuery(query);
            foreach (DataRow column in resultingTable.Rows) {
                activities.Add(
                    new ActivityModel {
                        title = Convert.ToString(column["titulo"]),
                        body = Convert.ToString(column["cuerpo"]),
                        id = Convert.ToInt32(column["id"]),
                        img = Convert.ToInt32(column["img"]),
                        date = Convert.ToString(column['date'])
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