using System;
using System.Collections.Generic;
using System.Data;
using Planetarium.Models;

namespace Planetarium.Handlers {
    abstract public class DatabaseClassificationsHandler : DatabaseHandler {
        public ContentParser ContentParser { get; set; }

        public DatabaseClassificationsHandler() {
            ContentParser = new ContentParser();
        }

        public List<string> GetAllCategories() {
            List<string> categories = new List<string>();

            string query = "SELECT DISTINCT categoria " +
                            "FROM Topico";

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
                           "WHERE T.categoria = '" + category + "';";

            DataTable topicsDataTable = CreateTableFromQuery(query);
            foreach (DataRow column in topicsDataTable.Rows) {
                topics.Add(Convert.ToString(column["nombrePK"]));
            }

            return topics;
        }

        protected void LinkAllFeatureWithTopics(Dictionary<string[], List<string>> featureTopicList) {
            foreach (string[] featureKeys in featureTopicList.Keys) {
                DataTable resultingTableOfFeatureWithItsTopics = GetFeatureWithTopicsTable(featureKeys);
                LinkFeatureWithTopics(featureTopicList[featureKeys], resultingTableOfFeatureWithItsTopics);
            }
        }

        abstract protected DataTable GetFeatureWithTopicsTable(string [] keys);

        private void LinkFeatureWithTopics(List<string> topicsList, DataTable resultingTable) {
            foreach (DataRow column in resultingTable.Rows) {
                var tempTopic = Convert.ToString(column["nombreTopicoPKFK"]);
                topicsList.Add(tempTopic);
            }
        }  
    }
}