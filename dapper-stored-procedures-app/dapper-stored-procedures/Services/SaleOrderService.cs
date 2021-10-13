using Dapper;
using dapper_stored_procedures.ApiModels;
using dapper_stored_procedures.Models;
using dapper_stored_procedures.Predicates;
using dapper_stored_procedures.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
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
            var parameters = new DynamicParameters();
            dynamic objArgs = new ExpandoObject();
            ExpandoObjectUtils.AddProperty(objArgs, "offset", (complexPaging.PageNumber - 1) * complexPaging.ItemsPerPage);
            ExpandoObjectUtils.AddProperty(objArgs, "take", complexPaging.ItemsPerPage);

            var orderBy = string.Join(',', complexPaging.SortPredicates.Select(s => $"{s.Field} {s.Direction}").ToArray());
            if (!string.IsNullOrEmpty(orderBy))
                ExpandoObjectUtils.AddProperty(objArgs, "orderBy", orderBy);

            string predicate = string.Empty;
            if (complexPaging.FilterPredicates.Count() > 0)
            {
                var filtersPredicates = new List<FilterPredicate>();
                foreach (var currFilter in complexPaging.FilterPredicates)
                {
                    if (!filtersPredicates.Any(f => f.Field.Contains(currFilter.Field)))
                    {
                        filtersPredicates.Add(currFilter);
                    }
                    else
                    {
                        // get max num inserted up to this moment
                        var lastFilter = filtersPredicates.Where(f => f.Field.Contains(currFilter.Field)).LastOrDefault();
                        if (lastFilter.Field.IndexOf('_') == -1)
                        {
                            currFilter.Field = $"{currFilter.Field}_1";
                        }
                        else
                        {
                            currFilter.Field = $"{currFilter.Field}_{Int32.Parse(lastFilter.Field.Split('_')[1]) + 1}";
                        }
                        filtersPredicates.Add(currFilter);
                    }
                }

                complexPaging.FilterPredicates = filtersPredicates;

                predicate = $" WHERE{string.Join(' ', complexPaging.FilterPredicates.Select(f => $"{f.LogicalOperator} {f.Field.Split('_')[0]} {f.RelationalOperator} @{f.Field}"))}";

                foreach (var filter in complexPaging.FilterPredicates)
                {
                    ExpandoObjectUtils.AddProperty(objArgs, filter.Field, filter.LiteralToCompare);
                }

                ExpandoObjectUtils.AddProperty(objArgs, "predicate", predicate);
            }

            parameters.AddDynamicParams(objArgs);

            var saleOrders = await _connection.QueryAsync<SaleOrder>("usp.get_sale_order_complex_paging", param: parameters, commandType: CommandType.StoredProcedure);
            return saleOrders;
        }
    }
}
