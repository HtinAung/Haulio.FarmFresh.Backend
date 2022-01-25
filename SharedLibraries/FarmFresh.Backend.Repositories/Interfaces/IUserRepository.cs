using FarmFresh.Backend.Entities;
using System;
using System.Threading.Tasks;

namespace FarmFresh.Backend.Repositories.Interfaces
{
    public interface IUserRepository: IBaseRepository<AppUser>
    {
        Task ChangeName(Guid id, string newName);
        Task<AppUser> GetUserById(Guid id);
    }
}
