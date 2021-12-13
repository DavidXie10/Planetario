using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Planetarium.Models;

namespace Planetarium.Handlers {
    public class AuthHandler : DatabaseHandler {     
        public bool InsertVisitorCredentials(VisitorModel visitor) {
            bool success = false;

            string query = "INSERT INTO Credencial (nombreUsuarioPK, cedulaVisitanteFK, claveDeAcceso, rol, banderaFuncionario)" +
                           " VALUES (@usuario, @cedulaVisitante, @claveDeAcceso, @rol, @banderaFuncionario)";

            SqlCommand queryCommand = new SqlCommand(query, connection);
            AddParametersToQueryCommand(queryCommand, visitor);
            success = DatabaseQuery(queryCommand);

            return success;
        }

        private void AddParametersToQueryCommand(SqlCommand queryCommand, VisitorModel visitor) {
            queryCommand.Parameters.AddWithValue("@usuario", visitor.Username);
            queryCommand.Parameters.AddWithValue("@cedulaVisitante", visitor.Dni);
            queryCommand.Parameters.AddWithValue("@claveDeAcceso", visitor.Password);
            queryCommand.Parameters.AddWithValue("@rol", 1);
            queryCommand.Parameters.AddWithValue("@banderaFuncionario", 0);
        }

        public List<string> GetAllVisitorsUserNames() {
            List<string> visitorsUserNames = new List<string>();
            string query = "SELECT nombreUsuarioPK FROM Credencial";
            DataTable resultingTable = CreateTableFromQuery(query);

            foreach (DataRow rawEducationalInfo in resultingTable.Rows) {
                visitorsUserNames.Add(Convert.ToString(rawEducationalInfo["nombreUsuarioPK"]));
            }

            return visitorsUserNames;
        }

        public bool Authenticate(LoginModel credentials) {
            string username = credentials.Username;
            string password = credentials.Password;

            string query = "SELECT Count(*) AS RowsCount " +
                           "FROM Credencial " +
                           "WHERE nombreUsuarioPK = '" + username + "'" +
                           "AND claveDeAcceso = '" + password + "'";

            DataTable resultingTable = CreateTableFromQuery(query);

            return Convert.ToInt32(resultingTable.Rows[0]["RowsCount"]) > 0;
        }

        public UserModel GetUserByUsername(string username) {
            string query = "SELECT * " +
                           "FROM Credencial " +
                           "WHERE nombreUsuarioPK = '" + username + "'";

            DataTable resultingTable = CreateTableFromQuery(query);
            DataRow userInstance = resultingTable.Rows[0];
            return GetUserContentFromTable(userInstance);
        }

        public UserModel GetUserContentFromTable(DataRow dbContent) {
            int employeeFlag = Convert.ToInt32(dbContent["banderaFuncionario"]);
            string userDniRol = employeeFlag == 1 ? "cedulaFuncionarioFK"  : "cedulaVisitanteFK";
            string dni = Convert.ToString(dbContent[userDniRol]);

            return new UserModel {
                Username = Convert.ToString(dbContent["nombreUsuarioPK"]),
                Password = Convert.ToString(dbContent["claveDeAcceso"]),
                Dni =  dni,
                EmployeeFlag = employeeFlag,
                Rol = Convert.ToInt32(dbContent["rol"]),
            };
        }

    }
}