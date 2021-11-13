using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Planetarium.Models;

namespace Planetarium.Handlers {
    public class EducationalActivityHandler : DatabaseClassificationsHandler {
        const int ON_REVIEW = 0;
        const int APPROVED = 1;

        public bool CheckEducationalActivity(string title) {
            string query = "SELECT Count(*) AS RowsCount FROM ActividadEducativa " +
                           "WHERE tituloPK = '" + title + "'";

            DataTable resultingTable = CreateTableFromQuery(query);

            return Convert.ToInt32(resultingTable.Rows[0]["RowsCount"]) > 0;
        }

        public bool ProposeEducationalActivity(EducationalActivityEventModel educationalActivity) {
            bool success = false;

            if (CheckEducationalActivity(educationalActivity.Title) == false) {
                CreateEducationalActivity(educationalActivity);
            }

            string query = "INSERT INTO EventoActividadEducativa (tituloPKFK, fechaInicioPK, capacidadMaxima, precio, estadoRevision, banderaVirtual, enlace, banderaPresencial) " +
                            "VALUES(@tituloPK,@fecha, @capacidad, @precio, @estado, @banderaVirtual, @enlace, @banderaPresencial)";

            SqlCommand queryCommand = new SqlCommand(query, connection);

            AddParametersToQueryCommandEvent(queryCommand, educationalActivity);
            success = DatabaseQuery(queryCommand);

            //TODO: hacer validaciones
            return success;
        }
        public bool CreateEducationalActivity(EducationalActivityEventModel educationalActivity) {
            bool success = false;
            string query = "INSERT INTO ActividadEducativa (tituloPK, duracion, descripcion, nivelComplejidad,cedulaFK,tipo) " +
                           "VALUES(@tituloPK,@duracion,@descripcion,@nivelComplejidad,'106260895',@tipo)";

            SqlCommand queryCommand = new SqlCommand(query, connection);

            //TO-DO: Cambiar cedula quemada
            AddParametersToQueryCommand(queryCommand, educationalActivity);

            success = DatabaseQuery(queryCommand);

            //TODO: hacer validaciones
            if(educationalActivity.ActivityType == "Charla" || educationalActivity.ActivityType == "Taller") {
                success = InsertActivitiesTopics(educationalActivity);
                success = InsertActivitiesAudiences(educationalActivity);
            }
            return success;
        }

        public bool InsertEducationalMaterial(EducationalActivityModel activity) {
            string query = "UPDATE ActividadEducativa " +
                           "SET rutasMaterialesEducativos = '" + ContentParser.GetStringFromList(activity.RefEducationalMaterial) + "'" +
                           "WHERE tituloPK = '" + activity.Title + "'";
            SqlCommand queryCommand = new SqlCommand(query, connection);
            bool success = DatabaseQuery(queryCommand);

            return success;
        }

        private void AddParametersToQueryCommand(SqlCommand queryCommand, EducationalActivityModel educationalActivity) {
            queryCommand.Parameters.AddWithValue("@tituloPK", educationalActivity.Title);
            queryCommand.Parameters.AddWithValue("@duracion", educationalActivity.Duration);
            queryCommand.Parameters.AddWithValue("@descripcion", educationalActivity.Description == null ? "" : educationalActivity.Description);
            queryCommand.Parameters.AddWithValue("@nivelComplejidad", educationalActivity.ComplexityLevel == null ? "" : educationalActivity.ComplexityLevel);
            queryCommand.Parameters.AddWithValue("@tipo", educationalActivity.ActivityType);
        }

        private void AddParametersToQueryCommandEvent(SqlCommand queryCommand, EducationalActivityEventModel educationalActivity) {
            queryCommand.Parameters.AddWithValue("@tituloPK", educationalActivity.Title);
            queryCommand.Parameters.AddWithValue("@fecha", educationalActivity.Date);
            queryCommand.Parameters.AddWithValue("@estado", (educationalActivity.ActivityType == "Charla" || educationalActivity.ActivityType == "Taller") ? ON_REVIEW : APPROVED);
            queryCommand.Parameters.AddWithValue("@capacidad", educationalActivity.MaximumCapacity);
            queryCommand.Parameters.AddWithValue("@precio", educationalActivity.Price);
            queryCommand.Parameters.AddWithValue("@enlace", educationalActivity.Link == null ? "" : educationalActivity.Link);
            queryCommand.Parameters.AddWithValue("@banderaVirtual", educationalActivity.TypeOfAssistance != null && (educationalActivity.TypeOfAssistance == "Mixto" || educationalActivity.TypeOfAssistance == "Virtual"));
            queryCommand.Parameters.AddWithValue("@banderaPresencial", educationalActivity.TypeOfAssistance != null && (educationalActivity.TypeOfAssistance == "Mixto" || educationalActivity.TypeOfAssistance == "Presencial"));
        }

        private bool InsertActivitiesTopics(EducationalActivityModel educationalActivity) {
            bool success = false;

            foreach (string topic in educationalActivity.Topics) {
                string query =  "INSERT INTO ActividadEducativaPerteneceATopico " +
                                "VALUES ('" + educationalActivity.Title + "','" + topic + "')";
                success = DatabaseQuery(query);
            }

            return success;
        }

        private bool InsertActivitiesAudiences(EducationalActivityModel educationalActivity) {
            bool success = false;

            foreach (string audience in educationalActivity.TargetAudience) {
                string query =  "INSERT INTO PublicoMeta " +
                                "VALUES ('" + educationalActivity.Title + "','" + audience + "')";
                success = DatabaseQuery(query);
            }

            return success;
        }

        public List<EducationalActivityEventModel> GetAllOnRevisionActivities() {
            return GetAllActivitiesFromState(ON_REVIEW);
        }

        public List<EducationalActivityEventModel> GetAllApprovedActivities() {
            return GetAllActivitiesFromState(APPROVED);
        }

        public List<EducationalActivityEventModel> GetAllActivitiesFromState(int state) {

            List<EducationalActivityEventModel> activities = new List<EducationalActivityEventModel>();

            string query = "SELECT DISTINCT F.nombre+ ' ' + F.apellido AS 'publicador',"
                            + " AE.tituloPK,"
                            + " AE.descripcion,"
                            + " EAE.fechaInicioPK,"
                            + " AE.duracion,"
                            + " AE.tipo,"
                            + " EAE.capacidadMaxima,"
                            + " EAE.precio,"
                            + " AE.nivelComplejidad,"
                            + " EAE.estadoRevision,"
                            + " EAE.enlace,"
                            + " EAE.banderaVirtual,"
                            + " EAE.banderaPresencial,"
                            + " T.categoria"
                            + " FROM Funcionario F RIGHT JOIN ActividadEducativa AE ON F.cedulaPK  = AE.cedulaFK "
                            + " LEFT JOIN ActividadEducativaPerteneceATopico AEPT ON AE.tituloPK = AEPT.tituloPKFK"
                            + " LEFT JOIN Topico T ON AEPT.nombreTopicoPKFK = T.nombrePK"
                            + " RIGHT JOIN EventoActividadEducativa EAE ON EAE.tituloPKFK = AE.tituloPK"
                            + " WHERE EAE.estadoRevision = " + state
                            + " ORDER BY EAE.fechaInicioPK DESC";

            DataTable resultingTable = CreateTableFromQuery(query);
            foreach (DataRow rawEducationalInfo in resultingTable.Rows) {
                activities.Add(CreateInstanceEducationalActivity(rawEducationalInfo));
            }
             
            LinkAllTargetAudience(activities);
            LinkAllFeatureWithTopics(CreateDictionary(activities));
            LinkAllMaterialWithActivity(activities);

            return activities;
        }

        public List<EducationalActivityEventModel> GetAllSimilarActivities(string title, List<string> topics, string category) {
            List<EducationalActivityEventModel> activities = new List<EducationalActivityEventModel>();
            List<EducationalActivityEventModel> similarActivities = new List<EducationalActivityEventModel>();

            activities = GetAllApprovedActivities();

            string[] compareWords = title.Split(' ');
            foreach (EducationalActivityEventModel activity in activities) {
                if (activity.Title != title) { 
                    if ((activity.ActivityType == "Charla" || activity.ActivityType == "Taller") && (CheckWords(compareWords, activity.Title) || FindOneElementInCommon(activity.Topics, topics) || (activity.Category == category))) {
                        similarActivities.Add(activity);
                    }
                }
            }
            return similarActivities;
        }

        private bool FindOneElementInCommon(List<string> listA, List<string> listB) {
            foreach (string element in listA) {
                if (listB.Contains(element)) {
                    return true;
                }
            }
            return false;
        }

        private bool CheckWords(string[] compareWords, string activityTitle) {
            foreach (string word in compareWords) {
                if (activityTitle.Contains(word) && (!IsArticle(word))) {
                    return true;
                }
            }
            return false;
        }
            

        private bool IsArticle(string word) {
            return (word == "de" || word == "en" || word == "la" || word == "y" || word == "el" || word == "del");
        }

        private EducationalActivityEventModel CreateInstanceEducationalActivity(DataRow rawEducationalInfo) {
            return new EducationalActivityEventModel {
                Title = Convert.ToString(rawEducationalInfo["tituloPK"]),
                Description = Convert.ToString(rawEducationalInfo["descripcion"]),
                Date = Convert.ToDateTime(rawEducationalInfo["fechaInicioPK"]),
                Duration = Convert.ToInt32(rawEducationalInfo["duracion"]),
                ActivityType = Convert.ToString(rawEducationalInfo["tipo"]),
                MaximumCapacity = Convert.ToInt32(rawEducationalInfo["capacidadMaxima"]),
                Price = Convert.ToInt32(rawEducationalInfo["precio"]),
                ComplexityLevel = Convert.ToString(rawEducationalInfo["nivelComplejidad"]),
                State = Convert.ToString(rawEducationalInfo["estadoRevision"]),
                TypeOfAssistance = GetTypeOfAssistence(Convert.ToBoolean(rawEducationalInfo["banderaVirtual"]), Convert.ToBoolean(rawEducationalInfo["banderaPresencial"])),
                Link = Convert.ToString(rawEducationalInfo["enlace"]),
                Publisher = Convert.ToString(rawEducationalInfo["publicador"]),
                Category = Convert.ToString(rawEducationalInfo["categoria"])
            };
        }

        public List<EventModel> GetAllCalendarActivitiesFromState(int state) {
            List<EventModel> activities = new List<EventModel>();

            string query = "SELECT DISTINCT AE.tituloPK,"
                            + " AE.descripcion,"
                            + " EAE.fechaInicioPK,"
                            + " AE.tipo"
                            + " FROM ActividadEducativa AE"
                            + " JOIN EventoActividadEducativa EAE ON EAE.tituloPKFK = AE.tituloPK"
                            + " WHERE EAE.estadoRevision = " + state;

            DataTable resultingTable = CreateTableFromQuery(query);
            foreach (DataRow rawEducationalInfo in resultingTable.Rows) {
                activities.Add(CreateInstanceEventModel(rawEducationalInfo));
            }

            return activities;
        }

        private EventModel CreateInstanceEventModel(DataRow rawEducationalInfo) {
            return new EventModel {
                Title = Convert.ToString(rawEducationalInfo["tituloPK"]),
                Description = Convert.ToString(rawEducationalInfo["descripcion"]),
                Date = Convert.ToDateTime(rawEducationalInfo["fechaInicioPK"]).ToString("yyyy-MM-dd"),
                TypeOfEvent = Convert.ToString(rawEducationalInfo["tipo"])
            };
        }
        private string GetTypeOfAssistence(bool virtualFlag, bool onSiteFlag) {
            return (virtualFlag && onSiteFlag) ? "Mixto" : (virtualFlag) ? "Virtual" : "Presencial";
        }


        private void LinkAllTargetAudience(List<EducationalActivityEventModel> activities) {
            foreach (EducationalActivityEventModel activity in activities) {
                DataTable resultingTableWithTargetAudience = GetTargetAudiencePerEducationalActivity(activity.Title);
                LinkEducationalActivityWithTargetAudience(activity, resultingTableWithTargetAudience);
            }
        }

        private DataTable GetTargetAudiencePerEducationalActivity(string activityTitle) {
            string query =  "SELECT publicoMetaPK " +
                            "FROM PublicoMeta INNER JOIN ActividadEducativa ON PublicoMeta.tituloPKFK = ActividadEducativa.tituloPK " +
                            "WHERE PublicoMeta.tituloPKFK = '" + activityTitle + "'";
            return CreateTableFromQuery(query);
        }

        private void LinkEducationalActivityWithTargetAudience(EducationalActivityModel activity, DataTable resultingTable) {
            activity.TargetAudience = new List<string>();
            foreach (DataRow column in resultingTable.Rows) {
                var tempTopic = Convert.ToString(column["publicoMetaPK"]);
                activity.TargetAudience.Add(tempTopic);
            }
        }

        private Dictionary<string[], List<string>> CreateDictionary(List<EducationalActivityEventModel> activities) {
            Dictionary<string[], List<string>> tempDictionary = new Dictionary<string[], List<string>>();
            foreach (EducationalActivityEventModel activityInstance in activities) {
                tempDictionary.Add(new string[] { activityInstance.Title }, activityInstance.Topics = new List<string>());
            }
            return tempDictionary;
        }

        private void LinkAllMaterialWithActivity(List<EducationalActivityEventModel> activities) {
            foreach (EducationalActivityEventModel activity in activities) {
                DataTable resultingTable = GetMaterialPerActivity(activity.Title);
                if (resultingTable != null) {
                    LinkActivityWithMaterial(activity, resultingTable);
                }
            }
        }

        private DataTable GetMaterialPerActivity(string activityTitle) {
            string query = "SELECT rutasMaterialesEducativos " +
                           "FROM ActividadEducativa AE " +
                           "WHERE AE.tituloPK = '" + activityTitle + "'";
            return CreateTableFromQuery(query);
        }

        private void LinkActivityWithMaterial(EducationalActivityEventModel activity, DataTable resultingTable) {
            activity.RefEducationalMaterial = ContentParser.GetListFromString(Convert.ToString(resultingTable.Rows[0]["rutasMaterialesEducativos"]));
        }

        override protected DataTable GetFeatureWithTopicsTable(string[] keys) {
            string query = "SELECT nombreTopicoPKFK FROM ActividadEducativa AE " +
                            "INNER JOIN ActividadEducativaPerteneceATopico AEPT ON AE.tituloPK = AEPT.tituloPKFK " +
                            "WHERE AE.tituloPK = '" + keys[0] + "' ";
            return CreateTableFromQuery(query);
        }


        public bool UpdateActivityState(string activityTitle, int state) {
            string query = "UPDATE EventoActividadEducativa SET estadoRevision = " + state + " WHERE tituloPK = '" + activityTitle + "' ";
            return DatabaseQuery(query);
        }

        public List<string> GetAllActivities() {
            List<string> activitiesTitles = new List<string>();
            string query = "SELECT * " +
                           "FROM EventoActividadEducativa EAE " +
                           "INNER JOIN ActividadEducativa AE ON EAE.tituloPKFK = AE.tituloPK " +
                           "WHERE EAE.estadoRevision = 1";
            DataTable resultingTable = CreateTableFromQuery(query);
            foreach (DataRow column in resultingTable.Rows) {
                activitiesTitles.Add(Convert.ToString(column["tituloPK"]));
            }
            return activitiesTitles;
        }

        public List<EducationalActivityEventModel> GetAllActivitiesParticipants() {

            List<EducationalActivityEventModel> activities = new List<EducationalActivityEventModel>();

            string query =  " SELECT DISTINCT AE.tituloPK, EAE.fechaInicioPK," +
                            " AE.nivelComplejidad, COUNT(*) AS 'Participantes'" +
                            " FROM Funcionario F JOIN ActividadEducativa AE ON F.cedulaPK = AE.cedulaFK" +
                            " JOIN ActividadEducativaPerteneceATopico AEPT ON AE.tituloPK = AEPT.tituloPKFK" +
                            " JOIN EventoActividadEducativa EAE ON EAE.tituloPKFK = AE.tituloPK" +
                            " JOIN Inscribirse I ON(I.tituloPKFK = AE.tituloPK AND EAE.fechaInicioPK = I.fechaInicioPKFK)" +
                            " WHERE EAE.estadoRevision = 1" +
                            " GROUP BY AE.tituloPK, EAE.fechaInicioPK, AE.nivelComplejidad" +
                            " ORDER BY EAE.fechaInicioPK DESC";

            DataTable resultingTable = CreateTableFromQuery(query);
            foreach (DataRow rawEducationalInfo in resultingTable.Rows) {
                activities.Add(CreateInstanceEducationalParticipants(rawEducationalInfo));
            }

            LinkAllTargetAudience(activities);

            return activities;
        }

        private EducationalActivityEventModel CreateInstanceEducationalParticipants(DataRow rawEducationalInfo) {
            return new EducationalActivityEventModel {
                Title = Convert.ToString(rawEducationalInfo["tituloPK"]),
                StatisticsDate = Convert.ToString(rawEducationalInfo["fechaInicioPK"]),
                ComplexityLevel = Convert.ToString(rawEducationalInfo["nivelComplejidad"]),
                RegisteredParticipants = Convert.ToInt32(rawEducationalInfo["Participantes"])
            };
        }
    }
}