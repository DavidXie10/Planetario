using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Planetarium.Models;

namespace Planetarium.Handlers
{
    public class EmployeesHandler
    {

        private SqlConnection connection;
        private string connectionRoute;

        public EmployeesHandler()
        {
            connectionRoute = ConfigurationManager.ConnectionStrings["EmployeeConnection"].ToString();
            connection = new SqlConnection(connectionRoute);
        }

        private DataTable CreateTableFromQuery(string query)
        {
            SqlCommand queryCommand = new SqlCommand(query, connection);
            SqlDataAdapter tableAdapter = new SqlDataAdapter(queryCommand);
            DataTable queryTable = new DataTable();
            connection.Open();
            tableAdapter.Fill(queryTable);
            connection.Close();
            return queryTable;
        }

        public List<EmployeesModel> GetAllEmployees()
        {
            List<EmployeesModel> employees = new List<EmployeesModel>();
            string query = "SELECT * FROM Funcionario ";
            DataTable resultingTable = CreateTableFromQuery(query);
            foreach (DataRow column in resultingTable.Rows)
            {
                employees.Add(
                    new EmployeesModel
                    {
                        //IdPhoto = Convert.ToString(column["fotoPerfil"]),
                        Name = Convert.ToString(column["nombre"]),
                        LastName = Convert.ToString(column["apellido"]),
                        AcademicDegree = Convert.ToString(column["titulosAcademicos"]),
                        //Occupation = Convert.ToString(column["ocupacion"]),
                        Mail = Convert.ToString(column["correo"]),
                        //Phrase = Convert.ToString(column["frase"]),
                    }
                );
            }

            return employees;
        }

        private byte[] GetFileBytes(HttpPostedFileBase file)
        {
            byte[] bytes;
            BinaryReader reader = new BinaryReader(file.InputStream);
            bytes = reader.ReadBytes(file.ContentLength);
            return bytes;
        }

        public bool CreateEmployee(EmployeesModel employee) 
        {

            string query = "INSERT INTO Funcionario(nombre, apellido, correo, cedulaPK, telefono, areaExpertiz, fechaNacimiento, lugarDeResidencia, titulosAcademicos, ocupacion, foto, fechaInicioEmpleo)" +
              "VALUES(@nombre, @apellido, @correo, @cedula, 000000000, @areaExpertiz, '2000-02-02', @lugarDeResidencia, @titulosAcademicos, 'pepe', CAST('empleado1' AS VARBINARY(MAX)), '2000-02-02')";
            // string query = "INSERT INTO Funcionario(nombre, apellido, correo, cedula, telefono, areaExpertiz, fechaNacimiento,lugarDeResidencia,titulosAcademicos)" + "VALUES (@nombre,@apellido,@correo,@cedula,@telefono,@areaExpertiz,@fechaNacimiento,@lugarDeResidencia,@titulosAcademicos)";

            SqlCommand queryCommand = new SqlCommand(query, connection);

            queryCommand.Parameters.AddWithValue("@nombre", employee.Name);
            queryCommand.Parameters.AddWithValue("@apellido", employee.LastName);
            queryCommand.Parameters.AddWithValue("@correo", employee.Mail );
            queryCommand.Parameters.AddWithValue("@cedula", employee.Dni );
            //queryCommand.Parameters.AddWithValue("@telefono", employee.PhoneNumber );
            queryCommand.Parameters.AddWithValue("@areaExpertiz", employee.ExpertiseArea );
            //queryCommand.Parameters.AddWithValue("@fechaNacimiento", employee.BirthDay );
            queryCommand.Parameters.AddWithValue("@lugarDeResidencia", employee.Address );
            queryCommand.Parameters.AddWithValue("@titulosAcademicos", employee.AcademicDegree);

            connection.Open();
            bool success = queryCommand.ExecuteNonQuery() >= 1;
            connection.Close();
            return success;
        }
    }
}