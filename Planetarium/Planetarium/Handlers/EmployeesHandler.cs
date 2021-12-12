using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Planetarium.Models;

namespace Planetarium.Handlers {
    public class EmployeesHandler : DatabaseHandler {

        public List<EmployeeModel> GetAllEmployees() {
            List<EmployeeModel> employees = new List<EmployeeModel>();
            string query = "SELECT * FROM Funcionario ORDER BY nombre ";
            DataTable resultingTable = CreateTableFromQuery(query);
            foreach (DataRow column in resultingTable.Rows) {
                employees.Add(CreateEmployee(column));
            }
            LinkAllEmployeeWithLanguages(employees);

            return employees;
        }

        private EmployeeModel CreateEmployee(DataRow employeeRawInfo) {
            return new EmployeeModel {
                Dni = Convert.ToString(employeeRawInfo["cedulaPK"]),
                FirstName = Convert.ToString(employeeRawInfo["nombre"]),
                LastName = Convert.ToString(employeeRawInfo["apellido"]),
                AcademicDegree = Convert.ToString(employeeRawInfo["titulosAcademicos"]),
                Occupation = Convert.ToString(employeeRawInfo["ocupacion"]),
                Mail = Convert.ToString(employeeRawInfo["correo"]),
                PhoneNumber = Convert.ToInt32(employeeRawInfo["telefono"]),
                Gender = Convert.ToChar(employeeRawInfo["genero"]),
                ExpertiseArea = Convert.ToString(employeeRawInfo["areaExpertiz"]),
                NativeCountry = Convert.ToString(employeeRawInfo["paisOrigen"]),
                DateOfBirth = Convert.ToDateTime(employeeRawInfo["fechaNacimiento"]),
                PhotoPath = Convert.ToString(employeeRawInfo["rutaFotoPerfil"])
            };
        }

        private void LinkAllEmployeeWithLanguages(List<EmployeeModel> employees) {
            foreach (EmployeeModel employee in employees) {
                DataTable resultingTableOfEmployeeWithTheirLanguage = GetEmployeeWithLanguagesTable(employee.Dni);
                LinkEmployeeWithLanguages(employee, resultingTableOfEmployeeWithTheirLanguage);
            }
        }

        private DataTable GetEmployeeWithLanguagesTable(string employeeDni) {
            string query = "SELECT I.idiomaPK " +
                           "FROM Idioma AS I " +
                           "WHERE I.cedulaPKFK IN(SELECT F.cedulaPK " +
                                                 "FROM Funcionario AS F " +
                                                 "WHERE F.cedulaPK = '" + employeeDni + "')";

            return CreateTableFromQuery(query);
        }

        private void LinkEmployeeWithLanguages(EmployeeModel employee, DataTable resultingTable) {
            employee.Languages = new List<string>();
            foreach (DataRow column in resultingTable.Rows) {
                var tempLanguage = Convert.ToString(column["idiomaPK"]);
                employee.Languages.Add(tempLanguage);
            }
        }

        public bool CreateEmployee(EmployeeModel employee) {
            bool employeeCreated = false;
            string query = "INSERT INTO Funcionario(cedulaPK,ocupacion,titulosAcademicos,correo,nombre,apellido, genero,fechaNacimiento,telefono,banderaColaborador,areaExpertiz,banderaCoordinador,banderaEducador,paisOrigen, rutaFotoPerfil)" +
              "VALUES(@cedula,@ocupacion,@titulosAcademicos,@correo,@nombre,@apellido, @genero,@fechaNacimiento,@telefono,1,@areaExpertiz,0,0,@paisOrigen,@rutaFotoPerfil) ";

            SqlCommand queryCommand = new SqlCommand(query, connection);

            AddParametersToQueryCommand(queryCommand, employee);

            employeeCreated = DatabaseQuery(queryCommand);

            bool languageInsertSuccess = InsertLanguages(employee);

            if (employeeCreated && languageInsertSuccess) {
                employeeCreated = true;
            }

            return employeeCreated;
        }

        private void AddParametersToQueryCommand(SqlCommand queryCommand, EmployeeModel employee) {
            queryCommand.Parameters.AddWithValue("@cedula", employee.Dni);
            queryCommand.Parameters.AddWithValue("@ocupacion", employee.Occupation);
            queryCommand.Parameters.AddWithValue("@titulosAcademicos", employee.AcademicDegree == null ? "" : employee.AcademicDegree);
            queryCommand.Parameters.AddWithValue("@correo", employee.Mail == null ? "" : employee.Mail );
            queryCommand.Parameters.AddWithValue("@nombre", employee.FirstName);
            queryCommand.Parameters.AddWithValue("@apellido", employee.LastName);
            queryCommand.Parameters.AddWithValue("@genero", employee.Gender == 'N' ? 'O' : employee.Gender);
            queryCommand.Parameters.AddWithValue("@fechaNacimiento", employee.DateOfBirth);
            queryCommand.Parameters.AddWithValue("@telefono", employee.PhoneNumber == 0 ? 0 : employee.PhoneNumber);
            queryCommand.Parameters.AddWithValue("@areaExpertiz", employee.ExpertiseArea == null ? "" : employee.ExpertiseArea);
            queryCommand.Parameters.AddWithValue("@paisOrigen", employee.NativeCountry == null ? "" : employee.NativeCountry);
            queryCommand.Parameters.AddWithValue("@rutaFotoPerfil", employee.PhotoFile.FileName.Replace(" ", "-").Replace("_", "-"));
        }

        public bool InsertLanguages(EmployeeModel employee) {
            bool success = false;
            foreach (string language in employee.Languages) {
                string languageQuery = "INSERT INTO Idioma (cedulaPKFK, idiomaPK)" +
                        "VALUES ('" + employee.Dni + "','" + language + "')";
                success = DatabaseQuery(languageQuery);
            }
            return success;
        }

        public List<string> GetEmployeesLanguages() {
            List<string> languages = new List<string>();
            string languageQuery = "SELECT DISTINCT I.idiomaPK " +
                                    "FROM Idioma AS I " +
                                    "ORDER BY I.idiomaPK; ";
            DataTable resultingTable = CreateTableFromQuery(languageQuery);
            
            foreach (DataRow column in resultingTable.Rows) {
                languages.Add(Convert.ToString(column["idiomaPK"]));
            }

            return languages;
        }
    }
}