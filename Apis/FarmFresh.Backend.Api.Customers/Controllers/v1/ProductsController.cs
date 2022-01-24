using AutoMapper;
using FarmFresh.Backend.Services.Interfaces;
using FarmFresh.Backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace FarmFresh.Backend.Api.Customers.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = GlobalConstants.CustomerUserRoleName)]
    public class ProductsController : ControllerBase
    {
        private readonly ICustomerServices _customerServices;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(
                ICustomerServices customerServices,
                IMapper mapper,
                ILogger<ProductsController> logger
            )
        {
            _customerServices = customerServices;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> Get(Guid productId)
        {
            _logger.LogInformation($"[GET] /api/v1/products/{productId}");
            var response = await _customerServices.GetProduct(productId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ProductListInput request)
        {
            _logger.LogInformation($"[GET] /api/v1/products => {JsonConvert.SerializeObject(request)}");
            var response = await _customerServices.GetProducts(request);
            return Ok(response);
        }

       

    }
}
