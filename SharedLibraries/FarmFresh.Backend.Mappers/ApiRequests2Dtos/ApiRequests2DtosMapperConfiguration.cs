using AutoMapper;
using FarmFresh.Backend.Api.Customers.Models;
using FarmFresh.Backend.Api.Stores.Models;
using FarmFresh.Backend.DataTransferObjects;

namespace FarmFresh.Backend.Mappers.ApiRequests2Dtos
{
    public class ApiRequests2DtosMapperConfiguration: Profile
    {
        public ApiRequests2DtosMapperConfiguration()
        {
            Init();
        }

        public void Init()
        {
            CreateMap<CreateNewProductRequest, ProductDto>();
            CreateMap<UpdateProductRequest, ProductDto>();

            CreateMap<CreateOrderRequest, OrderHistoryDto>();
        }
    }
}
