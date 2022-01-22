using FarmFresh.Backend.Entities;
using FarmFresh.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Backend.Repositories.Interfaces
{
    public interface IProductRepository: IBaseRepository<AppProduct>
    {
        Task<BaseListOutput<AppProduct>> GetAll(Guid storeId, ProductListInput input);
        Task<BaseListOutput<AppProduct>> GetAll(ProductListInput input);

        Task<AppProduct> GetById(Guid storeId, Guid productId, bool includeRelatonships = true);
        Task<AppProduct> GetById(Guid productId, bool includeRelatonships = true);
        Task<int> GetProductAvailableAmount(Guid storeId, Guid productId);
        Task UpdateProductAvailableAmount(Guid storeId, Guid productId, int currentAmount);
        Task Update(AppProduct entity);
        Task Insert(AppProduct entity);
        Task<int> BulkInsert(Guid storeId, IEnumerable<AppProduct> entities);
    }
}
