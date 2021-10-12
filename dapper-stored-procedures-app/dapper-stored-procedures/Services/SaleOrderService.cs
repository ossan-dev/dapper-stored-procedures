using Dapper;
using dapper_stored_procedures.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace dapper_stored_procedures.Services
{
    public class SaleOrderService : ISaleOrderService
    {
        private readonly IDbConnection _connection;

        public SaleOrderService(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<SaleOrder>> GetSaleOrders(int customerId)
        {
            var purchaseOrders = await _connection.QueryAsync<SaleOrder>("usp.get_sale_order_by_customer_id", new { customerId = customerId }, commandType: CommandType.StoredProcedure);
            return purchaseOrders;
        }
    }
}
