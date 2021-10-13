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
    public class SaleOrdersController : ControllerBase
    {
        private readonly ISaleOrderService _saleOrderService;

        public SaleOrdersController(ISaleOrderService saleOrderService)
        {
            _saleOrderService = saleOrderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetByCustomerId([FromQuery] int customerId)
        {
            return Ok(await _saleOrderService.GetSaleOrders(customerId));
        }
        
        [HttpGet]
        [Route("simple-paging")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int itemsPerPage = 10)
        {
            return Ok(await _saleOrderService.GetSaleOrdersPaged(pageNumber, itemsPerPage));
        }

        [HttpPost]
        [Route("complex-paging")]
        public async Task<IActionResult> GetComplexPaged(ComplexPaging complexPaging)
        {
            return Ok(await _saleOrderService.GetSaleOrdersPagedAndFiltered(complexPaging));
        }
    }
}
