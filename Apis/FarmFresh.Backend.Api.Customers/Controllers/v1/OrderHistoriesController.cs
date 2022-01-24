using FarmFresh.Backend.Services.Interfaces;
using FarmFresh.Backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FarmFresh.Backend.Api.Customers.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = GlobalConstants.CustomerUserRoleName)]
    public class OrderHistoriesController : ControllerBase
    {
        private readonly ICustomerServices _customerServices;
        private readonly ILogger<OrderHistoriesController> _logger;

        public OrderHistoriesController(
                ICustomerServices customerServices,
                ILogger<OrderHistoriesController> logger
            )
        {
            _customerServices = customerServices;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] BaseListInput request)
        {
            string userId = User.FindFirstValue("sub");
            _logger.LogInformation($"[GET] /api/v1/OrderHistories => {JsonConvert.SerializeObject(request)}");
            var response = await _customerServices.GetOrderHistories(Guid.Parse(userId), request);
            return Ok(response);
        }
    }
}
