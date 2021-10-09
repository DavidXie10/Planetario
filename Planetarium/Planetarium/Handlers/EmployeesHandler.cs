﻿using System;
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
                PhotoPath = Convert.ToString(employeeRawInfo["fotoPerfil"])
            };
        }

        private void LinkAllEmployeeWithLanguages(List<EmployeeModel> employees) {
            foreach (EmployeeModel employee in employees) {
                DataTable resultingTableOfEmployeeWithTheirLanguage = GetEmployeeWithLanguagesTable(employee.Dni);
                LinkScoopWithLanguages(employee, resultingTableOfEmployeeWithTheirLanguage);
            }
        }

        private DataTable GetEmployeeWithLanguagesTable(string employeeDni) {
            string query = "SELECT Idioma.idiomaPK FROM Funcionario " +
                        "INNER JOIN Idioma ON Funcionario.cedulaPK = Idioma.cedulaPK  " +
                        "WHERE Funcionario.cedulaPK = '" + employeeDni + "' ";

            return CreateTableFromQuery(query);
        }

        private void LinkScoopWithLanguages(EmployeeModel employee, DataTable resultingTable) {
            employee.Languages = new List<string>();
            foreach (DataRow column in resultingTable.Rows)
            {
                var tempLanguage = Convert.ToString(column["idiomaPK"]);
                employee.Languages.Add(tempLanguage);
            }
        }

        public bool CreateEmployee(EmployeeModel employee) {
            bool employeeCreated = false;
            string query = "INSERT INTO Funcionario(cedulaPK,ocupacion,titulosAcademicos,correo,nombre,apellido, frase, genero,fechaInicioEmpleo,fechaNacimiento,telefono,banderaColaborador,areaExpertiz,banderaCoordinador,banderaEducador,lugarDeResidencia,paisOrigen, fotoPerfil)" +
              "VALUES(@cedula,@ocupacion,@titulosAcademicos,@correo,@nombre,@apellido, @frase, @genero,'2000-02-02',@fechaNacimiento,@telefono,1,@areaExpertiz,0,0,@lugarDeResidencia,@paisOrigen,@fotoPerfil) ";

            SqlCommand queryCommand = new SqlCommand(query, connection);

            AddParametersToQueryCommand(queryCommand, employee);

            connection.Open();
            bool employeeInsertSuccess = queryCommand.ExecuteNonQuery() >= 1;
            connection.Close();
            bool languageInsertSuccess = InsertLanguages(employee);

            if (employeeInsertSuccess && languageInsertSuccess) {
                employeeCreated = true;
            }

            return employeeCreated;
        }


        private void AddParametersToQueryCommand(SqlCommand queryCommand, EmployeeModel employee) {
            queryCommand.Parameters.AddWithValue("@cedula", employee.Dni);
            queryCommand.Parameters.AddWithValue("@ocupacion", employee.Occupation);
            queryCommand.Parameters.AddWithValue("@titulosAcademicos", employee.AcademicDegree);
            queryCommand.Parameters.AddWithValue("@correo", employee.Mail);
            queryCommand.Parameters.AddWithValue("@nombre", employee.FirstName);
            queryCommand.Parameters.AddWithValue("@apellido", employee.LastName);
            queryCommand.Parameters.AddWithValue("@frase", employee.Phrase);
            queryCommand.Parameters.AddWithValue("@genero", employee.Gender);
            queryCommand.Parameters.AddWithValue("@fechaNacimiento", employee.DateOfBirth);
            queryCommand.Parameters.AddWithValue("@telefono", employee.PhoneNumber);
            queryCommand.Parameters.AddWithValue("@areaExpertiz", employee.ExpertiseArea);
            queryCommand.Parameters.AddWithValue("@lugarDeResidencia", employee.Address);
            queryCommand.Parameters.AddWithValue("@paisOrigen", employee.NativeCountry);
            queryCommand.Parameters.AddWithValue("@fotoPerfil", employee.PhotoFile.FileName.Replace(" ", "-").Replace("_", "-"));
        }

        public bool InsertLanguages(EmployeeModel employee) {
            bool sucess = false;
            foreach (string language in employee.Languages) {
                string languageQuery = "INSERT INTO Idioma (cedulaPK, idiomaPK)" +
                        "VALUES ('" + employee.Dni + "','" + language + "')";
                SqlCommand languageQueryCommand = new SqlCommand(languageQuery, connection);
                connection.Open();
                sucess = languageQueryCommand.ExecuteNonQuery() >= 1;
                connection.Close();
            }
            return sucess;
        }
    }
}