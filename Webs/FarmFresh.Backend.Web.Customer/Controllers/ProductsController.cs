using FarmFresh.Backend.Shared;
using FarmFresh.Backend.Web.Customer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FarmFresh.Backend.Web.Customer.Controllers
{
    [Authorize(Roles = GlobalConstants.CustomerUserRoleName)]
    public class ProductsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public ProductsController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("Api");
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetProducts(ProductListInput request)
        {
            string path = $"{_configuration["ResourceEndpoints:Path:AllProducts"]}?category={request.Category}&query={request.Query}&skipCount={request.SkipCount}&fetchSize={request.FetchSize}";
            var response = await _httpClient.GetAsync(path);
            BaseListOutput<ProductViewModel> result = new BaseListOutput<ProductViewModel>();
            if (response.IsSuccessStatusCode)
            {
                result = JsonConvert.DeserializeObject<BaseListOutput<ProductViewModel>>(
                        await response.Content.ReadAsStringAsync()
                    );
            }
            return Ok(result);
        }
        public async Task<IActionResult> GetProductCategories()
        {
            string path = _configuration["ResourceEndpoints:Path:AllCategories"];
            var response = await _httpClient.GetAsync(path);
            IEnumerable<string> result = new List<string>();
            if (response.IsSuccessStatusCode)
            {
                result = JsonConvert.DeserializeObject<IEnumerable<string>>(
                        await response.Content.ReadAsStringAsync()
                    );
            }
            return Ok(result);
        }

        public async Task<IActionResult> Detail(string id)
        {
            ViewBag.Message = string.Empty;
            string path = $"{_configuration["ResourceEndpoints:Path:ProductById"]}{id}/item";
            var response = await _httpClient.GetAsync(path);
            ProductViewModel model = new ProductViewModel();
            if (response.IsSuccessStatusCode)
            {
                model = JsonConvert.DeserializeObject<ProductViewModel>(
                        await response.Content.ReadAsStringAsync()
                    );
            }
            else
            {
                ViewBag.Message = ((int)response.StatusCode).ToString();
                try
                {
                    string errorMessageJson = await response.Content.ReadAsStringAsync();

                    string errorMessage = JsonConvert.DeserializeObject<ApiErrorModel>(errorMessageJson)?.ErrorMessage;
                    ViewBag.Message += !string.IsNullOrEmpty(errorMessage) ? $" - {errorMessage}" : string.Empty;
                }
                catch { }
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Order(CreateOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                string path = _configuration["ResourceEndpoints:Path:MakeAnOrder"];
                var response = await _httpClient.PostAsJsonAsync(path, model);
                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { isSuccess = true, message = string.Empty });
                }
                return Ok(new { isSuccess = false, message = "Failed to create an order. Please try again later" });

            }
            return Ok(new { isSuccess = false, message = "Invalid payload" });

        }

    }
}
