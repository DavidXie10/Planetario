
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
    public class EducationalMaterialHandler
    {
        private SqlConnection connection;
        private string connectionRoute;

        public EducationalMaterialHandler()
        {
            connectionRoute = ConfigurationManager.ConnectionStrings["PlanetariumConnection"].ToString();
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

        public List<EducationalMaterialModel> GetAllEducationalMaterial()
        {
            string query = "SELECT * FROM MaterialEducativo " +
                           "ORDER BY fechaPublicacion DESC ";
            DataTable resultingNewsTable = CreateTableFromQuery(query);
            List<EducationalMaterialModel> educationalMaterials = CreateEducationalMaterialFromDataTable(resultingNewsTable);
            LinkAllEducationalMaterialWithTopics(educationalMaterials);
            LinkAllEducationalMaterialWithCategory(educationalMaterials);
            return educationalMaterials;
        }

        private List<EducationalMaterialModel> CreateEducationalMaterialFromDataTable(DataTable resultingNewsTable)
        {
            List<EducationalMaterialModel> educationalMaterials = new List<EducationalMaterialModel>();
            foreach (DataRow scoopRawInfo in resultingNewsTable.Rows)
            {
                educationalMaterials.Add(CreateScoop(scoopRawInfo));
            }
            return educationalMaterials;
        }

        private EducationalMaterialModel CreateScoop(DataRow scoopRawInfo)
        {
            return new EducationalMaterialModel
            {
                Author = Convert.ToString(scoopRawInfo["autorPK"]),
                Title = Convert.ToString(scoopRawInfo["tituloPK"]),
                Date = Convert.ToDateTime(scoopRawInfo["fechaPublicacion"]),

            };
        }

        private void LinkAllEducationalMaterialWithTopics(List<EducationalMaterialModel> educationalMaterials)
        {
            foreach (EducationalMaterialModel scoop in educationalMaterials)
            {
                DataTable resultingTableOfNewsWithTheirTopic = GetEducationalMaterialWithTopicsTable(scoop.Title, scoop.Author);
                LinkScoopWithTopics(scoop, resultingTableOfNewsWithTheirTopic);
            }
        }

        private DataTable GetEducationalMaterialWithTopicsTable(string scoopTitle, string scoopAuthor)
        {
            string query = "SELECT nombreTopicoPKFK FROM MaterialEducativo " +
                       "INNER JOIN MaterialEducativoPerteneceATopico ON(tituloPK = tituloMaterialEducativoPKFK " +
                       "AND autorPK = autorMaterialEducativoPKFK) " +
                       "WHERE tituloPK = '" + scoopTitle + "' AND autorPK = '" + scoopAuthor + "' " +
                       "ORDER BY fechaPublicacion DESC";

            return CreateTableFromQuery(query);
        }

        private void LinkScoopWithTopics(EducationalMaterialModel scoop, DataTable resultingTable)
        {
            scoop.Topics = new List<string>();
            foreach (DataRow column in resultingTable.Rows)
            {
                var tempTopic = Convert.ToString(column["nombreTopicoPKFK"]);
                scoop.Topics.Add(tempTopic);
            }
        }

        private void LinkAllEducationalMaterialWithCategory(List<EducationalMaterialModel> educationalMaterials)
        {
            foreach (EducationalMaterialModel scoop in educationalMaterials)
            {
                DataTable resultingTableOfEducationalMaterialWithTheirCategory = GetEducationalMaterialWithCategoryTable(scoop.Topics[0]);
                LinkScoopWithCategory(scoop, resultingTableOfEducationalMaterialWithTheirCategory);
            }
        }

        private DataTable GetEducationalMaterialWithCategoryTable(string scoopTopic)
        {
            string query = "SELECT categoria FROM MaterialEducativoPerteneceATopico " +
                        "INNER JOIN Topico ON nombrePK = nombreTopicoPKFK  " +
                        "WHERE nombrePK = '" + scoopTopic + "'";
            return CreateTableFromQuery(query);
        }

        private void LinkScoopWithCategory(EducationalMaterialModel scoop, DataTable resultingTable)
        {
            foreach (DataRow column in resultingTable.Rows)
            {
                scoop.Category = Convert.ToString(column["categoria"]);
            }
        }
        
        private DataTable GetEducationalMaterialWithKeywordsTable(string scoopTitle, string scoopAuthor)
        {
            string query = "SELECT palabraClave FROM MaterialEducativo ME " +
                        "INNER JOIN PalabraClaveMaterialEducativo PC ON(ME.tituloPK = PC.tituloPK " +
                        "AND ME.autorPK = PC.autorPK) " +
                        "WHERE ME.tituloPK = '" + scoopTitle + "' " +
                        "AND ME.autorPK = '" + scoopAuthor + "' " +
                        "ORDER BY fechaPublicacion DESC";
            return CreateTableFromQuery(query);
        }

        public bool InsertEducationalMaterial(EducationalMaterialModel educationalMaterial)
        {
            string query = "INSERT INTO MaterialEducativo (tituloPK, autorPK, fechaPublicacion ) " +
                           "VALUES(@tituloPK,@autorPK, CAST( GETDATE() AS Date ))";
            SqlCommand queryCommand = new SqlCommand(query, connection);

            queryCommand.Parameters.AddWithValue("@tituloPK", educationalMaterial.Title);
            queryCommand.Parameters.AddWithValue("@autorPK", educationalMaterial.Author);

            connection.Open();
            bool success = queryCommand.ExecuteNonQuery() >= 1;
            connection.Close();

            success = InsertEducationalMaterialTopics(educationalMaterial);

            if (educationalMaterial.EducationalMaterialFileNames != null)
            {
                success = InsertEducationalMaterialFiles(educationalMaterial);
            }

            return success;
        }

        private bool InsertEducationalMaterialTopics(EducationalMaterialModel educationalMaterial)
        {
            bool success = false;

            foreach (string topic in educationalMaterial.Topics)
            {
                string query = "INSERT INTO MaterialEducativoPerteneceATopico " +
                        "VALUES ('" + educationalMaterial.Author + "','" + educationalMaterial.Title + "','" + topic + "')";
                SqlCommand queryCommand = new SqlCommand(query, connection);
                connection.Open();
                success = queryCommand.ExecuteNonQuery() >= 1;
                connection.Close();
            }

            return success;
        }

        private bool InsertEducationalMaterialFiles(EducationalMaterialModel educationalMaterial)
        {
            bool success = false;

            foreach (string file in educationalMaterial.EducationalMaterialFileNames)
            {
                string query = "INSERT INTO ArchivoDeMaterialEducativo " +
                        "VALUES ('" + educationalMaterial.Author + "','" + educationalMaterial.Title + "','" + file + "')";
                SqlCommand queryCommand = new SqlCommand(query, connection);
                connection.Open();
                success = queryCommand.ExecuteNonQuery() >= 1;
                connection.Close();
            }

            return success;
        }

        public List<string> GetTopicsByCategory(string category)
        {
            List<string> topics = new List<string>();

            string query = "SELECT nombrePK " +
                            "FROM Topico T " +
                            "WHERE T.categoria LIKE '%" + category + "%';";

            DataTable topicsDataTable = CreateTableFromQuery(query);

            foreach (DataRow column in topicsDataTable.Rows)
            {
                topics.Add(Convert.ToString(column["nombrePK"]));
            }

            return topics;
        }

        public List<string> GetAllActivities()
        {
            List<string> activitiesTitles = new List<string>();
            string query = "SELECT * FROM Activity ";
            DataTable resultingTable = CreateTableFromQuery(query);
            foreach (DataRow column in resultingTable.Rows)
            {
                activitiesTitles.Add( Convert.ToString(column["titulo"]));                     
            }
            return activitiesTitles;
        }


    }

}