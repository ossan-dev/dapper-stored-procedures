using Dapper;
using dapper_stored_procedures.ApiModels;
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
            var saleOrders = await _connection.QueryAsync<SaleOrder>("usp.get_sale_order_by_customer_id", new { customerId = customerId }, commandType: CommandType.StoredProcedure);
            return saleOrders;
        }

        public async Task<IEnumerable<SaleOrder>> GetSaleOrdersPaged(int numPage, int itemsPerPage)
        {
            var saleOrders = await _connection.QueryAsync<SaleOrder>("usp.get_sale_order_simple_paging", new { offset = (numPage - 1) * itemsPerPage, take = itemsPerPage }, commandType: CommandType.StoredProcedure);
            return saleOrders;
        }

        public async Task<IEnumerable<SaleOrder>> GetSaleOrdersPagedAndFiltered(ComplexPaging complexPaging)
        {
            var orderBy = string.Join(',', complexPaging.SortPredicates.Select(s => $"{s.Field} {s.Direction}").ToArray());
            string predicate = string.Empty;
            if (complexPaging.FilterPredicates.Count() > 0)
            {
                predicate = $" WHERE {string.Join(' ', complexPaging.FilterPredicates.Select(f => $"{f.LogicalOperator} {f.Field} {f.RelationalOperator} {f.LiteralToCompare}"))}";
            }

            var saleOrders = await _connection.QueryAsync<SaleOrder>("usp.get_sale_order_complex_paging", new { offset = (complexPaging.PageNumber - 1) * complexPaging.ItemsPerPage, take = complexPaging.ItemsPerPage, orderBy = orderBy, predicate = predicate}, commandType: CommandType.StoredProcedure);
            return saleOrders;
        }
    }
}
