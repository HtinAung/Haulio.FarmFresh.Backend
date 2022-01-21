using FarmFresh.Backend.Entities;
using FarmFresh.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Backend.Repositories.Interfaces
{
    public interface IOrderHistoryRepository:IBaseRepository<AppOrderHistory>
    {
        Task<BaseResponse<AppOrderHistory>> GetAllByUser(Guid userId, BaseRequest request);
        Task<BaseResponse<AppOrderHistory>> GetAllByStore(Guid storeId, BaseRequest request);
        Task Insert(AppOrderHistory entity);
    }
}
