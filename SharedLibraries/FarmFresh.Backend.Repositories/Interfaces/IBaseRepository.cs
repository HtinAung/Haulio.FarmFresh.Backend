using FarmFresh.Backend.Shared;
using System;
using System.Threading.Tasks;

namespace FarmFresh.Backend.Repositories.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task SetInactive(Guid id);
        Task SetActive(Guid id);
    }
}
