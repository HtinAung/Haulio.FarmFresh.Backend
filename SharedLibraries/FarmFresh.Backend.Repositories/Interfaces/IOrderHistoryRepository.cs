using FarmFresh.Backend.Entities;
using FarmFresh.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Backend.Repositories.Interfaces
{
    public interface IOrderHistoryRepository
    {
        Task<BaseListOutput<AppOrderHistory>> GetAllByUser(Guid userId, BaseListInput input);
        Task<BaseListOutput<AppOrderHistory>> GetAllByStore(Guid storeId, BaseListInput input);
        Task Insert(AppOrderHistory entity);
    }
}
