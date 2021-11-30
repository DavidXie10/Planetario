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
            List<SouvenirModel> souvenirs = new List<SouvenirModel>();

            string query = "SELECT * FROM Articulo";
            DataTable resultingTable = CreateTableFromQuery(query);

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

            LinkAllSouvenirsWithImages(souvenirs);

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
    }
}