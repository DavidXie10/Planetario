using System;
using System.Collections.Generic;
using Planetarium.Models;
using System.Data;

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
                souvenirs.Add(
                    new SouvenirModel {
                        SouvenirId = Convert.ToInt32(column["idPK"]),
                        Name = Convert.ToString(column["nombre"]),
                        Price = Convert.ToDouble(column["precio"]),
                        Stock = Convert.ToInt32(column["cantidad"]), 
                        Category = Convert.ToString(column["categoria"]),
                        Description = Convert.ToString(column["descripcion"])
                    }
                );
            }

            return souvenirs;
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
                query += "idPK = " + souvenirId + " AND ";
            }

            query = query.Substring(0, query.LastIndexOf("AND") - 1);

            DataTable resultingTable = CreateTableFromQuery(query);

            List<SouvenirModel> selectedSouvenirs = CreateSouvenirs(resultingTable);

            LinkSelectedSouvenirsWithCount(selectedSouvenirs, souvenirIds);

            LinkAllSouvenirsWithImages(selectedSouvenirs);

            return selectedSouvenirs;
        }

        private void LinkSelectedSouvenirsWithCount(List<SouvenirModel> selectedSouvenirs, Dictionary<string, int>  souvenirIds) {
            for(int index = 0; index < selectedSouvenirs.Count; ++index) {
                selectedSouvenirs[index].SelectedCount = souvenirIds[index.ToString()];
            }
        }
    }
}