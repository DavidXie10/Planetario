using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
                        Title = Convert.ToString(column["titulo"]),
                        Category = Convert.ToString(column["categoria"]),
                        Topic = Convert.ToString(column["topico"]),
                        Date = Convert.ToString(column["fechaDePublicacion"]),
                        Description = Convert.ToString(column["resumen"])
                    }
                );
            }

            return news;
        }

        private byte[] GetFileBytes(HttpPostedFileBase file)
        {
            byte[] bytes;
            BinaryReader reader = new BinaryReader(file.InputStream);
            bytes = reader.ReadBytes(file.ContentLength);
            return bytes;
        }
    }
}