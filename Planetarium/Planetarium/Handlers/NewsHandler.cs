using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using Planetarium.Models;

namespace Planetarium.Handlers
{
    public class NewsHandler
    {
        private SqlConnection connection;
        private string connectionRoute;

        public NewsHandler()
        {
            connectionRoute = ConfigurationManager.ConnectionStrings["AGREGAR EN WEB.CONF"].ToString();
            connection = new SqlConnection(connectionRoute);
        }

        private DataTable CreateTableFromQuery(string query)
        {
            SqlCommand queryCommand = new SqlCommand(query, connection);
            SqlDataAdapter tableAdapter = new SqlDataAdapter(queryCommand);
            DataTable queryTable = new DataTable();
            connection.Open();
            tableAdapter.Fill(queryTable);
            connection.Close();
            return queryTable;
        }

        public List<NewsModel> GetAllNews()
        {
            List<NewsModel> news = new List<NewsModel>();
            string query = "SELECT * FROM Noticia ORDER BY fechaDePublicacion DESC";
            DataTable resultingTable = CreateTableFromQuery(query);
            foreach (DataRow column in resultingTable.Rows)
            {
                news.Add(
                    new NewsModel
                    {
                        title = Convert.ToString(column["titulo"]),
                        category = Convert.ToString(column["categoria"]),
                        topic = Convert.ToString(column["topico"]),
                        date = Convert.ToString(column["fechaDePublicacion"]),
                        description = Convert.ToString(column["resumen"]),

                    }
                );
            }

            return news;
        }
    }
}