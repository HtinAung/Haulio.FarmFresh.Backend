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
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = GlobalConstants.CustomerUserRoleName)]
    public class ProductsController : ControllerBase
    {
        private readonly ICustomerServices _customerServices;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(
                ICustomerServices customerServices,
                ILogger<ProductsController> logger
            )
        {
            _customerServices = customerServices;
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
            request.Query = string.IsNullOrEmpty(request.Query) ? string.Empty : request.Query;
            request.Category = string.IsNullOrEmpty(request.Category) ? string.Empty : request.Category;
            _logger.LogInformation($"[GET] /api/v1/products => {JsonConvert.SerializeObject(request)}");
            var response = await _customerServices.GetProducts(request);
            return Ok(response);
        }

       

    }
}
