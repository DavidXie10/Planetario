using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Planetarium.Models;
using static Planetarium.Handlers.DatabaseHandler;

namespace Planetarium.Handlers {
    public class EducationalActivityHandler : DatabaseClassificationsHandler {
        const int ON_REVIEW = 0;
        const int APPROVED = 1;

        public bool ProposeEducationalActivity(EducationalActivityModel educationalActivity) {
            bool success = false;
            string query = "INSERT INTO ActividadEducativa (tituloPK, fechaInicioPK, duracion, capacidadMaxima, precio, descripcion, nivelComplejidad, estadoRevision, modalidad, banderaVirtual, enlace, banderaPresencial, cedulaFK) " +
                           "VALUES(@tituloPK,@fechaInicioPK,@duracion,@capacidadMaxima,@precio,@descripcion,@nivelComplejidad,0,@modalidad,@banderaVirtual,@enlace,@banderaPresencial,'106260895') ";
            SqlCommand queryCommand = new SqlCommand(query, connection);

            //TO-DO: Cambiar cedula quemada
            AddParametersToQueryCommand(queryCommand, educationalActivity);

            success = DatabaseQuery(queryCommand);
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
            queryCommand.Parameters.AddWithValue("@modalidad", educationalActivity.ActivityType);
            queryCommand.Parameters.AddWithValue("@enlace", educationalActivity.Link);
            int virtualFlag = educationalActivity.TypeOfAssistance == "Virtual" ? 1 : 0;
            int onSiteFlag = educationalActivity.TypeOfAssistance == "Virtual" ? 0 : 1;
            queryCommand.Parameters.AddWithValue("@banderaVirtual", virtualFlag);
            queryCommand.Parameters.AddWithValue("@banderaPresencial", onSiteFlag);
        }

        private bool InsertActivitiesTopics(EducationalActivityModel educationalActivity) {
            bool success = false;

            foreach (string topic in educationalActivity.Topics) {
                string query =  "INSERT INTO ActividadEducativaPerteneceATopico " +
                                "VALUES ('" + educationalActivity.Title + "','" + educationalActivity.Date + "','" + topic + "')";
                success = DatabaseQuery(query);
            }

            return success;
        }

        private bool InsertActivitiesAudiences(EducationalActivityModel educationalActivity) {
            bool success = false;

            foreach (string audience in educationalActivity.TargetAudience) {
                string query =  "INSERT INTO PublicoMeta " +
                                "VALUES ('" + educationalActivity.Title + "','" + educationalActivity.Date + "','" + audience + "')";
                success = DatabaseQuery(query);
            }

            return success;
        }

        public List<EducationalActivityModel> GetAllOnRevisionActivities() {
            return GetAllActivitiesFromState(ON_REVIEW);
        }

        public List<EducationalActivityModel> GetAllApprovedActivities() {
            return GetAllActivitiesFromState(APPROVED);
        }

        public List<EducationalActivityModel> GetAllActivitiesFromState(int state) {

            List<EducationalActivityModel> activities = new List<EducationalActivityModel>();

            string query = "SELECT DISTINCT F.nombre+ ' ' + F.apellido 'publicador',"
                            + " AE.tituloPK,"
                            + " AE.descripcion,"
                            + " AE.fechaInicioPK,"
                            + " AE.duracion,"
                            + " AE.capacidadMaxima,"
                            + " AE.precio,"
                            + " AE.nivelComplejidad,"
                            + " AE.estadoRevision,"
                            + " AE.modalidad,"
                            + " AE.enlace,"
                            + " AE.banderaVirtual,"
                            + " T.categoria"
                            + " FROM Funcionario F  JOIN ActividadEducativa AE"
                            + " ON F.cedulaPK  = AE.cedulaFK "
                            + " JOIN ActividadEducativaPerteneceATopico AEPT ON (AE.tituloPK = AEPT.tituloPKFK"
                            + " AND AE.fechaInicioPK = AEPT.fechaInicioPKFK)"
                            + " JOIN Topico T ON AEPT.nombreTopicoPKFK = T.nombrePK"
                            + " JOIN Idioma I ON I.cedulaPKFK = AE.cedulaFK "
                            + " WHERE AE.estadoRevision = " + state
                            + " ORDER BY AE.fechaInicioPK ";

            DataTable resultingTable = CreateTableFromQuery(query);
            foreach (DataRow rawEducationalInfo in resultingTable.Rows) {
                activities.Add(CreateInstanceEducationalActivity(rawEducationalInfo));
            }

            LinkAllTargetAudience(activities);
            LinkAllFeatureWithTopics(CreateDictionary(activities));

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
                State = Convert.ToString(rawEducationalInfo["estadoRevision"]),
                TypeOfAssistance = (Convert.ToInt32(rawEducationalInfo["banderaVirtual"]) == 1) ? "Virtual" : "Presencial",
                ActivityType = Convert.ToString(rawEducationalInfo["modalidad"]),
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
            string query =  "SELECT publicoMetaPK " +
                            "FROM PublicoMeta INNER JOIN ActividadEducativa " +
                            "ON PublicoMeta.fechaInicioPKFK = ActividadEducativa.fechaInicioPK AND PublicoMeta.tituloPKFK = ActividadEducativa.tituloPK " +
                            "WHERE PublicoMeta.tituloPKFK = '" + activityTitle + "'" + " AND PublicoMeta.fechaInicioPKFK = " + "'" + initialDate + "';";
            return CreateTableFromQuery(query);
        }

        private Dictionary<string[], List<string>> CreateDictionary(List<EducationalActivityModel> activities) {
            Dictionary<string[], List<string>> tempDictionary = new Dictionary<string[], List<string>>();
            foreach (EducationalActivityModel activityInstance in activities) {
                tempDictionary.Add(new string[] { activityInstance.Title, activityInstance.Date.ToString() }, activityInstance.Topics = new List<string>());
            }
            return tempDictionary;
        }

        override protected DataTable GetFeatureWithTopicsTable(string[] keys) {
            string query = "SELECT nombreTopicoPKFK FROM ActividadEducativa AE " +
                            "INNER JOIN ActividadEducativaPerteneceATopico AEPT " +
                            "ON (AE.tituloPK = AEPT.tituloPKFK " +
                            "AND AE.fechaInicioPK = AEPT.fechaInicioPKFK) " +
                            "WHERE AE.tituloPK = '" + keys[0] + "' " +
                            "AND AE.fechaInicioPK = '" + keys[1] + "' ";
            return CreateTableFromQuery(query);
        }

        private void LinkEducationalActivityWithTargetAudience(EducationalActivityModel activity, DataTable resultingTable) {
            activity.TargetAudience = new List<string>();
            foreach (DataRow column in resultingTable.Rows) {
                var tempTopic = Convert.ToString(column["publicoMetaPK"]);
                activity.TargetAudience.Add(tempTopic);
            }
        }

        public bool UpdateActivityState(string activityTitle, int state) {
            string query = "UPDATE ActividadEducativa SET estadoRevision = " + state + " WHERE tituloPK = '" + activityTitle + "' ";
            return DatabaseQuery(query);
        }
    }
}