using FarmFresh.Backend.Entities;
using FarmFresh.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FarmFresh.Backend.Repositories.Interfaces
{
    public interface IProductCategoryRepository:IBaseRepository<AppProductCategory>
    {
        Task BulkInsert(IEnumerable<AppProductCategory> entities);
        Task Update(AppProductCategory entity);
        Task<BaseListOutput<AppProductCategory>> GetAll(BaseListInput input);
        Task<AppProductCategory> GetById(Guid id);
    }
}
