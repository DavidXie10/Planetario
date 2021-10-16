
using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Planetarium.Models;

namespace Planetarium.Handlers {
    public class EducationalMaterialHandler : DatabaseClassificationsHandler {


        public List<EducationalMaterialModel> GetAllEducationalMaterial() {
            string query = "SELECT DISTINCT ME.autorPK, ME.tituloPK, ME.fechaPublicacion, T.categoria " +
                           "FROM MaterialEducativo ME " +
                           "INNER JOIN MaterialEducativoPerteneceATopico MEPAT ON ME.autorPK = MEPAT.autorPKFK "  +
                           "AND ME.tituloPK = MEPAT.tituloPKFK " +
                           "INNER JOIN Topico T ON MEPAT.nombreTopicoPKFK = T.nombrePK " +
                           "ORDER BY fechaPublicacion DESC";

            DataTable resultingNewsTable = CreateTableFromQuery(query);
            List<EducationalMaterialModel> educationalMaterials = CreateEducationalMaterialFromDataTable(resultingNewsTable);
            LinkAllFeatureWithTopics(CreateDictionary(educationalMaterials));
            LinkAllEducationalMaterialWithEducationalActivity(educationalMaterials);
            LinkAllEducationalMaterialWithFileName(educationalMaterials);
            return educationalMaterials;
        }
        
        private void LinkAllEducationalMaterialWithEducationalActivity(List<EducationalMaterialModel> educationalMaterials) {
            foreach (EducationalMaterialModel educationalMaterial in educationalMaterials) {
                DataTable resultingTableOfEducationalMaterialWithActivity = GetEducationalMaterialWithEducationalActivityTable(educationalMaterial.Title, educationalMaterial.Author);
                LinkEducationalMaterialWithEducationalActivity(educationalMaterial, resultingTableOfEducationalMaterialWithActivity);
                
            }
        }

        private DataTable GetEducationalMaterialWithEducationalActivityTable(string educationalMaterialTitle, string educationalMaterialAuthor) {
            string query = "SELECT tituloActividadPKFK, fechaInicioPKFK  FROM Ofrecer " +
                           "WHERE tituloMaterialPKFK = '" + educationalMaterialTitle + "' AND autorPKFK = '" + educationalMaterialAuthor + "'";

            return CreateTableFromQuery(query);
        }

        private void LinkEducationalMaterialWithEducationalActivity(EducationalMaterialModel educationalMaterial, DataTable resultingTable) {
            educationalMaterial.ActivityTitle = Convert.ToString(resultingTable.Rows[0]["tituloActividadPKFK"]);
        }

        private List<EducationalMaterialModel> CreateEducationalMaterialFromDataTable(DataTable resultingNewsTable) {
            List<EducationalMaterialModel> educationalMaterials = new List<EducationalMaterialModel>();
            foreach (DataRow educationalMaterialRawInfo in resultingNewsTable.Rows) {
                educationalMaterials.Add(CreateEducationalMaterial(educationalMaterialRawInfo));
            }
            return educationalMaterials;
        }

        private EducationalMaterialModel CreateEducationalMaterial(DataRow educationalMaterialRawInfo) {
            return new EducationalMaterialModel {
                Author = Convert.ToString(educationalMaterialRawInfo["autorPK"]),
                Title = Convert.ToString(educationalMaterialRawInfo["tituloPK"]),
                PublicationDate = Convert.ToDateTime(educationalMaterialRawInfo["fechaPublicacion"]),
                Category = Convert.ToString(educationalMaterialRawInfo["categoria"])
            };
        }

        private void LinkAllEducationalMaterialWithFileName(List<EducationalMaterialModel> educationalMaterials) {
            foreach (EducationalMaterialModel educationalMaterial in educationalMaterials) {
                DataTable resultingTableOfEducationalMaterialWithTheirFileName = GetEducationalMaterialWithFileNameTable(educationalMaterial.Title, educationalMaterial.Author);
                LinkEducationalMaterialWithFileName(educationalMaterial, resultingTableOfEducationalMaterialWithTheirFileName);
            }
        }

        private DataTable GetEducationalMaterialWithFileNameTable(string educationalMaterialTitle, string educationalMaterialAuthor) {
            string query = "SELECT nombreArchivoPK FROM NombreArchivoMaterialEducativo " +
                           "WHERE tituloPKFK = '" + educationalMaterialTitle + "' AND autorPKFK = '" + educationalMaterialAuthor + "'";
            return CreateTableFromQuery(query);
        }

        private void LinkEducationalMaterialWithFileName(EducationalMaterialModel educationalMaterial, DataTable resultingTable) {
            educationalMaterial.EducationalMaterialFileNames = new List<string>();
            foreach (DataRow column in resultingTable.Rows) {
                var tempFileName = Convert.ToString(column["nombreArchivoPK"]);
                educationalMaterial.EducationalMaterialFileNames.Add(tempFileName);
            }
        }

        private Dictionary<string[], List<string>> CreateDictionary(List<EducationalMaterialModel> materials) {
            Dictionary<string[], List<string>> tempDictionary = new Dictionary<string[], List<string>>();
            foreach (EducationalMaterialModel materialInstance in materials) {
                tempDictionary.Add(new string[] { materialInstance.Author, materialInstance.Title }, materialInstance.Topics = new List<string>());
            }
            return tempDictionary;
        }

        override protected DataTable GetFeatureWithTopicsTable(string[] keys) {
            string query = "SELECT nombreTopicoPKFK FROM MaterialEducativo ME " +
                       "INNER JOIN MaterialEducativoPerteneceATopico MEPAT ON (ME.tituloPK = MEPAT.tituloPKFK " +
                       "AND ME.autorPK = MEPAT.autorPKFK) " +
                       "WHERE tituloPK = '" + keys[1] + "' AND autorPK = '" + keys[0] + "' ";

            return CreateTableFromQuery(query);
        }

        public bool InsertEducationalMaterial(EducationalMaterialModel educationalMaterial) {
            bool success = false;
            string educationalMaterialQuery = "INSERT INTO MaterialEducativo (tituloPK, autorPK, fechaPublicacion ) " +
                           "VALUES(@tituloPK,@autorPK, CAST( GETDATE() AS Date ))";
            SqlCommand queryCommand = new SqlCommand(educationalMaterialQuery, connection);

            queryCommand.Parameters.AddWithValue("@tituloPK", educationalMaterial.Title);
            queryCommand.Parameters.AddWithValue("@autorPK", educationalMaterial.Author);

            success = DatabaseQuery(queryCommand);

            success = InsertEducationalMaterialTopics(educationalMaterial);

            if (educationalMaterial.EducationalMaterialFileNames != null) {
                success = InsertEducationalMaterialFiles(educationalMaterial);
            }

            success = InsertRelationshipWithEducationalActivity(educationalMaterial);

            return success;
        }

        private List<DateTime> GetAllDates(EducationalMaterialModel educationalMaterial) {
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
            List<DateTime> dates = GetAllDates(educationalMaterial);
            bool success = false;
            foreach (DateTime date in dates) {
                string query = "INSERT INTO Ofrecer(cedulaPKFK, tituloActividadPKFK, fechaInicioPKFK, tituloMaterialPKFK, autorPKFK) " +
                            "VALUES('203250235', @tituloActividad, @fechaInicio, @tituloMaterial, @autor)";

                SqlCommand queryCommand = new SqlCommand(query, connection);
                queryCommand.Parameters.AddWithValue("@tituloActividad", educationalMaterial.ActivityTitle);
                queryCommand.Parameters.AddWithValue("@fechaInicio", date);
                queryCommand.Parameters.AddWithValue("@tituloMaterial", educationalMaterial.Title);
                queryCommand.Parameters.AddWithValue("@autor", educationalMaterial.Author);

                success = DatabaseQuery(queryCommand);
            }

            return success;
        }

        private bool InsertEducationalMaterialTopics(EducationalMaterialModel educationalMaterial) {
            bool success = false;

            foreach (string topic in educationalMaterial.Topics) {              
                string query = "INSERT INTO MaterialEducativoPerteneceATopico " +
                        "VALUES ('" + educationalMaterial.Author + "','" + educationalMaterial.Title + "','" + topic + "')";         
                success = DatabaseQuery(query);
            }
            return success;
        }

        private bool InsertEducationalMaterialFiles(EducationalMaterialModel educationalMaterial) {
            bool success = false;
            foreach (string file in educationalMaterial.EducationalMaterialFileNames) {
                string query = "INSERT INTO NombreArchivoMaterialEducativo " +
                        "VALUES ('" + educationalMaterial.Author + "','" + educationalMaterial.Title + "','" + file + "')";
                success = DatabaseQuery(query);
            }
            return success;
        }

        public List<string> GetAllActivities() {
            List<string> activitiesTitles = new List<string>();
            string query = "SELECT * FROM ActividadEducativa WHERE estadoRevision = 1";
            DataTable resultingTable = CreateTableFromQuery(query);
            foreach (DataRow column in resultingTable.Rows) {
                activitiesTitles.Add( Convert.ToString(column["tituloPK"]));                     
            }
            return activitiesTitles;
        }
    }

}