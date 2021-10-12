using dapper_stored_procedures.ApiModels;
using dapper_stored_procedures.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dapper_stored_procedures.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly IPurchaseOrderService _purchaseOrderService;

        public PurchaseOrdersController(IPurchaseOrderService purchaseOrderService)
        {
            _purchaseOrderService = purchaseOrderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBySupplierId([FromQuery] int supplierId)
        {
            return Ok(await _purchaseOrderService.GetPurchaseOrders(supplierId));
        }

        [HttpGet]
        [Route("simple-paging")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int itemsPerPage = 10)
        {
            return Ok(await _purchaseOrderService.GetPurchaseOrdersPaged(pageNumber, itemsPerPage));
        }
        
        [HttpPost]
        [Route("complex-paging")]
        public async Task<IActionResult> GetComplexPaged(ComplexPaging complexPaging)
        {
            return Ok(await _purchaseOrderService.GetPurchaseOrdersPagedAndFiltered(complexPaging));
        }
    }
}
