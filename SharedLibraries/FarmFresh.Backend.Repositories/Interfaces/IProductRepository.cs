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
        Task<BaseResponse<AppProduct>> GetAll(Guid storeId, BaseRequest request);
        Task<AppProduct> GetById(Guid storeId, Guid productId);
        Task Update(AppProduct entity);
        Task Insert(AppProduct entity);
    }
}
