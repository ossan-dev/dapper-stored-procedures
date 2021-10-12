using Dapper;
using dapper_stored_procedures.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace dapper_stored_procedures.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IDbConnection _connection;

        public PurchaseOrderService(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<PurchaseOrder>> GetPurchaseOrders(int supplierId)
        {
            var purchaseOrders = await _connection.GetListAsync<PurchaseOrder>("where SupplierID = @SupplierID", new { SupplierID = supplierId });
            return purchaseOrders;
        }
    }
}