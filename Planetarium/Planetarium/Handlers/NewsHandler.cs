using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Planetarium.Models;

namespace Planetarium.Handlers {
    public class NewsHandler {

        private SqlConnection connection;
        private string connectionRoute;

        public NewsHandler() {
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

        public List<NewsModel> GetAllNews() {
            string query = "SELECT * FROM Noticia " +
                           "ORDER BY fechaPublicacion DESC ";
            DataTable resultingNewsTable = CreateTableFromQuery(query);
            List<NewsModel> news = CreateNewsFromDataTable(resultingNewsTable);
            LinkAllNewsWithTopics(news);
            LinkAllNewsWithCategory(news);
            LinkAllNewsWithImages(news);
            return news;
        }

        private List<NewsModel> CreateNewsFromDataTable(DataTable resultingNewsTable) {
            List<NewsModel> news = new List<NewsModel>();
            foreach (DataRow scoopRawInfo in resultingNewsTable.Rows) {
                news.Add(InstanceScoop(scoopRawInfo));
            }
            return news;
        }

        private NewsModel InstanceScoop(DataRow scoopRawInfo) {
            return new NewsModel {
                Title = Convert.ToString(scoopRawInfo["tituloPK"]),
                Date = Convert.ToDateTime(scoopRawInfo["fechaPublicacion"]),
                Content = Convert.ToString(scoopRawInfo["contenido"]),
                PublisherId = Convert.ToString(scoopRawInfo["cedulaFK"]),
                Description = Convert.ToString(scoopRawInfo["resumen"]),
                Author = Convert.ToString(scoopRawInfo["autor"]),
            };
        }

        private void LinkAllNewsWithTopics(List<NewsModel> news) {
            foreach (NewsModel scoop in news) {
                DataTable resultingTableOfNewsWithTheirTopic = GetNewsWithTopicsTable(scoop.Title);
                LinkScoopWithTopics(scoop, resultingTableOfNewsWithTheirTopic);
            }
        }

        private DataTable GetNewsWithTopicsTable (string scoopTitle) {
            string query = "SELECT * FROM Noticia " +
                        "INNER JOIN NoticiaPerteneceATopico ON Noticia.tituloPK = NoticiaPerteneceATopico.tituloPKFK  " +
                        "WHERE tituloPK = '" + scoopTitle + "' " +
                        "ORDER BY fechaPublicacion DESC";
            return CreateTableFromQuery(query);
        }

        private void LinkScoopWithTopics(NewsModel scoop, DataTable resultingTable) {
            scoop.Topics = new List<string>();
            foreach (DataRow column in resultingTable.Rows) {
                var tempTopic = Convert.ToString(column["nombreTopicoPKFK"]);
                scoop.Topics.Add(tempTopic);
            }
        }

        private void LinkAllNewsWithCategory(List<NewsModel> news) {
            foreach (NewsModel scoop in news) {
                DataTable resultingTableOfNewsWithTheirCategory = GetNewsWithCategoryTable(scoop.Topics[0]);
                LinkScoopWithCategory(scoop, resultingTableOfNewsWithTheirCategory);
            }
        }

        private DataTable GetNewsWithCategoryTable(string scoopTopic) {
            string query = "SELECT * FROM NoticiaPerteneceATopico " +
                        "INNER JOIN Topico ON Topico.nombrePK = NoticiaPerteneceATopico.nombreTopicoPKFK  " +
                        "WHERE Topico.nombrePK = '" + scoopTopic + "'";
            return CreateTableFromQuery(query);
        }

        private void LinkScoopWithCategory(NewsModel scoop, DataTable resultingTable) {
            foreach (DataRow column in resultingTable.Rows) {
                scoop.Category = Convert.ToString(column["categoria"]);
            }
        }

        private void LinkAllNewsWithImages(List<NewsModel> news) {
            foreach (NewsModel scoop in news) {
                DataTable resultingTableOfNewsWithTheirImages = GetNewsWithImagesTable(scoop.Title);
                if (resultingTableOfNewsWithTheirImages != null)
                    LinkScoopWithImages(scoop, resultingTableOfNewsWithTheirImages);
            }
        }
        private DataTable GetNewsWithImagesTable(string scoopTitle) {
            string query = "SELECT * FROM Noticia " +
                        "INNER JOIN ImagenPerteneceANoticia ON Noticia.tituloPK = ImagenPerteneceANoticia.tituloPKFK  " +
                        "WHERE tituloPK = '" + scoopTitle + "' ";
            return CreateTableFromQuery(query);
        }

        private void LinkScoopWithImages(NewsModel scoop, DataTable resultingTable) {
            scoop.ImagesRef = new List<string>();
            foreach (DataRow column in resultingTable.Rows) {
                var tempImageRef = Convert.ToString(column["referenciaImagenPK"]);
                scoop.ImagesRef.Add(tempImageRef);
            }
        }

        private byte[] GetFileBytes(HttpPostedFileBase file) {
            byte[] bytes;
            BinaryReader reader = new BinaryReader(file.InputStream);
            bytes = reader.ReadBytes(file.ContentLength);
            return bytes;
        }

        public bool PublishNews(NewsModel news) {
            string query = "INSERT INTO Noticia (tituloPK, resumen, fechaPublicacion, cedulaFK, contenido, autor) " +
                           "VALUES(@tituloPK,@resumen, CAST( GETDATE() AS Date ) ,'103230738',@contenido,@autor)";
            SqlCommand queryCommand = new SqlCommand(query, connection);
            //TO-DO: Cambiar cedula quemada

            queryCommand.Parameters.AddWithValue("@tituloPK", news.Title);
            queryCommand.Parameters.AddWithValue("@resumen", news.Description);
            queryCommand.Parameters.AddWithValue("@contenido", news.Content);
            queryCommand.Parameters.AddWithValue("@autor", news.Author);

            if (news.Topics.Count == 0 || news.Title.Equals("")) {
                return false;
            }

            connection.Open();
            bool success = queryCommand.ExecuteNonQuery() >= 1;
            connection.Close();

            if (!success) {
                return success;
            }

            foreach (string topic in news.Topics) {
                query = "INSERT INTO NoticiaPerteneceATopico " +
                        "VALUES ('" + news.Title + "','" + topic + "')";
                queryCommand = new SqlCommand(query, connection);
                connection.Open();
                success = queryCommand.ExecuteNonQuery() >= 1;
                connection.Close();
            }

            if (news.ImagesRef != null) {
                foreach (string imageRef in news.ImagesRef) {
                    query = "INSERT INTO ImagenPerteneceANoticia " +
                            "VALUES ('" + news.Title + "','" + imageRef + "')";
                    queryCommand = new SqlCommand(query, connection);
                    connection.Open();
                    success = queryCommand.ExecuteNonQuery() >= 1;
                    connection.Close();
                }
            }
            return success;
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

    }
}