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
    [Authorize(Roles = GlobalConstants.StoreAdminRoleName)]
    public class CategoriesController : ControllerBase
    {
        private readonly IStoreServices _storeServices;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(
                IStoreServices storeServices,
                ILogger<CategoriesController> logger
            )
        {
            _storeServices = storeServices;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]BaseListInput request)
        {
            request.Query = string.IsNullOrEmpty(request.Query) ? string.Empty : request.Query;
            _logger.LogInformation($"[GET] /api/v1/categories => {JsonConvert.SerializeObject(request)}");
            var response = await _storeServices.GetProductCategories(request);
            return Ok(response);
        }
    }
}
