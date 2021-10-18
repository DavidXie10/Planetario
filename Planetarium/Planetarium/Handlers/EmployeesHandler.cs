using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
                Address = Convert.ToString(employeeRawInfo["lugarDeResidencia"]),
                NativeCountry = Convert.ToString(employeeRawInfo["paisOrigen"]),
                DateOfBirth = Convert.ToDateTime(employeeRawInfo["fechaNacimiento"]),
                Phrase = Convert.ToString(employeeRawInfo["frase"]),
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
            string query = "SELECT Idioma.idiomaPK FROM Funcionario " +
                        "INNER JOIN Idioma ON Funcionario.cedulaPK = Idioma.cedulaPKFK  " +
                        "WHERE Funcionario.cedulaPK = '" + employeeDni + "' ";

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
            string query = "INSERT INTO Funcionario(cedulaPK,ocupacion,titulosAcademicos,correo,nombre,apellido, frase, genero,fechaInicioEmpleo,fechaNacimiento,telefono,banderaColaborador,areaExpertiz,banderaCoordinador,banderaEducador,lugarDeResidencia,paisOrigen, rutaFotoPerfil)" +
              "VALUES(@cedula,@ocupacion,@titulosAcademicos,@correo,@nombre,@apellido, @frase, @genero,'2000-02-02',@fechaNacimiento,@telefono,1,@areaExpertiz,0,0,@lugarDeResidencia,@paisOrigen,@rutaFotoPerfil) ";

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
            queryCommand.Parameters.AddWithValue("@frase", employee.Phrase == null ? "" : employee.Phrase);
            queryCommand.Parameters.AddWithValue("@genero", employee.Gender == 'N' ? 'O' : employee.Gender);
            queryCommand.Parameters.AddWithValue("@fechaNacimiento", employee.DateOfBirth);
            queryCommand.Parameters.AddWithValue("@telefono", employee.PhoneNumber == 0 ? 0 : employee.PhoneNumber);
            queryCommand.Parameters.AddWithValue("@areaExpertiz", employee.ExpertiseArea == null ? "" : employee.ExpertiseArea);
            queryCommand.Parameters.AddWithValue("@lugarDeResidencia", employee.Address == null ? "" : employee.Address);
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
    }
}