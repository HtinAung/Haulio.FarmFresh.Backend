using FarmFresh.Backend.DataTransferObjects;
using FarmFresh.Backend.Shared;
using System;
using System.Threading.Tasks;

namespace FarmFresh.Backend.Services.Interfaces
{
    public interface IStoreServices
    {
        Task<BaseListOutput<ProductCategoryDto>> GetProductCategories(BaseListInput input);
        Task<BaseListOutput<ProductDto>> GetProducts(ProductListInput input);
        Task<ProductDto> GetProduct(Guid productId);
        Task<BaseListOutput<OrderHistoryDto>> GetOrderHistories(Guid storeId, BaseListInput input);
        Task<UserDto> GetUserById(Guid id);
        Task CreateStore(StoreDto dto);
        Task InsertProduct(ProductDto dto);
        Task UpdateProduct(ProductDto dto);
        Task UpdateProductAvailableAmount(Guid storeId, Guid productId, int currentAmount);
        Task DeleteProduct(Guid productId);
    }
}
