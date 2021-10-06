using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Planetarium.Models;

namespace Planetarium.Handlers {
    public class EmployeesHandler {
        private SqlConnection connection;
        private string connectionRoute;

        public EmployeesHandler() {
            connectionRoute = ConfigurationManager.ConnectionStrings["EmployeeConnection"].ToString();
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

        public List<EmployeeModel> GetAllEmployees() {
            List<EmployeeModel> employees = new List<EmployeeModel>();
            string query = "SELECT * FROM Funcionario ORDER BY nombre ";
            DataTable resultingTable = CreateTableFromQuery(query);
            foreach (DataRow column in resultingTable.Rows) {
                employees.Add(CreateScoop(column));
            }
            LinkAllEmployeeWithLanguages(employees);

            return employees;
        }

        private EmployeeModel CreateScoop(DataRow scoopRawInfo) {
            return new EmployeeModel {
                Dni = Convert.ToString(scoopRawInfo["cedulaPK"]),
                FirstName = Convert.ToString(scoopRawInfo["nombre"]),
                LastName = Convert.ToString(scoopRawInfo["apellido"]),
                AcademicDegree = Convert.ToString(scoopRawInfo["titulosAcademicos"]),
                Occupation = Convert.ToString(scoopRawInfo["ocupacion"]),
                Mail = Convert.ToString(scoopRawInfo["correo"]),
                PhoneNumber = Convert.ToInt32(scoopRawInfo["telefono"]),
                Gender = Convert.ToChar(scoopRawInfo["genero"]),
                ExpertiseArea = Convert.ToString(scoopRawInfo["areaExpertiz"]),
                Address = Convert.ToString(scoopRawInfo["lugarDeResidencia"]),
                NativeCountry = Convert.ToString(scoopRawInfo["paisOrigen"]),
                DateOfBirth = Convert.ToDateTime(scoopRawInfo["fechaNacimiento"]),
                Phrase = Convert.ToString(scoopRawInfo["frase"]),
                IdPhoto = Convert.ToString(scoopRawInfo["fotoPerfil"])

            };
        }

        private void LinkAllEmployeeWithLanguages(List<EmployeeModel> employees) {
            foreach (EmployeeModel scoop in employees) {
                DataTable resultingTableOfEmployeeWithTheirLanguage = GetEmployeeWithLanguagesTable(scoop.Dni);
                LinkScoopWithLanguages(scoop, resultingTableOfEmployeeWithTheirLanguage);
            }
        }

        private DataTable GetEmployeeWithLanguagesTable(string scoopDni) {
            string query = "SELECT Idioma.idiomaPK FROM Funcionario " +
                        "INNER JOIN Idioma ON Funcionario.cedulaPK = Idioma.cedulaPK  " +
                        "WHERE Funcionario.cedulaPK = '" + scoopDni + "' ";

            return CreateTableFromQuery(query);
        }

        private void LinkScoopWithLanguages(EmployeeModel scoop, DataTable resultingTable) {
            scoop.Languages = new List<string>();
            foreach (DataRow column in resultingTable.Rows)
            {
                var tempLanguage = Convert.ToString(column["idiomaPK"]);
                scoop.Languages.Add(tempLanguage);
            }
        }


        private byte[] GetFileBytes(HttpPostedFileBase file) {
            byte[] bytes;
            BinaryReader reader = new BinaryReader(file.InputStream);
            bytes = reader.ReadBytes(file.ContentLength);
            return bytes;
        }

        public bool CreateEmployee(EmployeeModel employee) {
            bool employeeCreated = false;
            string query = "INSERT INTO Funcionario(cedulaPK,ocupacion,titulosAcademicos,foto,fotoTipo,correo,nombre,apellido,genero,fechaInicioEmpleo,fechaNacimiento,telefono,banderaColaborador,areaExpertiz,banderaCoordinador,banderaEducador,lugarDeResidencia,paisOrigen)" +
              "VALUES(@cedula,@ocupacion,@titulosAcademicos,@archivo,@tipoFoto,@correo,@nombre,@apellido,@gender,'2000-02-02',@fechaNacimiento,@telefono,1,@areaExpertiz,0,0,@lugarDeResidencia,@paisOrigen) ";
            string languageQuery = "INSERT INTO Idioma (cedulaPK, idiomaPK)" +
                                    "VALUES(@cedula, @idioma)";

            SqlCommand queryCommand = new SqlCommand(query, connection);

            queryCommand.Parameters.AddWithValue("@cedula", employee.Dni);
            queryCommand.Parameters.AddWithValue("@gender", employee.Gender);
            queryCommand.Parameters.AddWithValue("@ocupacion", employee.Occupation);
            queryCommand.Parameters.AddWithValue("@titulosAcademicos", employee.AcademicDegree);
            queryCommand.Parameters.AddWithValue("@correo", employee.Mail);
            queryCommand.Parameters.AddWithValue("@nombre", employee.FirstName);
            queryCommand.Parameters.AddWithValue("@apellido", employee.LastName);
            queryCommand.Parameters.AddWithValue("@telefono", employee.PhoneNumber);
            queryCommand.Parameters.AddWithValue("@areaExpertiz", employee.ExpertiseArea);
            queryCommand.Parameters.AddWithValue("@lugarDeResidencia", employee.Address);
            queryCommand.Parameters.AddWithValue("@archivo", GetFileBytes(employee.PhotoFile));
            queryCommand.Parameters.AddWithValue("@paisOrigen", employee.NativeCountry);
            queryCommand.Parameters.AddWithValue("@tipoFoto", employee.PhotoFile.ContentType);
            queryCommand.Parameters.AddWithValue("@fechaNacimiento", employee.DateOfBirth);

            connection.Open();
            bool employeeInsertSuccess = queryCommand.ExecuteNonQuery() >= 1;
            connection.Close();

            SqlCommand languageQueryCommand = new SqlCommand(languageQuery, connection);
            
            foreach (string language in employee.Languages) {
                languageQueryCommand.Parameters.AddWithValue("@cedula", employee.Dni);
                languageQueryCommand.Parameters.AddWithValue("@idioma", language);
                connection.Open();
                languageQueryCommand.ExecuteNonQuery();
                connection.Close();
            }
            bool languageInsertSuccess = true;

            if (employeeInsertSuccess && languageInsertSuccess) {
                employeeCreated = true;
            }

            return employeeCreated;
        }
    }
}