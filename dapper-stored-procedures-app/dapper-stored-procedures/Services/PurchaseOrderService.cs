using Dapper;
using dapper_stored_procedures.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace dapper_stored_procedures.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        public PurchaseOrderService()
        {

        }

        public IEnumerable<PurchaseOrder> GetPurchaseOrders(int supplierId)
        {
            using IDbConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=wwi;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            var purchaseOrders = connection.GetList<PurchaseOrder>("where SupplierID = @SupplierID", new { SupplierID = supplierId });
            return purchaseOrders;
        }
    }
}