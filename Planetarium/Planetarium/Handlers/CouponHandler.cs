using System;
using System.Collections.Generic;
using Planetarium.Models;
using System.Data;

namespace Planetarium.Handlers {
    public class CouponHandler : DatabaseHandler {

        public List<CouponModel> GetAllCoupons(string visitorDni) {
            List<CouponModel> coupons = new List<CouponModel>();

            string query = "SELECT * FROM Cupon " +
                           "WHERE cedulaFK = '" + visitorDni + "' " +
                           "AND codigoPK != ''";
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

        public bool DeleteCoupon(string couponApplied) {
            string query = "DELETE FROM Cupon " +
                           "WHERE codigoPK = '" + couponApplied + "'";

            bool success = DatabaseQuery(query);

            return success;
        }

        public bool CheckCoupon(string coupon) {
            string query = "SELECT Count(*) AS RowsCount FROM Cupon " +
                           "WHERE codigoPK = '" + coupon + "'";

            DataTable resultingTable = CreateTableFromQuery(query);

            return Convert.ToInt32(resultingTable.Rows[0]["RowsCount"]) > 0;
        }

    }
}