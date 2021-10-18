using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Planetarium.Models;
using System.Linq;

namespace Planetarium.Handlers {
    public class NewsHandler : DatabaseClassificationsHandler {

        public List<NewsModel> GetAllNews() {
            string query = "SELECT DISTINCT N.tituloPK, N.resumen, N.fechaPublicacion, N.cedulaFK, N.contenido, N.autor, T.categoria " +
                           "FROM Noticia N " +
                           "INNER JOIN NoticiaPerteneceATopico NPAT ON NPAT.tituloPKFK = N.tituloPK " +
                           "INNER JOIN Topico T ON T.nombrePK = NPAT.nombreTopicoPKFK " +
                           "ORDER BY fechaPublicacion DESC ";
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
                Author = Convert.ToString(scoopRawInfo["autor"]),
                Category = Convert.ToString(scoopRawInfo["categoria"])
            };
        }

        override protected DataTable GetFeatureWithTopicsTable(string[] keys) {
            string query = "SELECT * FROM Noticia " +
                        "INNER JOIN NoticiaPerteneceATopico ON Noticia.tituloPK = NoticiaPerteneceATopico.tituloPKFK  " +
                        "WHERE tituloPK = '" + keys[0] + "' ";
            return CreateTableFromQuery(query);
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
            if (news.ImagesRef.Count > 0) {
                success = InsertImages(news);
            }
            return success;
        }
    }
}