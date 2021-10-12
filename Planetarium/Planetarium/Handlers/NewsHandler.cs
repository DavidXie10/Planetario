using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Planetarium.Models;

namespace Planetarium.Handlers {
    public class NewsHandler : DatabaseHandler {

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

        private bool InsertImages(NewsModel news) {
            bool success = false;

            foreach (string imageRef in news.ImagesRef) {
                
                string query = "INSERT INTO ImagenPerteneceANoticia " +
                        "VALUES ('" + news.Title + "','" + imageRef.Replace("_", "-").Replace(" ", "-") + "')";
                success = DatabaseQuery(query);
            }

            return success;
        }

        private bool InsertNewsTopics(NewsModel news) {
            bool success = false;

            foreach (string topic in news.Topics) {
                string query = "INSERT INTO NoticiaPerteneceATopico " +
                        "VALUES ('" + news.Title + "','" + topic + "')";
                success = DatabaseQuery(query);
            }

            return success;
        }

        public bool PublishNews(NewsModel news) {
            bool success = false;
            string query = "INSERT INTO Noticia (tituloPK, resumen, fechaPublicacion, cedulaFK, contenido, autor) " +
                           "VALUES(@tituloPK,@resumen, CAST( GETDATE() AS Date ) ,'202210135',@contenido,@autor)";
            SqlCommand queryCommand = new SqlCommand(query, connection);
            //TO-DO: Cambiar cedula quemada

            queryCommand.Parameters.AddWithValue("@tituloPK", news.Title);
            queryCommand.Parameters.AddWithValue("@resumen", news.Description);
            queryCommand.Parameters.AddWithValue("@contenido", news.Content);
            queryCommand.Parameters.AddWithValue("@autor", news.Author);

            success = DatabaseQuery(queryCommand);
            success = InsertNewsTopics(news);

            if (news.ImagesRef != null) {
                success = InsertImages(news);
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