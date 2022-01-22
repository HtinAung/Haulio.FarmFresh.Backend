using FarmFresh.Backend.Entities;
using System;
using System.Threading.Tasks;

namespace FarmFresh.Backend.Repositories.Interfaces
{
    public interface IStoreRepository :IBaseRepository<AppStore>
    {
        Task<AppStore> GetById(Guid id);
        Task<Guid> Insert(AppStore entity);
        Task ChangeName(Guid id, string newName);
        Task TieAdminUserWithStore(Guid storeId, Guid userId);
    }
}
