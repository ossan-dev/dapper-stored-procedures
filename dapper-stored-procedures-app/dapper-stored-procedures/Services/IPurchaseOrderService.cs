using dapper_stored_procedures.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dapper_stored_procedures.Services
{
    public interface IPurchaseOrderService
    {
        Task<IEnumerable<PurchaseOrder>> GetPurchaseOrders(int supplierId);
        Task<IEnumerable<PurchaseOrder>> GetPurchaseOrdersPaged(int numPage, int itemsPerPage);
    }
}