using FarmFresh.Backend.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FarmFresh.Backend.Repositories.Interfaces
{
    public interface IProductCategoryRepository:IBaseRepository<AppProductCategory>
    {
        Task BulkInsert(IEnumerable<AppProductCategory> entities);
        Task Update(AppProductCategory entity);
    }
}
