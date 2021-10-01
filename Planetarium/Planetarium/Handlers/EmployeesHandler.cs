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

        public List<EmployeeModel> GetAllEmployees()
        {
            List<EmployeeModel> employees = new List<EmployeeModel>();
            string query = "SELECT * FROM Funcionario ";
            DataTable resultingTable = CreateTableFromQuery(query);
            foreach (DataRow column in resultingTable.Rows)
            {
                employees.Add(
                    new EmployeeModel
                    {
                        //IdPhoto = Convert.ToString(column["fotoPerfil"]),
                        FirstName = Convert.ToString(column["nombre"]),
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

        public bool CreateEmployee(EmployeeModel employee) 
        {

            string query = "INSERT INTO Funcionario(cedulaPK,ocupacion,titulosAcademicos,foto,correo,nombre,apellido,genero,fechaInicioEmpleo,fechaNacimiento,telefono,banderaColaborador,areaExpertiz,banderaCoordinador,banderaEducador,lugarDeResidencia,fotoPerfil,paisDeOrigen)" +
              "VALUES(@cedula,@ocupacion,@titulosAcademicos,CAST('empleado1' AS VARBINARY(MAX)),@correo,@nombre,@apellido,'M','2000-02-02','2000-02-02',@telefono,1,@areaExpertiz,0,0,@lugarDeResidencia,CAST('empleado1' AS VARBINARY(MAX)),'CR') "; 
            // string query = "INSERT INTO Funcionario(nombre, apellido, correo, cedula, telefono, areaExpertiz, fechaNacimiento,lugarDeResidencia,titulosAcademicos)" + "VALUES (@nombre,@apellido,@correo,@cedula,@telefono,@areaExpertiz,@fechaNacimiento,@lugarDeResidencia,@titulosAcademicos)";

            SqlCommand queryCommand = new SqlCommand(query, connection);

            queryCommand.Parameters.AddWithValue("@cedula", employee.Dni);
            queryCommand.Parameters.AddWithValue("@ocupacion", employee.Occupation);
            queryCommand.Parameters.AddWithValue("@titulosAcademicos", employee.AcademicDegree);
            queryCommand.Parameters.AddWithValue("@correo", employee.Mail );
            queryCommand.Parameters.AddWithValue("@nombre", employee.FirstName);
            queryCommand.Parameters.AddWithValue("@apellido", employee.LastName);
            queryCommand.Parameters.AddWithValue("@telefono", employee.PhoneNumber );
            queryCommand.Parameters.AddWithValue("@areaExpertiz", employee.ExpertiseArea );
            queryCommand.Parameters.AddWithValue("@lugarDeResidencia", employee.Address );

            connection.Open();
            bool success = queryCommand.ExecuteNonQuery() >= 1;
            connection.Close();
            Console.WriteLine(queryCommand.ToString());
            return success;
        }
    }
}