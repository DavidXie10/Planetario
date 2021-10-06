using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using Planetarium.Models;

namespace Planetarium.Handlers {

    public class ActivityHandler {

        private SqlConnection connection;
        private string connectionRoute;

        public ActivityHandler() {
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

        public List<ActivityModel> GetAllActivities() {
            string query = "SELECT DISTINCT F.nombre+ ' ' + F.apellido 'publicador',"
                            + " AE.tituloPK,"
                            +" AE.descripcion,"
                            +" AE.fechaInicioPK,"
                            +" AE.duracion,"
                            +" AE.capacidadMaxima,"
                            +" AE.precio,"
                            +" AE.nivelComplejidad,"
                            +" AE.estado,"
                            +" AE.tipo,"
                            +" AE.enlace,"
                            +" T.categoria"
                            +" FROM Funcionario F  JOIN ActividadEducativa AE"
                            +" ON F.cedulaPK  = AE.cedulaFK "
                            +" JOIN ActividadEducativaPerteneceATopico AEPT ON AE.tituloPK = AEPT.tituloPKFK"
                            +" JOIN Topico T ON AEPT.nombreTopicoPKFK = T.nombrePK"
                            +" JOIN Idioma I ON I.cedulaPK = AE.cedulaFK ORDER BY AE.fechaInicioPK ";
            DataTable resultingTable = CreateTableFromQuery(query);
            List<ActivityModel> activities = new List<ActivityModel>();
            foreach (DataRow column in resultingTable.Rows) {
                activities.Add(
                    new ActivityModel {
                        Title = Convert.ToString(column["tituloPK"]),
                        Description = Convert.ToString(column["descripcion"]),
                        Date = Convert.ToDateTime(column["fechaInicioPK"]),
                        Duration = Convert.ToInt32(column["duracion"]),
                        Capacity = Convert.ToString(column["capacidadMaxima"]),
                        Price = Convert.ToInt32(column["precio"]),
                        Complexity = Convert.ToString(column["nivelComplejidad"]),
                        State = Convert.ToInt32(column["estado"]),
                        Type = Convert.ToString(column["tipo"]),
                        Link = Convert.ToString(column["enlace"]),
                        Publisher = Convert.ToString(column["publicador"]),
                        Category = Convert.ToString(column["categoria"])
                    }
                );
            }
            LinkAllTopics(activities);

            return activities;
        }

        private void LinkAllTopics(List<ActivityModel> activities)
        {
            foreach (ActivityModel scoop in activities)
            {
                DataTable resultingTableWithTheirTopic = GetTopicsPerTable(scoop.Title);
                LinkScoopWithTopics(scoop, resultingTableWithTheirTopic);
            }
        }

        private DataTable GetTopicsPerTable(string scoopTitle)
        {
            string query = "SELECT nombreTopicoPKFK FROM ActividadEducativa INNER JOIN ActividadEducativaPerteneceATopico ON ActividadEducativa.tituloPK = ActividadEducativaPerteneceATopico.tituloPKFK  WHERE tituloPK = '"+ scoopTitle + "'";
            return CreateTableFromQuery(query);
        }


        private void LinkScoopWithTopics(ActivityModel scoop, DataTable resultingTable)
        {
            scoop.Topics = new List<string>();
            foreach (DataRow column in resultingTable.Rows)
            {
                var tempTopic = Convert.ToString(column["nombreTopicoPKFK"]);
                scoop.Topics.Add(tempTopic);
            }
        }

        private byte[] GetFileBytes(HttpPostedFileBase file) {
            byte[] bytes;
            BinaryReader reader = new BinaryReader(file.InputStream);
            bytes = reader.ReadBytes(file.ContentLength);
            return bytes;
        }
    }
}