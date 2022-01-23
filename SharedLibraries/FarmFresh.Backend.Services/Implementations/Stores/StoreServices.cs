using AutoMapper;
using FarmFresh.Backend.DataTransferObjects;
using FarmFresh.Backend.Entities;
using FarmFresh.Backend.Repositories.Interfaces;
using FarmFresh.Backend.Services.Interfaces.Stores;
using FarmFresh.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Backend.Services.Implementations.Stores
{
    public class StoreServices:IStoreServices
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IOrderHistoryRepository _orderHistoryRepository;
        private readonly BlobStorageHelper _blobStorageHelper;
        private readonly IMapper _mapper;
        public StoreServices(
                IProductRepository productRepository,
                IProductCategoryRepository productCategoryRepository,
                IOrderHistoryRepository orderHistoryRepository,
                BlobStorageHelper blobStorageHelper,
                IMapper mapper
            )
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _orderHistoryRepository = orderHistoryRepository;
            _blobStorageHelper = blobStorageHelper;
            _mapper = mapper;
        }

        
        public async Task<BaseListOutput<ProductCategoryDto>> GetProductCategories(
                BaseListInput input
            )
        {
            BaseListOutput<AppProductCategory> rawResult = await _productCategoryRepository.GetAll(input);
            BaseListOutput<ProductCategoryDto> dtoResult = new BaseListOutput<ProductCategoryDto>
            {
                FetchSize = rawResult.FetchSize,
                Query = rawResult.Query,
                SkipCount = rawResult.SkipCount,
                TotalRows = rawResult.TotalRows
            };
            dtoResult.Rows = _mapper.Map<IEnumerable<AppProductCategory>, IEnumerable<ProductCategoryDto>>(rawResult.Rows);
            return dtoResult;
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

        public async Task<BaseListOutput<OrderHistoryDto>> GetOrderHistories(Guid storeId, BaseListInput input)
        {
            BaseListOutput<AppOrderHistory> rawResult = await _orderHistoryRepository.GetAllByStore(storeId, input);
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

        public async Task InsertProduct(ProductDto dto)
        {
            dto.ImageUrl = await _blobStorageHelper.UploadFile(dto.UploadedImage, $"{Guid.NewGuid()}{dto.UploadedImageExtension}");
            AppProduct entity = _mapper.Map<ProductDto, AppProduct>(dto);
            await _productRepository.Insert(entity);
        }

        public async Task UpdateProduct(ProductDto dto)
        {
            if (dto.UploadedImage != null)
            {
                dto.ImageUrl = await _blobStorageHelper.UploadFile(dto.UploadedImage, $"{Guid.NewGuid()}{dto.UploadedImageExtension}");
            }
            AppProduct entity = _mapper.Map<ProductDto, AppProduct>(dto);
            await _productRepository.Update(entity);
        }

        public async Task UpdateProductAvailableAmount(Guid storeId, Guid productId, int currentAmount)
        {
            await _productRepository.UpdateProductAvailableAmount(storeId, productId, currentAmount);
        }

        public async Task DeleteProduct(Guid productId)
        {
            await _productRepository.SetInactive(productId);
        }

    }
}
