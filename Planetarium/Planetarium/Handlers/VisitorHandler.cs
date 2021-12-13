using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Planetarium.Models;
using System.Linq;

namespace Planetarium.Handlers {
    public class VisitorHandler : DatabaseHandler {

        public bool RegisterVisitor(VisitorModel visitor) {
            bool success = false;

            string query = "INSERT INTO Visitante (cedulaPK, nombreCompleto, correo, nivelEducativo, fechaNacimiento, genero, paisOrigen)" +
                           " VALUES (@cedula, @nombre, @correo, @nivelEducativo, @fechaNacimiento, @genero, @paisOrigen)";

            SqlCommand queryCommand = new SqlCommand(query, connection);
            AddParametersToQueryCommand(queryCommand, visitor);
            success = DatabaseQuery(queryCommand);
            
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
        public VisitorModel GetVisitorByDni(string dni, bool getDefault) {
            VisitorModel visitorConfirmed = null;
            if (CheckVisitor(dni)) {
                visitorConfirmed = GetVisitorFromDatabase(dni);
            } else if (getDefault) {
                visitorConfirmed = GetDefaultVisitor();
            }
            return visitorConfirmed;
        }

        public VisitorModel GetVisitorFromDatabase(string dni) {
            string query = "SELECT * " +
                            "FROM Visitante " +
                            "WHERE cedulaPK = '" + dni + "'";
            DataTable resultingTable = CreateTableFromQuery(query);
            DataRow visitorInstance = resultingTable.Rows[0];
            return GetVisitorContentFromTable(visitorInstance);
        }

        public VisitorModel GetVisitorContentFromTable(DataRow dbContent) {
            return new VisitorModel  {
                Dni = Convert.ToString(dbContent["cedulaPK"]),
                FullName = Convert.ToString(dbContent["nombreCompleto"]),
                Mail = Convert.ToString(dbContent["correo"]),
                EducationalLevel = Convert.ToString(dbContent["niveleducativo"]),
                Gender = Convert.ToChar(dbContent["genero"]),
                DateOfBirth = Convert.ToDateTime(dbContent["fechaNacimiento"]),
                NativeCountry = Convert.ToString(dbContent["paisOrigen"])
            };
        }

        public VisitorModel GetDefaultVisitor() {
            return new VisitorModel {
                Dni = "0",
                FullName = "John Doe",
                Mail = "johnDoe@email.com",
                Gender = 'O',
                EducationalLevel = null,
                DateOfBirth = DateTime.Now,
                NativeCountry = null
            };
        }

        public bool InsertVisitor(string visitorDni, string activityTitle, string activityDate, string seat, double price, string targetAudience) {

            int ticketId = CreateTicket(seat, price, targetAudience);
            string query = "INSERT INTO Inscribirse (cedulaPKFK, tituloPKFK, fechaInicioPKFK, idEntradaPKFK)" +
                    " VALUES (@idVisitante,@tituloActividad, @fechaInicio, @idEntrada)";

            SqlCommand queryCommand = new SqlCommand(query, connection);
            queryCommand.Parameters.AddWithValue("@idVisitante", visitorDni);
            queryCommand.Parameters.AddWithValue("@tituloActividad", activityTitle);
            queryCommand.Parameters.AddWithValue("@fechaInicio", activityDate);
            queryCommand.Parameters.AddWithValue("@idEntrada", ticketId);
            bool success = DatabaseQuery(queryCommand);

            return success;
        }

        public bool InsertVariousVisitors(string visitorDni, string activityTitle, string activityDate, double price, string seats, string seatTypes) {
            bool success = false;
            int childSeats = Convert.ToInt32(seatTypes.Split(',')[0]);
            int adultSeats = Convert.ToInt32(seatTypes.Split(',')[1]);
            int seniorSeats = Convert.ToInt32(seatTypes.Split(',')[2]);
            List<string> selectedSeats = CheckListValue(seats.Split(',').ToList());
            int totalSeats = selectedSeats.Count();
            price = price / totalSeats;
            while (selectedSeats.Count > 0) {
                if (childSeats > 0) {
                    InsertVisitor(visitorDni, activityTitle, activityDate, selectedSeats[0], price, "Infantil");
                    selectedSeats.RemoveAt(0);
                    childSeats--;
                }

                if (adultSeats > 0) {
                    InsertVisitor(visitorDni, activityTitle, activityDate, selectedSeats[0], price, "Adulto");
                    selectedSeats.RemoveAt(0);
                    adultSeats--;
                }

                if (seniorSeats > 0) {
                    InsertVisitor(visitorDni, activityTitle, activityDate, selectedSeats[0], price, "Adulto Mayor");
                    selectedSeats.RemoveAt(0);
                    seniorSeats--;
                }
            }
            success = true;
            return success;
        }

        public List<string> CheckListValue(List<string> list) {
            List<string> checkedList = new List<string>();
            foreach(string element in list) {
                if(element != "") {
                    checkedList.Add(element);
                }
            }
            return checkedList;
        }

        private int CreateTicket(string seat, double price, string targetAudience) {
            string query = "INSERT INTO Entrada(precio, numeroAsiento, publicoMeta) " +
                           "VALUES(@precio,@numeroAsiento,@publicoMeta) ";

            SqlCommand queryCommand = new SqlCommand(query, connection);
            queryCommand.Parameters.AddWithValue("@precio", price);
            queryCommand.Parameters.AddWithValue("@numeroAsiento", seat);
            queryCommand.Parameters.AddWithValue("@publicoMeta", targetAudience);
            DatabaseQuery(queryCommand);

            query = "SELECT IDENT_CURRENT('Entrada') ";
            DataTable resultingTable = CreateTableFromQuery(query);

            return Convert.ToInt32(resultingTable.Rows[0][0]);
        }

        public bool InsertAssignSeat(string dni, string activityTitle, string activityDate, string seat) {
            string query = "INSERT INTO AsignarAsiento (tituloPKFK, fechaInicioPKFK, cedulaVisitantePKFK, numeroAsiento)" +
                    " VALUES ('" + activityTitle + "', '" + activityDate + "', '" + dni + "', '" + seat + "')";

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