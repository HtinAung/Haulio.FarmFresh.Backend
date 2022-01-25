using AutoMapper;
using FarmFresh.Backend.Services.Interfaces;
using FarmFresh.Backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FarmFresh.Backend.Api.Stores.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = GlobalConstants.StoreAdminRoleName)]
    public class OrderHistoriesController : ControllerBase
    {
        private readonly IStoreServices _storeServices;
        private readonly ILogger<OrderHistoriesController> _logger;

        public OrderHistoriesController(
                IStoreServices storeServices,
                ILogger<OrderHistoriesController> logger
            )
        {
            _storeServices = storeServices;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] BaseListInput request)
        {
            request.Query = string.IsNullOrEmpty(request.Query) ? string.Empty : request.Query;
            string userId = User.FindFirstValue("sub");
            var user = await _storeServices.GetUserById(Guid.Parse(userId));
            _logger.LogInformation($"[GET] /api/v1/OrderHistories => {JsonConvert.SerializeObject(request)}");
            var response = await _storeServices.GetOrderHistories(user.StoreId.Value, request);
            return Ok(response);
        }
    }
}
