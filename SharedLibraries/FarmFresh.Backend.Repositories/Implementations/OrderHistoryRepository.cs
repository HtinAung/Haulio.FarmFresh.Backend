using FarmFresh.Backend.Entities;
using FarmFresh.Backend.Repositories.Interfaces;
using FarmFresh.Backend.Shared;
using FarmFresh.Backend.Storages.SQLServer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace FarmFresh.Backend.Repositories.Implementations
{
    public class OrderHistoryRepository : IOrderHistoryRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse<AppOrderHistory>> GetAllByUser(Guid userId, BaseRequest request)
        {
            if(! await _context.Users.AnyAsync(c => c.Id == userId && c.IsActive))
            {
                throw new Exception($"User with id {userId} couldn't be found / been de-activated");
            }
            int totalRows = await _context
                .OrderHistories
                .Include(c => c.User)
                .CountAsync(c => c.UserId == userId && c.IsActive);

            var result = await _context
                .OrderHistories
                .Where(c => c.UserId == userId && c.IsActive)
                .Skip(request.SkipCount)
                .Take(request.FetchSize)
                .Include(c => c.User)
                .AsNoTracking()
                .ToListAsync();

            return new BaseResponse<AppOrderHistory>
            {
                TotalRows = totalRows,
                SkipCount = request.SkipCount,
                FetchSize = request.FetchSize,
                Rows = result
            };
        }

        public async Task<BaseResponse<AppOrderHistory>> GetAllByStore(Guid storeId, BaseRequest request)
        {
            if (!await _context.Stores.AnyAsync(c => c.Id == storeId && c.IsActive))
            {
                throw new Exception($"Store with id {storeId} couldn't be found / been de-activated");
            }

            int totalRows = await _context
                .OrderHistories
                .Include(c => c.User)
                .CountAsync(c => c.StoreId == storeId && c.IsActive);

            var result = await _context
                .OrderHistories
                .Where(c => c.StoreId == storeId && c.IsActive)
                .Skip(request.SkipCount)
                .Take(request.FetchSize)
                .Include(c => c.Store)
                .AsNoTracking()
                .ToListAsync();

            return new BaseResponse<AppOrderHistory>
            {
                TotalRows = totalRows,
                SkipCount = request.SkipCount,
                FetchSize = request.FetchSize,
                Rows = result
            };
        }

        public async Task Insert(AppOrderHistory entity)
        {
            _context.OrderHistories.Add(entity);
            await _context.SaveChangesAsync();
        }


        #region Not Implemented
        public Task SetActive(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task SetInactive(Guid id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
