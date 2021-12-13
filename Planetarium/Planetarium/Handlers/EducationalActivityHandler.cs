using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using Planetarium.Models;

namespace Planetarium.Handlers {
    public class EducationalActivityHandler : DatabaseClassificationsHandler {
        const int ON_REVIEW = 0;
        const int APPROVED = 1;
        public string[] TARGET_AUDIENCES = { "Infantil", "Juvenil", "Adulto", "Adulto Mayor" };

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

            return success;
        }

        public bool CreateEducationalActivity(EducationalActivityEventModel educationalActivity) {
            bool success = false;
            string query = "INSERT INTO ActividadEducativa (tituloPK, duracion, descripcion, nivelComplejidad,cedulaFK,tipo) " +
                           "VALUES(@tituloPK,@duracion,@descripcion,@nivelComplejidad,'103230738',@tipo) ";

            SqlCommand queryCommand = new SqlCommand(query, connection);

            AddParametersToQueryCommand(queryCommand, educationalActivity);

            success = DatabaseQuery(queryCommand);

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
            queryCommand.Parameters.AddWithValue("@banderaVirtual", educationalActivity.TypeOfAssistance != null && (educationalActivity.TypeOfAssistance == "Bimodal" || educationalActivity.TypeOfAssistance == "Virtual"));
            queryCommand.Parameters.AddWithValue("@banderaPresencial", educationalActivity.TypeOfAssistance != null && (educationalActivity.TypeOfAssistance == "Bimodal" || educationalActivity.TypeOfAssistance == "Presencial"));
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
                            + " T.categoria,"
                            + " AE.rutasMaterialesEducativos"
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

            return activities;
        }

        public List<EducationalActivityEventModel> GetAllSimilarActivities(string title, List<string> topics, string category) {
            List<EducationalActivityEventModel> activities = new List<EducationalActivityEventModel>();
            List<EducationalActivityEventModel> similarActivities = new List<EducationalActivityEventModel>();

            activities = GetAllApprovedActivities();

            string[] compareWords = title.Split(' ');
            foreach (EducationalActivityEventModel activity in activities) {
                if (activity.Title != title) { 
                    if (IsSimilarActivity(activity, compareWords, topics, category)) {
                        similarActivities.Add(activity);
                    }
                }
            }
            return similarActivities;
        }

        private bool IsSimilarActivity(EducationalActivityEventModel activity, string[] compareWords, List<string> topics, string category) {
            return (activity.ActivityType == "Charla" || activity.ActivityType == "Taller") && (CheckWords(compareWords, activity.Title) || FindOneElementInCommon(activity.Topics, topics) || (activity.Category == category));
        }

        private bool FindOneElementInCommon(List<string> source, List<string> target) {
            foreach (string element in source) {
                if (target.Contains(element)) {
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
                Category = Convert.ToString(rawEducationalInfo["categoria"]),
                RefEducationalMaterial = ContentParser.GetListFromString(Convert.ToString(rawEducationalInfo["rutasMaterialesEducativos"]))
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
            return (virtualFlag && onSiteFlag) ? "Bimodal" : (virtualFlag) ? "Virtual" : "Presencial";
        }

        private void LinkAllTargetAudience(List<EducationalActivityEventModel> activities) {
            foreach (EducationalActivityEventModel activity in activities) {
                DataTable resultingTableWithTargetAudience = GetTargetAudiencePerEducationalActivity(activity.Title);
                LinkEducationalActivityWithTargetAudience(activity, resultingTableWithTargetAudience);
            }
        }

        private DataTable GetTargetAudiencePerEducationalActivity(string activityTitle) {
            string query = "DECLARE @titulo NVARCHAR(100) " +
                           "SELECT @titulo = '" + activityTitle + "' " +
                           "EXEC USP_obtenerPublicoMetaPorActividad @titulo ";
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

        public List<SelectListItem> GetAllActivitiesNames() {
            List<SelectListItem> activitiesTitles = new List<SelectListItem>();
            string query = "SELECT tituloPK " +
                           "FROM EventoActividadEducativa EAE " +
                           "INNER JOIN ActividadEducativa AE ON EAE.tituloPKFK = AE.tituloPK " +
                           "WHERE EAE.estadoRevision = 1" +
                           "AND (AE.tipo = 'Charla' OR AE.tipo = 'Taller')";
            DataTable resultingTable = CreateTableFromQuery(query);

            foreach (DataRow column in resultingTable.Rows) {
                activitiesTitles.Add(new SelectListItem { Text = Convert.ToString(column["tituloPK"]), Value = Convert.ToString(column["tituloPK"]) });
            }
            return activitiesTitles;
        }

        public List<EducationalActivityEventModel> GetAllActivitiesParticipants() {
            List<EducationalActivityEventModel> activities = new List<EducationalActivityEventModel>();

            string query = "SELECT DISTINCT EAE.tituloPKFK, EAE.fechaInicioPK, AE.nivelComplejidad, " +
                            "SUM(CASE WHEN E.publicoMeta = 'Infantil' THEN 1 ELSE 0 END) AS 'Infantil', " +
                            "SUM(CASE WHEN E.publicoMeta = 'Juvenil' THEN 1 ELSE 0 END) AS 'Juvenil',  " +
                            "SUM(CASE WHEN E.publicoMeta = 'Adulto' THEN 1 ELSE 0 END) AS 'Adulto',  " +
                            "SUM(CASE WHEN E.publicoMeta = 'Adulto Mayor' THEN 1 ELSE 0 END) AS 'Adulto Mayor' " +
                            "FROM Funcionario F JOIN ActividadEducativa AE ON F.cedulaPK = AE.cedulaFK " +
                            "JOIN ActividadEducativaPerteneceATopico AEPT ON AE.tituloPK = AEPT.tituloPKFK " +
                            "JOIN EventoActividadEducativa EAE ON EAE.tituloPKFK = AE.tituloPK " +
                            "JOIN Inscribirse I ON(I.tituloPKFK = AE.tituloPK AND EAE.fechaInicioPK = I.fechaInicioPKFK) " +
                            "JOIN Entrada E ON E.idPK = I.idEntradaPKFK " +
                            "JOIN Visitante V ON V.cedulaPK = I.cedulaPKFK " +
                            "WHERE EAE.estadoRevision = 1 " +
                            "GROUP BY EAE.tituloPKFK, EAE.fechaInicioPK, AE.nivelComplejidad; ";

            DataTable resultingTable = CreateTableFromQuery(query);
            foreach (DataRow rawEducationalInfo in resultingTable.Rows) {
                activities.Add(CreateInstanceEducationalParticipants(rawEducationalInfo));
            }
            return activities;
        }

        private EducationalActivityEventModel CreateInstanceEducationalParticipants(DataRow rawEducationalInfo) {
            return new EducationalActivityEventModel {
                Title = Convert.ToString(rawEducationalInfo["tituloPKFK"]),
                StatisticsDate = Convert.ToString(rawEducationalInfo["fechaInicioPK"]),
                ComplexityLevel = Convert.ToString(rawEducationalInfo["nivelComplejidad"]),
                RegisteredParticipants = LinkRegisteredParticipants(rawEducationalInfo)
            };
        }

        public bool CheckCapacity(string activityTitle, string activityDate) {
            string query = "SELECT EAE.tituloPKFK, (EAE.capacidadMaxima - COUNT(I.idEntradaPKFK)) AS Cupos " +
                           "FROM Inscribirse I " +
                           "RIGHT JOIN EventoActividadEducativa EAE ON(I.tituloPKFK = EAE.tituloPKFK AND I.fechaInicioPKFK = EAE.fechaInicioPK) " +
                           "WHERE EAE.tituloPKFK = '" + activityTitle + "' " +
                           "AND EAE.fechaInicioPK = '" + activityDate + "' " +
                           "GROUP BY EAE.tituloPKFK, EAE.fechaInicioPK, EAE.capacidadMaxima;";

            DataTable resultingTable = CreateTableFromQuery(query);

            return Convert.ToInt32(resultingTable.Rows[0]["Cupos"]) > 0;
        }

        public int GetMaxCapacity(string activityTitle, string activityDate) {
            string query = "SELECT EAE.capacidadMaxima " +
                           "FROM EventoActividadEducativa EAE " +
                           "WHERE EAE.tituloPKFK = '" + activityTitle + "' " +
                           "AND EAE.fechaInicioPK = '" + activityDate + "' ";

            DataTable resultingTable = CreateTableFromQuery(query);

            return Convert.ToInt32(resultingTable.Rows[0]["capacidadMaxima"]);
        }

        public List<string> GetReservedSeats(string activityTitle, string activityDate) {
            string query = "SELECT E.numeroAsiento " +
                           "FROM Inscribirse I " +
                           "INNER JOIN Entrada E ON I.idEntradaPKFK = E.idPK " +
                           "WHERE I.tituloPKFK = '" + activityTitle + "' " +
                           "AND I.fechaInicioPKfk = '" + activityDate + "'";

            DataTable resultingTable = CreateTableFromQuery(query);    
            List<string> reservedSeats = new List<string>();

            foreach(DataRow rawEducationalInfo in resultingTable.Rows) {
                reservedSeats.Add(Convert.ToString(rawEducationalInfo["numeroAsiento"]));
            }
            return reservedSeats;
        }

        public double GetPrice(string activityTitle, string activityDate) {
            string query = "SELECT precio " +
                           "FROM EventoActividadEducativa EAE " +
                           "WHERE EAE.tituloPKFK = '" + activityTitle + "' " +
                           "AND EAE.fechaInicioPK = '" + activityDate + "' ";

            DataTable resultingTable = CreateTableFromQuery(query);

            return Convert.ToDouble(resultingTable.Rows[0]["precio"]);
        }


        public Dictionary<string, int> LinkRegisteredParticipants(DataRow rawEducationalInfo) {
            Dictionary<string, int> registeredParticipants = new Dictionary<string, int>();
            foreach (string targetAudience in TARGET_AUDIENCES) {
                registeredParticipants[targetAudience] = Convert.ToInt32(rawEducationalInfo[targetAudience]);
            }

            return registeredParticipants;
        }

        public Dictionary<string, int> FillRank(string columnName, string methodName ) {
            Dictionary<string, int> categoriesRank = new Dictionary<string, int>();

            string query = "SELECT * " +
                           "FROM rankingInvolucramiento" + methodName + "()";

            DataTable resultingTable = CreateTableFromQuery(query);
            foreach (DataRow rawEducationalInfo in resultingTable.Rows) {
                if (Convert.ToInt32(rawEducationalInfo["Inscritos"]) > 0) {
                    categoriesRank.Add(Convert.ToString(rawEducationalInfo[columnName]), Convert.ToInt32(rawEducationalInfo["Inscritos"]));
                }
            }

            return categoriesRank;
        }

        public bool InsertStreaming(StreamingModel streaming) {
            string query = "UPDATE EventoActividadEducativa " +
                           "SET enlace = '" + streaming.Link + "' " +
                           "WHERE tituloPKFK = '" + streaming.EducationalActivityTitle + "';";

            return DatabaseQuery(query);
        }
    }
}