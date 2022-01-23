using AutoMapper;
using FarmFresh.Backend.Services.Interfaces;
using FarmFresh.Backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace FarmFresh.Backend.Api.Stores.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderHistoriesController : ControllerBase
    {
        private readonly IStoreServices _storeServices;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderHistoriesController> _logger;

        public OrderHistoriesController(
                IStoreServices storeServices,
                IMapper mapper,
                ILogger<OrderHistoriesController> logger
            )
        {
            _storeServices = storeServices;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{storeId}")]
        public async Task<IActionResult> Get(Guid storeId, [FromQuery] ProductListInput request)
        {
            _logger.LogInformation($"[GET] /api/v1/OrderHistories/{storeId.ToString()} => {JsonConvert.SerializeObject(request)}");
            var response = await _storeServices.GetOrderHistories(storeId, request);
            return Ok(response);
        }
    }
}
