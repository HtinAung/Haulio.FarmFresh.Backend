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

    public class CategoriesController : ControllerBase
    {
        private readonly ICustomerServices _customerServices;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(
                ICustomerServices customerServices,
                ILogger<CategoriesController> logger
            )
        {
            _customerServices = customerServices;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] BaseListInput request)
        {
            _logger.LogInformation($"[GET] /api/v1/categories => {JsonConvert.SerializeObject(request)}");
            var response = await _customerServices.GetProductCategories(request);
            return Ok(response);
        }
    }
}
