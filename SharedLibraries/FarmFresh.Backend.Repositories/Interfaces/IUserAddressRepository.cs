using FarmFresh.Backend.Entities;
using System;
using System.Threading.Tasks;

namespace FarmFresh.Backend.Repositories.Interfaces
{
    public interface IUserAddressRepository
    {
        Task<AppUserAddress> GetByUserId(Guid userId);
        Task Update(AppUserAddress entity);
    }
}
