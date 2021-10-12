using dapper_stored_procedures.Models;
using System.Collections.Generic;

namespace dapper_stored_procedures.Services
{
    public interface IPurchaseOrderService
    {
        IEnumerable<PurchaseOrder> GetPurchaseOrders(int supplierId);
    }
}