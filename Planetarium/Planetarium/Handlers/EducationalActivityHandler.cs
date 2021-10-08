using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Planetarium.Models;

namespace Planetarium.Handlers {
    public class EducationalActivityHandler {
        private SqlConnection connection;
        private string connectionRoute;

        public EducationalActivityHandler() {
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

        public bool ProposeEducationalActivity(EducationalActivityModel educationalActivity) {
            string query = "INSERT INTO ActividadEducativa (tituloPK, fechaInicioPK, duracion, capacidadMaxima, precio, descripcion, nivelComplejidad, estado, tipo, banderaVirtual, enlace, banderaPresencial, cedulaFK) " +
                           "VALUES(@tituloPK,@fechaInicioPK,@duracion,@capacidadMaxima,@precio,@descripcion,@nivelComplejidad,0,@tipo,@banderaVirtual,@enlace,@banderaPresencial,'106260895') ";
            SqlCommand queryCommand = new SqlCommand(query, connection);

            //TO-DO: Cambiar cedula quemada

            AddParametersToQueryCommand(queryCommand, educationalActivity);

            connection.Open();
            bool success = queryCommand.ExecuteNonQuery() >= 1;
            connection.Close();

            success = InsertActivitiesTopics(educationalActivity);
            success = InsertActivitiesAudiences(educationalActivity);

            return success;
        }

        private void AddParametersToQueryCommand(SqlCommand queryCommand, EducationalActivityModel educationalActivity) {
            queryCommand.Parameters.AddWithValue("@tituloPK", educationalActivity.Title);
            queryCommand.Parameters.AddWithValue("@fechaInicioPK", educationalActivity.Date);
            queryCommand.Parameters.AddWithValue("@duracion", educationalActivity.Duration);
            queryCommand.Parameters.AddWithValue("@capacidadMaxima", educationalActivity.MaximumCapacity);
            queryCommand.Parameters.AddWithValue("@precio", educationalActivity.Price);
            queryCommand.Parameters.AddWithValue("@descripcion", educationalActivity.Description);
            queryCommand.Parameters.AddWithValue("@nivelComplejidad", educationalActivity.ComplexityLevel);
            queryCommand.Parameters.AddWithValue("@tipo", educationalActivity.ActivityType);
            queryCommand.Parameters.AddWithValue("@enlace", educationalActivity.Link);

            int virtualFlag = educationalActivity.TypeOfAssistance == "Virtual" ? 1 : 0;
            int onSiteFlag = educationalActivity.TypeOfAssistance == "Virtual" ? 0 : 1;

            queryCommand.Parameters.AddWithValue("@banderaVirtual", virtualFlag);
            queryCommand.Parameters.AddWithValue("@banderaPresencial", onSiteFlag);
        }

        private bool InsertActivitiesTopics(EducationalActivityModel educationalActivity) {
            bool success = false;

            foreach (string topic in educationalActivity.Topics) {
                string query = "INSERT INTO ActividadEducativaPerteneceATopico " +
                        "VALUES ('" + educationalActivity.Title + "','" + educationalActivity.Date + "','" + topic + "')";
                SqlCommand queryCommand = new SqlCommand(query, connection);
                connection.Open();
                success = queryCommand.ExecuteNonQuery() >= 1;
                connection.Close();
            }

            return success;
        }

        private bool InsertActivitiesAudiences(EducationalActivityModel educationalActivity) {
            bool success = false;

            foreach (string audience in educationalActivity.TargetAudience) {
                string query = "INSERT INTO PublicoMeta " +
                        "VALUES ('" + educationalActivity.Title + "','" + educationalActivity.Date + "','" + audience + "')";
                SqlCommand queryCommand = new SqlCommand(query, connection);
                connection.Open();
                success = queryCommand.ExecuteNonQuery() >= 1;
                connection.Close();
            }

            return success;
        }

        public List<EducationalActivityModel> GetAllActivities() {
            string query = "SELECT DISTINCT F.nombre+ ' ' + F.apellido 'publicador',"
                            + " AE.tituloPK,"
                            + " AE.descripcion,"
                            + " AE.fechaInicioPK,"
                            + " AE.duracion,"
                            + " AE.capacidadMaxima,"
                            + " AE.precio,"
                            + " AE.nivelComplejidad,"
                            + " AE.estado,"
                            + " AE.tipo,"
                            + " AE.enlace,"
                            + " AE.banderaVirtual,"
                            + " T.categoria"
                            + " FROM Funcionario F  JOIN ActividadEducativa AE"
                            + " ON F.cedulaPK  = AE.cedulaFK "
                            + " JOIN ActividadEducativaPerteneceATopico AEPT ON AE.tituloPK = AEPT.tituloPKFK"
                            + " JOIN Topico T ON AEPT.nombreTopicoPKFK = T.nombrePK"
                            + " JOIN Idioma I ON I.cedulaPK = AE.cedulaFK ORDER BY AE.fechaInicioPK ";
            DataTable resultingTable = CreateTableFromQuery(query);
            List<EducationalActivityModel> activities = new List<EducationalActivityModel>();

            foreach (DataRow rawEducationalInfo in resultingTable.Rows) {
                activities.Add(CreateInstanceEducationalActivity(rawEducationalInfo));
            }
            LinkAllTargetAudience(activities);
            LinkAllTopics(activities);

            return activities;
        }

        private EducationalActivityModel CreateInstanceEducationalActivity(DataRow rawEducationalInfo) {
            return new EducationalActivityModel {
                Title = Convert.ToString(rawEducationalInfo["tituloPK"]),
                Description = Convert.ToString(rawEducationalInfo["descripcion"]),
                Date = Convert.ToDateTime(rawEducationalInfo["fechaInicioPK"]),
                Duration = Convert.ToInt32(rawEducationalInfo["duracion"]),
                MaximumCapacity = Convert.ToInt32(rawEducationalInfo["capacidadMaxima"]),
                Price = Convert.ToInt32(rawEducationalInfo["precio"]),
                ComplexityLevel = Convert.ToString(rawEducationalInfo["nivelComplejidad"]),
                State = Convert.ToString(rawEducationalInfo["estado"]),
                TypeOfAssistance = (Convert.ToInt32(rawEducationalInfo["banderaVirtual"]) == 1) ? "Virtual" : "Presencial",
                ActivityType = Convert.ToString(rawEducationalInfo["tipo"]),
                Link = Convert.ToString(rawEducationalInfo["enlace"]),
                Publisher = Convert.ToString(rawEducationalInfo["publicador"]),
                Category = Convert.ToString(rawEducationalInfo["categoria"])
            };
        }

        private void LinkAllTargetAudience(List<EducationalActivityModel> activities) {
            foreach (EducationalActivityModel activity in activities) {
                DataTable resultingTableWithTargetAudience = GetTargetAudiencePerEducationalActivity(activity.Title, Convert.ToString(activity.Date));
                LinkEducationalActivityWithTargetAudience(activity, resultingTableWithTargetAudience);
            }
        }

        private DataTable GetTargetAudiencePerEducationalActivity(string activityTitle, string initialDate) {
            string query = "SELECT publicoMetaPK " +
                "FROM PublicoMeta INNER JOIN ActividadEducativa" +
                " ON PublicoMeta.fechaInicioPK = ActividadEducativa.fechaInicioPK AND PublicoMeta.tituloPK = ActividadEducativa.tituloPK " +
                "WHERE PublicoMeta.tituloPK = '" + activityTitle + "'" + " AND PublicoMeta.fechaInicioPK = " + "'" + initialDate + "';";
            return CreateTableFromQuery(query);
        }

        private void LinkAllTopics(List<EducationalActivityModel> activities) {
            foreach (EducationalActivityModel activity in activities) {
                DataTable resultingTableWithTheirTopic = GetTopicsPerActivityEducationalActivity(activity.Title);
                LinkActivityWithTopics(activity, resultingTableWithTheirTopic);
            }
        }

        private DataTable GetTopicsPerActivityEducationalActivity(string activityTitle) {
            string query = "SELECT nombreTopicoPKFK FROM ActividadEducativa INNER JOIN ActividadEducativaPerteneceATopico ON ActividadEducativa.tituloPK = ActividadEducativaPerteneceATopico.tituloPKFK  WHERE tituloPK = '" + activityTitle + "'";
            return CreateTableFromQuery(query);
        }

        private void LinkEducationalActivityWithTargetAudience(EducationalActivityModel activity, DataTable resultingTable) {
            activity.TargetAudience = new List<string>();
            foreach (DataRow column in resultingTable.Rows) {
                var tempTopic = Convert.ToString(column["publicoMetaPK"]);
                activity.TargetAudience.Add(tempTopic);
            }
        }

        private void LinkActivityWithTopics(EducationalActivityModel activity, DataTable resultingTable) {
            activity.Topics = new List<string>();
            foreach (DataRow column in resultingTable.Rows) {
                var tempTopic = Convert.ToString(column["nombreTopicoPKFK"]);
                activity.Topics.Add(tempTopic);
            }
        }

        public bool UpdateActivityState(string activityTitle, int state) {
            string query = "UPDATE ActividadEducativa SET estado = " + state + " WHERE tituloPK = '" + activityTitle + "' ";
            SqlCommand queryCommand = new SqlCommand(query, connection);
            connection.Open();
            bool activityStateUpdateSuccess = queryCommand.ExecuteNonQuery() >= 1;
            connection.Close();
            return activityStateUpdateSuccess;
        }
    }
}