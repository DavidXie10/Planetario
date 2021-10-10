
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
            LinkAllEducationalMaterialWithEducationalActivity(educationalMaterials);
            return educationalMaterials;
        }
        
        private void LinkAllEducationalMaterialWithEducationalActivity(List<EducationalMaterialModel> educationalMaterials) {
            foreach (EducationalMaterialModel educationalMaterial in educationalMaterials) {
                DataTable resultingTableOfNewsWithTheirTopic = GetEducationalMaterialWithEducationalActivityTable(educationalMaterial.Title, educationalMaterial.Author);
                LinkEducationalMaterialWithEducationalActivity(educationalMaterial, resultingTableOfNewsWithTheirTopic);
                
            }
        }

        private DataTable GetEducationalMaterialWithEducationalActivityTable(string educationalMaterialTitle, string educationalMaterialAuthor) {
            string query = "SELECT tituloActividadPK, fechaInicioPK  FROM Ofrecer " +
                       "WHERE tituloMaterialPK = '" + educationalMaterialTitle + "' AND autorPK = '" + educationalMaterialAuthor + "'";

            return CreateTableFromQuery(query);
        }

        private void LinkEducationalMaterialWithEducationalActivity(EducationalMaterialModel educationalMaterial, DataTable resultingTable) {
            //educationalMaterial.ActivityTitle = Convert.ToString(resultingTable["tituloActividadPK"]);
        }

        private List<EducationalMaterialModel> CreateEducationalMaterialFromDataTable(DataTable resultingNewsTable)
        {
            List<EducationalMaterialModel> educationalMaterials = new List<EducationalMaterialModel>();
            foreach (DataRow educationalMaterialRawInfo in resultingNewsTable.Rows)
            {
                educationalMaterials.Add(CreateEducationalMaterial(educationalMaterialRawInfo));
            }
            return educationalMaterials;
        }

        private EducationalMaterialModel CreateEducationalMaterial(DataRow educationalMaterialRawInfo)
        {
            return new EducationalMaterialModel
            {
                Author = Convert.ToString(educationalMaterialRawInfo["autorPK"]),
                Title = Convert.ToString(educationalMaterialRawInfo["tituloPK"]),
                PublicationDate = Convert.ToDateTime(educationalMaterialRawInfo["fechaPublicacion"]),
            };
        }

        private void LinkAllEducationalMaterialWithTopics(List<EducationalMaterialModel> educationalMaterials)
        {
            foreach (EducationalMaterialModel educationalMaterial in educationalMaterials)
            {
                DataTable resultingTableOfNewsWithTheirTopic = GetEducationalMaterialWithTopicsTable(educationalMaterial.Title, educationalMaterial.Author);
                LinkEducationalMaterialWithTopics(educationalMaterial, resultingTableOfNewsWithTheirTopic);
            }
        }

        private DataTable GetEducationalMaterialWithTopicsTable(string educationalMaterialTitle, string educationalMaterialAuthor)
        {
            string query = "SELECT nombreTopicoPKFK FROM MaterialEducativo " +
                       "INNER JOIN MaterialEducativoPerteneceATopico ON(tituloPK = tituloMaterialEducativoPKFK " +
                       "AND autorPK = autorMaterialEducativoPKFK) " +
                       "WHERE tituloPK = '" + educationalMaterialTitle + "' AND autorPK = '" + educationalMaterialAuthor + "' " +
                       "ORDER BY fechaPublicacion DESC";

            return CreateTableFromQuery(query);
        }

        private void LinkEducationalMaterialWithTopics(EducationalMaterialModel educationalMaterial, DataTable resultingTable)
        {
            educationalMaterial.Topics = new List<string>();
            foreach (DataRow column in resultingTable.Rows)
            {
                var tempTopic = Convert.ToString(column["nombreTopicoPKFK"]);
                educationalMaterial.Topics.Add(tempTopic);
            }
        }

        private void LinkAllEducationalMaterialWithCategory(List<EducationalMaterialModel> educationalMaterials)
        {
            /*
            foreach (EducationalMaterialModel educationalMaterial in educationalMaterials) {
                DataTable resultingTableOfEducationalMaterialWithTheirCategory = GetEducationalMaterialWithCategoryTable(educationalMaterial.Topics[0]);
                LinkEducationalMaterialWithCategory(educationalMaterial, resultingTableOfEducationalMaterialWithTheirCategory);
            }
            */
            
        }

        private DataTable GetEducationalMaterialWithCategoryTable(string educationalMaterialTopic)
        {
            string query = "SELECT categoria FROM MaterialEducativoPerteneceATopico " +
                        "INNER JOIN Topico ON nombrePK = nombreTopicoPKFK  " +
                        "WHERE nombrePK = '" + educationalMaterialTopic + "'";
            return CreateTableFromQuery(query);
        }

        private void LinkEducationalMaterialWithCategory(EducationalMaterialModel educationalMaterial, DataTable resultingTable)
        {
            foreach (DataRow column in resultingTable.Rows)
            {
                educationalMaterial.Category = Convert.ToString(column["categoria"]);
            }
        }
        
        private DataTable GetEducationalMaterialWithKeywordsTable(string educationalMaterialTitle, string educationalMaterialAuthor)
        {
            string query = "SELECT palabraClave FROM MaterialEducativo ME " +
                        "INNER JOIN PalabraClaveMaterialEducativo PC ON(ME.tituloPK = PC.tituloPK " +
                        "AND ME.autorPK = PC.autorPK) " +
                        "WHERE ME.tituloPK = '" + educationalMaterialTitle + "' " +
                        "AND ME.autorPK = '" + educationalMaterialAuthor + "' " +
                        "ORDER BY fechaPublicacion DESC";
            return CreateTableFromQuery(query);
        }

        public bool InsertEducationalMaterial(EducationalMaterialModel educationalMaterial)
        {
            string educationalMaterialQuery = "INSERT INTO MaterialEducativo (tituloPK, autorPK, fechaPublicacion ) " +
                           "VALUES(@tituloPK,@autorPK, CAST( GETDATE() AS Date ))";
            SqlCommand queryCommand = new SqlCommand(educationalMaterialQuery, connection);

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

            success = InsertRelationshipWithEducationalActivity(educationalMaterial);

            return success;
        }

        private List<DateTime> getAllDates(EducationalMaterialModel educationalMaterial) {
            List<DateTime> dates = new List<DateTime>();
            string query = "SELECT fechaInicioPK FROM ActividadEducativa " +
                       "WHERE tituloPK = '" + educationalMaterial.ActivityTitle+ "'";

            DataTable table = CreateTableFromQuery(query);

            foreach (DataRow column in table.Rows) {
                dates.Add(Convert.ToDateTime(column["fechaInicioPK"]));
            }

            return dates;
        }

        private bool InsertRelationshipWithEducationalActivity(EducationalMaterialModel educationalMaterial) {
            List<DateTime> dates = getAllDates(educationalMaterial);
            bool success = false;
            foreach (DateTime date in dates) {
                string query = "INSERT INTO Ofrecer(cedulaPK, tituloActividadPK, fechaInicioPK, tituloMaterialPK, autorPK) " +
                            "VALUES('203250235', @tituloActividad, @fechaInicio, @tituloMaterial, @autor)";

                SqlCommand queryCommand = new SqlCommand(query, connection);
                queryCommand.Parameters.AddWithValue("@tituloActividad", educationalMaterial.ActivityTitle);
                queryCommand.Parameters.AddWithValue("@fechaInicio", date);
                queryCommand.Parameters.AddWithValue("@tituloMaterial", educationalMaterial.Title);
                queryCommand.Parameters.AddWithValue("@autor", educationalMaterial.Author);

                connection.Open();
                success = queryCommand.ExecuteNonQuery() >= 1;
                connection.Close();
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
            string query = "SELECT * FROM ActividadEducativa ";
            DataTable resultingTable = CreateTableFromQuery(query);
            foreach (DataRow column in resultingTable.Rows)
            {
                activitiesTitles.Add( Convert.ToString(column["tituloPK"]));                     
            }
            return activitiesTitles;
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

    }

}