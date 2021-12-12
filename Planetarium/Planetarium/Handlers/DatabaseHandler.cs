using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Planetarium.Handlers {
    public class DatabaseHandler {

        protected SqlConnection connection;
        protected string connectionRoute;
        protected const string CONNECTION_NAME = "PlanetariumConnection";

        public DatabaseHandler() {
            connectionRoute = ConfigurationManager.ConnectionStrings[CONNECTION_NAME].ToString();
            connection = new SqlConnection(connectionRoute);
        }

        public DataTable CreateTableFromQuery(string query) {
            SqlCommand queryCommand = new SqlCommand(query, connection);
            SqlDataAdapter tableAdapter = new SqlDataAdapter(queryCommand);
            DataTable queryTable = new DataTable();
            connection.Open();
            tableAdapter.Fill(queryTable);
            connection.Close();
            return queryTable;
        }

        public bool DatabaseQuery(string query) {
            bool success = false;
            SqlCommand queryCommand = new SqlCommand(query, connection);
            connection.Open();
            success = queryCommand.ExecuteNonQuery() >= 1;
            connection.Close();
            return success;
        }

        public bool DatabaseQuery(SqlCommand queryCommand) {
            bool success = false;
            connection.Open();
            success = queryCommand.ExecuteNonQuery() >= 1;
            connection.Close();
            return success;
        }
    }
}