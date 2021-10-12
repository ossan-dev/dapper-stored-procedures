using Dapper;
using dapper_stored_procedures.ApiModels;
using dapper_stored_procedures.Models;
using dapper_stored_procedures.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
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

        public async Task<IEnumerable<PurchaseOrder>> GetPurchaseOrdersPaged(int numPage, int itemsPerPage)
        {
            var purchaseOrders = await _connection.GetListPagedAsync<PurchaseOrder>(pageNumber: numPage, rowsPerPage: itemsPerPage, conditions: "", orderby: "");
            return purchaseOrders;
        }

        public async Task<IEnumerable<PurchaseOrder>> GetPurchaseOrdersPagedAndFiltered(ComplexPaging complexPaging)
        {
            // TODO: this version doesn't work with multiple filters on the same field
            string predicate = string.Empty;
            var parameters = new DynamicParameters();
            if (complexPaging.FilterPredicates.Count() > 0)
            {
                predicate = $"WHERE {string.Join(' ', complexPaging.FilterPredicates.Select(f => $"{f.LogicalOperator} {f.Field} {f.RelationalOperator} @{f.Field}"))}";
                dynamic predicateArgs = new ExpandoObject();
                foreach (var filter in complexPaging.FilterPredicates)
                {
                    ExpandoObjectUtils.AddProperty(predicateArgs, filter.Field, filter.LiteralToCompare);
                }

                parameters.AddDynamicParams(predicateArgs);
            }


            var orderBy = string.Join(',', complexPaging.SortPredicates.Select(s => $"{s.Field} {s.Direction}").ToArray());

            var purchaseOrders = await _connection.GetListPagedAsync<PurchaseOrder>(pageNumber: complexPaging.PageNumber, rowsPerPage: complexPaging.ItemsPerPage, conditions: predicate, orderby: orderBy, parameters);
            return purchaseOrders;
        }
    }
}