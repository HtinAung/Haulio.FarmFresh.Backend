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
    public class OrderHistoriesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public OrderHistoriesController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("Api");
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetOrderHistories(BaseListInput request)
        {
            string path = $"{_configuration["ResourceEndpoints:Path:AllOrderHistories"]}?skipCount={request.SkipCount}&fetchSize={request.FetchSize}";
            var response = await _httpClient.GetAsync(path);
            BaseListOutput<OrderHistoryViewModel> result = new BaseListOutput<OrderHistoryViewModel>();
            if (response.IsSuccessStatusCode)
            {
                result = JsonConvert.DeserializeObject<BaseListOutput<OrderHistoryViewModel>>(
                        await response.Content.ReadAsStringAsync()
                    );
            }
            return Ok(result);
        }
    }
}
