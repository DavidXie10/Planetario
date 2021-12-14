using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Planetarium.Models;

namespace Planetarium.Handlers {
    public class NewsHandler : DatabaseClassificationsHandler {
        public List<NewsModel> GetAllNews() {
            string query = "SELECT DISTINCT N.tituloPK, N.resumen, N.fechaPublicacion, N.cedulaFK, N.contenido, T.categoria " +
                           "FROM Noticia N " +
                           "INNER JOIN NoticiaPerteneceATopico NPAT ON NPAT.tituloNoticiaPKFK = N.tituloPK " +
                           "INNER JOIN Topico T ON T.nombrePK = NPAT.nombreTopicoPKFK " +
                           "ORDER BY N.fechaPublicacion DESC ";
            DataTable resultingNewsTable = CreateTableFromQuery(query);
            List<NewsModel> news = CreateNewsFromDataTable(resultingNewsTable);
            LinkAllFeatureWithTopics(CreateDictionary(news));
            LinkAllNewsWithImages(news);
            return news;
        }

        private Dictionary<string[], List<string>> CreateDictionary (List<NewsModel> news) {
            Dictionary<string[], List<string>> tempDictionary = new Dictionary<string[], List<string>>();
            foreach (NewsModel newsInstance in news) {
                tempDictionary.Add(new string[] { newsInstance.Title }, newsInstance.Topics = new List<string>());
            }
            return tempDictionary;
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
                Category = Convert.ToString(scoopRawInfo["categoria"])
            };
        }

        override protected DataTable GetFeatureWithTopicsTable(string[] keys) {
            string query = "SELECT * FROM Noticia " +
                        "INNER JOIN NoticiaPerteneceATopico ON Noticia.tituloPK = NoticiaPerteneceATopico.tituloNoticiaPKFK  " +
                        "WHERE tituloPK = '" + keys[0] + "' ";
            return CreateTableFromQuery(query);
        }

        private void LinkAllNewsWithImages(List<NewsModel> news) {
            foreach (NewsModel scoop in news) {
                DataTable newsImages = GetNewsWithImagesTable(scoop.Title);
                if (newsImages != null)
                    LinkScoopWithImages(scoop, newsImages);
            }
        }

        private DataTable GetNewsWithImagesTable(string scoopTitle) {
            string query = "SELECT rutasImagenes FROM Noticia " +
                        "WHERE tituloPK = '" + scoopTitle + "' ";
            return CreateTableFromQuery(query);
        }

        private void LinkScoopWithImages(NewsModel scoop, DataTable newsImages) {
            scoop.ImagesRef = ContentParser.GetListFromString(Convert.ToString(newsImages.Rows[0]["rutasImagenes"]));
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
            string query = "INSERT INTO Noticia (tituloPK, resumen, fechaPublicacion, cedulaFK, contenido, rutasImagenes)" +
                           "VALUES(@tituloPK,@resumen, CAST( GETDATE() AS Date ),'202210135',@contenido,@rutasImagenes)";
            SqlCommand queryCommand = new SqlCommand(query, connection);
            //TO-DO: Cambiar cedula quemada
            string imagesRef = "";

            if (news.ImagesRef.Count > 0) {
                imagesRef = ContentParser.GetStringFromList(news.ImagesRef);
            }

            queryCommand.Parameters.AddWithValue("@rutasImagenes", imagesRef);
            queryCommand.Parameters.AddWithValue("@tituloPK", news.Title);
            queryCommand.Parameters.AddWithValue("@resumen", news.Description);
            queryCommand.Parameters.AddWithValue("@contenido", news.Content);
            success = DatabaseQuery(queryCommand);
            success = InsertNewsTopics(news);

            return success;
        }
    }
}