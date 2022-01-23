using AutoMapper;
using FarmFresh.Backend.DataTransferObjects;
using FarmFresh.Backend.Entities;
using FarmFresh.Backend.Repositories.Interfaces;
using FarmFresh.Backend.Services.Interfaces;
using FarmFresh.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FarmFresh.Backend.Services.Implementations.Customers
{
    public class CustomerServices: ICustomerServices
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IOrderHistoryRepository _orderHistoryRepository;
        private readonly IMapper _mapper;

        public CustomerServices(
                IProductRepository productRepository,
                IProductCategoryRepository productCategoryRepository,
                IOrderHistoryRepository orderHistoryRepository,
                IMapper mapper
            )
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _orderHistoryRepository = orderHistoryRepository;
            _mapper = mapper;
        }


        public async Task<BaseListOutput<ProductDto>> GetProducts(
                ProductListInput input
            )
        {
            BaseListOutput<AppProduct> rawResult = await _productRepository.GetAll(input);
            BaseListOutput<ProductDto> dtoResult = new BaseListOutput<ProductDto>
            {
                FetchSize = rawResult.FetchSize,
                Query = rawResult.Query,
                SkipCount = rawResult.SkipCount,
                TotalRows = rawResult.TotalRows
            };
            dtoResult.Rows = _mapper.Map<IEnumerable<AppProduct>, IEnumerable<ProductDto>>(rawResult.Rows);
            return dtoResult;
        }

        public async Task<IEnumerable<string>> GetProductCategories(
                BaseListInput input
            )
        {
            BaseListOutput<AppProductCategory> rawResult = await _productCategoryRepository.GetAll(input);
            var appendedResult = new List<string>()
            {
                GlobalConstants.CategoryOnSalesKeyword,
                GlobalConstants.CategoryNewKeyword,
                GlobalConstants.CategoryStoreKeyword
            };
            appendedResult.AddRange(rawResult.Rows.Select(c => c.Name));

            return appendedResult;
        }

        public async Task<BaseListOutput<OrderHistoryDto>> GetOrderHistories(Guid userId, BaseListInput input)
        {
            BaseListOutput<AppOrderHistory> rawResult = await _orderHistoryRepository.GetAllByUser(userId, input);
            BaseListOutput<OrderHistoryDto> dtoResult = new BaseListOutput<OrderHistoryDto>
            {
                FetchSize = rawResult.FetchSize,
                Query = rawResult.Query,
                SkipCount = rawResult.SkipCount,
                TotalRows = rawResult.TotalRows
            };
            dtoResult.Rows = _mapper.Map<IEnumerable<AppOrderHistory>, IEnumerable<OrderHistoryDto>>(rawResult.Rows);
            return dtoResult;
        }

        public async Task MakeAnOrder(OrderHistoryDto dto)
        {
            int currentAmount = await _productRepository.GetProductAvailableAmount(dto.StoreId, dto.ProductId.Value);
            int newAmount = currentAmount - dto.Total;
            var entity = _mapper.Map<OrderHistoryDto, AppOrderHistory>(dto);
            await _orderHistoryRepository.Insert(entity);
            await _productRepository.UpdateProductAvailableAmount(dto.StoreId, dto.ProductId.Value, newAmount);
        }
    }
}
