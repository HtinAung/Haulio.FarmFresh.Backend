using AutoMapper;
using FarmFresh.Backend.Api.Customers.Models;
using FarmFresh.Backend.DataTransferObjects;
using FarmFresh.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
namespace FarmFresh.Backend.Api.Customers.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = GlobalConstants.CustomerUserRoleName)]

    public class OrdersController : ControllerBase
    {
        private readonly ICustomerServices _customerServices;
        private readonly IMapper _mapper;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(
                ICustomerServices customerServices,
                IMapper mapper,
                ILogger<OrdersController> logger
            )
        {   
            _customerServices = customerServices;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateOrderRequest request)
        {
            if (request.Total <= 0)
                throw new Exception("Invalid total order");
            _logger.LogInformation($"[POST] /api/v1/orders => {JsonConvert.SerializeObject(request)}");

            string userId = User.FindFirstValue("sub");
            var dto = _mapper.Map<OrderHistoryDto>(request);
            dto.UserId = Guid.Parse(userId);

            await _customerServices.MakeAnOrder(dto);
            return Ok();
        }
    }
}
