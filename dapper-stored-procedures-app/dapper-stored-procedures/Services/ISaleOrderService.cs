using dapper_stored_procedures.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dapper_stored_procedures.Services
{
    public interface ISaleOrderService
    {
        Task<IEnumerable<SaleOrder>> GetSaleOrders(int customerId);
    }
}