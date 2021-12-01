using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Planetarium.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Planetarium.Handlers {
    public class CouponHandler : DatabaseHandler {

        public List<CouponModel> GetAllCoupons(string visitorDni) {
            List<CouponModel> coupons = new List<CouponModel>();

            string query = "SELECT * FROM Cupon " +
                           "WHERE cedulaFK = '" + visitorDni + "'";
            DataTable resultingTable = CreateTableFromQuery(query);

            foreach (DataRow column in resultingTable.Rows) {
                coupons.Add(
                    new CouponModel {
                        Code = Convert.ToString(column["codigoPK"]),
                        Description = Convert.ToString(column["descripcion"]),
                        Date = Convert.ToDateTime(column["fechaVencimiento"]),
                        VisitorDni = Convert.ToString(column["cedulaFK"]),
                        Discount = Convert.ToDouble(column["descuento"])
                    }
                );
            }

            return coupons;
        }

    }
}