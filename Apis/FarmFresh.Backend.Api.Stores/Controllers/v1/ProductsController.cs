using AutoMapper;
using FarmFresh.Backend.Api.Stores.Models;
using FarmFresh.Backend.DataTransferObjects;
using FarmFresh.Backend.Services.Interfaces;
using FarmFresh.Backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FarmFresh.Backend.Api.Stores.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IStoreServices _storeServices;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(
                IStoreServices storeServices,
                IMapper mapper,
                ILogger<ProductsController> logger
            )
        {
            _storeServices = storeServices;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> Get(Guid productId)
        {
            _logger.LogInformation($"[GET] /api/v1/products/{productId.ToString()}");
            var response = await _storeServices.GetProduct(productId);
            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]ProductListInput request)
        {
            _logger.LogInformation($"[GET] /api/v1/products => {JsonConvert.SerializeObject(request)}");
            var response = await _storeServices.GetProducts(request);
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromForm]CreateNewProductRequest request)
        {
            _logger.LogInformation($"[POST] /api/v1/products => {JsonConvert.SerializeObject(request)}");
            
            var dto = _mapper.Map<ProductDto>(request);
            var stream = request.Image.OpenReadStream();

            dto.UploadedImage = stream;
            dto.UploadedImageExtension = Path.GetExtension(request.Image.FileName);
            await _storeServices.InsertProduct(dto);
            return Ok();
        }


        [HttpPut]
        public async Task<IActionResult> Put([FromForm] UpdateProductRequest request)
        {
            _logger.LogInformation($"[PUT] /api/v1/products => {JsonConvert.SerializeObject(request)}");
            var dto = _mapper.Map<ProductDto>(request);

            if(request.Image != null)
            {
                var stream = request.Image.OpenReadStream();
                dto.UploadedImage = stream;
                dto.UploadedImageExtension = Path.GetExtension(request.Image.FileName);

            }
            await _storeServices.UpdateProduct(dto);
            return Ok();
        }


        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(Guid productId)
        {
            _logger.LogInformation($"[DELETE] /api/v1/products/{productId.ToString()}");
            await _storeServices.DeleteProduct(productId);
            return Ok();
        }
    }
}
