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
    public class SalesOrdersController : ControllerBase
    {
        private readonly ISaleOrderService _saleOrderService;

        public SalesOrdersController(ISaleOrderService saleOrderService)
        {
            _saleOrderService = saleOrderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetByCustomerId([FromQuery] int customerId)
        {
            return Ok(await _saleOrderService.GetSaleOrders(customerId));
        }
    }
}
