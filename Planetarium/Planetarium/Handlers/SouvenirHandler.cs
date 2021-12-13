using System;
using System.Collections.Generic;
using Planetarium.Models;
using System.Data;
using System.Data.SqlClient;

namespace Planetarium.Handlers {
    public class SouvenirHandler : DatabaseHandler {
        ContentParser contentParser { get;  set; }

        public SouvenirHandler() {
            contentParser = new ContentParser();
        }

        public List<SouvenirModel> GetAllItems() {
            string query = "SELECT * FROM Articulo";
            DataTable resultingTable = CreateTableFromQuery(query);

            List<SouvenirModel> souvenirs = CreateSouvenirs(resultingTable);

            LinkAllSouvenirsWithImages(souvenirs);

            return souvenirs;
        }

        private List<SouvenirModel> CreateSouvenirs(DataTable resultingTable) {
            List<SouvenirModel> souvenirs = new List<SouvenirModel>();

            foreach (DataRow column in resultingTable.Rows) {
                souvenirs.Add(CreateSouvenir(column));
            }

            return souvenirs;
        }

        private SouvenirModel CreateSouvenir(DataRow column) {
            return new SouvenirModel {
                SouvenirId = Convert.ToInt32(column["idPK"]),
                Name = Convert.ToString(column["nombre"]),
                Price = Convert.ToDouble(column["precio"]),
                Stock = Convert.ToInt32(column["cantidad"]),
                Category = Convert.ToString(column["categoria"]),
                Description = Convert.ToString(column["descripcion"])
            };
        }

        private void LinkAllSouvenirsWithImages(List<SouvenirModel> souvenirs) {
            foreach (SouvenirModel souvenir in souvenirs) {
                DataTable souvenirImages = GetSouvenirWithImagesTable(souvenir.SouvenirId);
                if (souvenirImages != null)
                    LinkScoopWithImages(souvenir, souvenirImages);
            }
        }

        private DataTable GetSouvenirWithImagesTable(int scoopId) {
            string query = "SELECT rutasImagenes FROM Articulo " +
                            "WHERE idPK = '" + scoopId + "' ";
            return CreateTableFromQuery(query);
        }

        private void LinkScoopWithImages(SouvenirModel souvenir, DataTable souvenirImages) {
            souvenir.ImagesRef = contentParser.GetListFromString(Convert.ToString(souvenirImages.Rows[0]["rutasImagenes"]));
        }

        public bool UpdateSouvernirStock(int souvernirId, int newStock) {
            string query = "UPDATE Articulo " +
                           "SET cantidad = " + newStock + " " +
                           "WHERE idPK = " + souvernirId;

            return DatabaseQuery(query);
        }

        public List<SouvenirModel> GetSelectedSouvenirs(Dictionary<string, int> souvenirIds) {
            string query = "SELECT * FROM Articulo WHERE ";
            foreach(string souvenirId in souvenirIds.Keys) {
                query += "idPK = " + souvenirId + " OR ";
            }

            query = query.Substring(0, query.LastIndexOf("OR"));

            DataTable resultingTable = CreateTableFromQuery(query);

            List<SouvenirModel> selectedSouvenirs = CreateSouvenirs(resultingTable);

            LinkSelectedSouvenirsWithCount(selectedSouvenirs, souvenirIds);

            LinkAllSouvenirsWithImages(selectedSouvenirs);

            return selectedSouvenirs;
        }

        private void LinkSelectedSouvenirsWithCount(List<SouvenirModel> selectedSouvenirs, Dictionary<string, int> souvenirIds) {
            foreach (SouvenirModel souvenir in selectedSouvenirs) {
                souvenir.SelectedCount = souvenirIds[souvenir.SouvenirId.ToString()];
            }
        }


        public int RegisterSale(List<SouvenirModel> souvenirs, double totalPrice, DateTime date, string visitorDni) {
            string query = "INSERT INTO VentaRealizada(precioTotal, fechaCompra, cedulaFK) " +
                           "VALUES (@precio, @fechaCompra, @visitante)";

            SqlCommand queryCommand = new SqlCommand(query, connection);
            AddParametersToQueryCommand(queryCommand, totalPrice, date, visitorDni);
            DatabaseQuery(queryCommand);

            query = "SELECT IDENT_CURRENT('VentaRealizada') ";
            DataTable resultingTable = CreateTableFromQuery(query);

            int saleId = Convert.ToInt32(resultingTable.Rows[0][0]);

            foreach (SouvenirModel souvenir in souvenirs) {
                RegisterSouvenirSale(saleId, souvenir);
            }
            return saleId;
        }

        private void AddParametersToQueryCommand(SqlCommand queryCommand, double totalPrice, DateTime date, string visitorDni) {
            queryCommand.Parameters.AddWithValue("@precio", totalPrice);
            queryCommand.Parameters.AddWithValue("@fechaCompra", date);
            queryCommand.Parameters.AddWithValue("@visitante", visitorDni);
        }

        private void RegisterSouvenirSale(int saleId, SouvenirModel souvenir) {
            string query = "INSERT INTO DescripcionVentaRealizada(ventaRealizadaIdPKFK, precio, cantidad, idProductoFK) " +
                           "VALUES (@ventaId, @precio, @cantidad, @productoId)";

            SqlCommand queryCommand = new SqlCommand(query, connection);
            AddParametersToQueryCommand(queryCommand, saleId, souvenir);
            DatabaseQuery(queryCommand);
        }

        private void AddParametersToQueryCommand(SqlCommand queryCommand, int saleId, SouvenirModel souvenir) {
            queryCommand.Parameters.AddWithValue("@ventaId", saleId);
            queryCommand.Parameters.AddWithValue("@precio", souvenir.Price * souvenir.SelectedCount);
            queryCommand.Parameters.AddWithValue("@cantidad", souvenir.SelectedCount);
            queryCommand.Parameters.AddWithValue("@productoId", souvenir.SouvenirId);
        }

        public bool UpdateSelectedItemsStock(List<SouvenirModel> selectedSouvenirs) {
            bool success = false;

            foreach (SouvenirModel souvenir in selectedSouvenirs) {
                success = UpdateSouvernirStock(souvenir.SouvenirId, souvenir.Stock - souvenir.SelectedCount);
            }

            return success;
        }
    }
}