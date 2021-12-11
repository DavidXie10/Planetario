using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Planetarium.Models;

namespace Planetarium.Handlers {
    public class DataAnalyticsHandler : DatabaseHandler {
        public DataAnalyticsHandler() {
            connectionRoute = ConfigurationManager.ConnectionStrings[CONNECTION_NAME].ToString();
            connection = new SqlConnection(connectionRoute);
        }

        public List<ItemModel> GetItemReport(DateTime begin, DateTime end) {
            List<ItemModel> items = new List<ItemModel>();
            string query = "EXEC USP_obtenerVentasArticulos '" + begin + "','" + end + "'";
            DataTable resultingTable = CreateTableFromQuery(query);
            foreach (DataRow column in resultingTable.Rows) {
                items.Add(
                    new ItemModel {
                        ID = Convert.ToInt32(column["ID"]),
                        Name = Convert.ToString(column["Nombre"]),
                        QuantitySold = Convert.ToInt32(column["Cantidad Vendida"]),
                        LastBoughtDate = Convert.ToDateTime(column["Ultima fecha de compra"]),
                        Category = Convert.ToString(column["Tipo de producto"]),
                        Income = Convert.ToDouble(column["Ingresos"])
                    });
            }
            return items;
        }

        public List<TicketModel> GetTicketReport(DateTime begin, DateTime end) {
            List<TicketModel> items = new List<TicketModel>();
            string query = "EXEC USP_obtenerVentasEntradas '" + begin + "','" + end + "'";

            DataTable resultingTable = CreateTableFromQuery(query);
            foreach (DataRow column in resultingTable.Rows) {
                items.Add(
                    new TicketModel {
                        ActivityTitle = Convert.ToString(column["Nombre"]),
                        QuantitySold = Convert.ToInt32(column["Cantidad Vendida"]),
                        StartActivityDay = Convert.ToDateTime(column["Ultima fecha de compra"]),
                        Income = Convert.ToDouble(column["Ingresos"])
                    });
            }
            return items;
        }

    }
}