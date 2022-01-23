using FarmFresh.Backend.DataTransferObjects;
using FarmFresh.Backend.Shared;
using System;
using System.Threading.Tasks;

namespace FarmFresh.Backend.Services.Interfaces.Stores
{
    public interface IStoreServices
    {
        Task<BaseListOutput<ProductCategoryDto>> GetProductCategories(BaseListInput input);
        Task<BaseListOutput<ProductDto>> GetProducts(ProductListInput input);
        Task InsertProduct(ProductDto dto);
        Task UpdateProduct(ProductDto dto);
        Task UpdateProductAvailableAmount(Guid storeId, Guid productId, int currentAmount);
        Task DeleteProduct(Guid productId);
    }
}
