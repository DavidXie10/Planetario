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
                        IdPhoto = Convert.ToString(column["foto"]),
                        Name = Convert.ToString(column["nombre"]),
                        LastName = Convert.ToString(column["apellido"]),
                        AcademicDegree = Convert.ToString(column["titulosAcademicos"]),
                        Occupation = Convert.ToString(column["ocupacion"]),
                        Mail = Convert.ToString(column["correo"]),
                        Phrase = Convert.ToString(column["frase"]),
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
    }
}