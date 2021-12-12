using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Planetarium.Models;

namespace Planetarium.Handlers {
    public class DataAnalyticsHandler : DatabaseHandler {
        public DataAnalyticsHandler() {
            connectionRoute = ConfigurationManager.ConnectionStrings[CONNECTION_NAME].ToString();
            connection = new SqlConnection(connectionRoute);
        }

        public List<ItemModel> GetSimpleItemReport(DateTime begin, DateTime end) {
            List<ItemModel> items = new List<ItemModel>();
            string query = "EXEC USP_obtenerVentasArticulos '" + begin + "','" + end + "'";
            DataTable resultingTable = CreateTableFromQuery(query);
            foreach (DataRow column in resultingTable.Rows) {
                items.Add(CreateItem(column));
            }
            return items;
        }

        private ItemModel CreateItem(DataRow column) {
            return new ItemModel {
                ID = Convert.ToInt32(column["ID"]),
                Name = Convert.ToString(column["Nombre"]),
                QuantitySold = Convert.ToInt32(column["Cantidad Vendida"]),
                LastBoughtDate = Convert.ToDateTime(column["Ultima fecha de compra"]),
                Category = Convert.ToString(column["Tipo de producto"]),
                Income = Convert.ToDouble(column["Ingresos"])
            };
        }

        public List<ItemAdvanceModel> GetAdvanceItemReport(DateTime begin, DateTime end) {
            List<ItemAdvanceModel> itemsAdvanced = new List<ItemAdvanceModel>();
            string query = "EXEC USP_obtenerVentasArticulosAvanzado '" + begin + "','" + end + "'";

            DataTable resultingTable = CreateTableFromQuery(query);
            foreach (DataRow column in resultingTable.Rows) {
                itemsAdvanced.Add(CreateItemAdvanced(column));
            }
            return itemsAdvanced;
        }

        private ItemAdvanceModel CreateItemAdvanced(DataRow column) {
            return new ItemAdvanceModel {
                ID = Convert.ToInt32(column["ID"]),
                Name = Convert.ToString(column["Nombre"]),
                Children = Convert.ToInt32(column["Infantil"]),
                Juvenile = Convert.ToInt32(column["Juvenil"]),
                Adult = Convert.ToInt32(column["Adulto"]),
                Senior = Convert.ToInt32(column["Adulto Mayor"]),
                Income = Convert.ToDouble(column["Ingresos"])
            };
        }


        public List<TicketModel> GetSimpleTicketReport(DateTime begin, DateTime end) {
            List<TicketModel> tickets = new List<TicketModel>();
            string query = "EXEC USP_obtenerVentasEntradas '" + begin + "','" + end + "'";

            DataTable resultingTable = CreateTableFromQuery(query);
            foreach (DataRow column in resultingTable.Rows) {
                tickets.Add(CreateTicket(column));
            }
            return tickets;
        }

        private TicketModel CreateTicket(DataRow column) {
            return new TicketModel {
                ActivityTitle = Convert.ToString(column["Titulo de la actividad"]),
                StartActivityDay = Convert.ToDateTime(column["Fecha de inicio de la actividad"]),
                QuantitySold = Convert.ToInt32(column["Cantidad de entradas vendidas"]),
                Income = Convert.ToDouble(column["Ingreso"])
            };
        }

        public List<TicketAdvanceModel> GetAdvanceTicketReport(DateTime begin, DateTime end) {
            List<TicketAdvanceModel> ticketsAdvanced = new List<TicketAdvanceModel>();
            string query = "EXEC USP_obtenerVentasEntradasAvanzado '" + begin + "','" + end + "'";

            DataTable resultingTable = CreateTableFromQuery(query);
            foreach (DataRow column in resultingTable.Rows) {
                ticketsAdvanced.Add(CreateTicketAdvance(column));
            }
            return ticketsAdvanced;
        }

        private TicketAdvanceModel CreateTicketAdvance(DataRow column) {
            return new TicketAdvanceModel {
                ActivityTitle = Convert.ToString(column["tituloPKFK"]),
                StartActivityDay = Convert.ToDateTime(column["fechaInicioPK"]),
                Children = Convert.ToInt32(column["Infantil"]),
                Juvenile = Convert.ToInt32(column["Juvenil"]),
                Adult = Convert.ToInt32(column["Adulto"]),
                Senior = Convert.ToInt32(column["Adulto Mayor"]),
                Income = Convert.ToDouble(column["Ingreso"])
            };
        }

    }
}