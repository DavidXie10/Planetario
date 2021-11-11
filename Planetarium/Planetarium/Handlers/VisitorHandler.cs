﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Planetarium.Models;
using static Planetarium.Handlers.DatabaseHandler;

namespace Planetarium.Handlers {
    public class VisitorHandler : DatabaseHandler {

        public bool RegisterVisitor(VisitorModel visitor, string activityTitle, string activityDate) {
            bool success = false;

            string query = "INSERT INTO Visitante (cedulaPK, nombreCompleto, correo, nivelEducativo, fechaNacimiento, genero, paisOrigen)" +
                           " VALUES (@cedula, @nombre, @correo, @nivelEducativo, @fechaNacimiento, @genero, @paisOrigen)";

            SqlCommand queryCommand = new SqlCommand(query, connection);
            AddParametersToQueryCommand(queryCommand, visitor);
            success = DatabaseQuery(queryCommand);

            success = InsertVisitor(visitor, activityTitle, activityDate);

            return success;
        }

        private void AddParametersToQueryCommand(SqlCommand queryCommand, VisitorModel visitor) {
            queryCommand.Parameters.AddWithValue("@cedula", visitor.Dni);
            queryCommand.Parameters.AddWithValue("@nombre", visitor.FullName);
            queryCommand.Parameters.AddWithValue("@correo", visitor.Mail);
            queryCommand.Parameters.AddWithValue("@nivelEducativo", visitor.EducationalLevel);
            queryCommand.Parameters.AddWithValue("@fechaNacimiento", visitor.DateOfBirth);
            queryCommand.Parameters.AddWithValue("@genero", visitor.Gender);
            queryCommand.Parameters.AddWithValue("@paisOrigen", visitor.NativeCountry);
        }

        public List<string> GetAllVisitorsDnis() {
            List<string> visitorsDnis = new List<string>();
            string query = "SELECT cedulaPK FROM Visitante";
            DataTable resultingTable = CreateTableFromQuery(query);

            foreach (DataRow rawEducationalInfo in resultingTable.Rows) {
                visitorsDnis.Add(Convert.ToString(rawEducationalInfo["cedulaPK"]));
            }

            return visitorsDnis;
        }

        public bool InsertVisitor(VisitorModel visitor, string activityTitle, string activityDate) {
            string query = "INSERT INTO Inscribirse (cedulaPKFK, tituloPKFK, fechaInicioPKFK)" +
                    " VALUES ('" + visitor.Dni + "', '" + activityTitle + "', '" + activityDate + "')";

            SqlCommand queryCommand = new SqlCommand(query, connection);
            bool success = DatabaseQuery(queryCommand);

            return success;
        }

        public bool CheckVisitor(string dni) {
            string query = "SELECT Count(*) AS RowsCount FROM Visitante " +
                           "WHERE cedulaPK = '" + dni + "'";

            DataTable resultingTable = CreateTableFromQuery(query);

            return Convert.ToInt32(resultingTable.Rows[0]["RowsCount"]) > 0;
        }

        public bool CheckVisitor(string dni, string title, string date) {           
            string query = "SELECT Count(*) AS RowsCount FROM Inscribirse " +
                           "WHERE cedulaPKFK = '" + dni + "' " +
                           "AND tituloPKFK = '" + title + "' " +
                           "AND fechaInicioPKFK = '" + date + "'";

            DataTable resultingTable = CreateTableFromQuery(query);

            return Convert.ToInt32(resultingTable.Rows[0]["RowsCount"]) > 0;
        }
    }
}