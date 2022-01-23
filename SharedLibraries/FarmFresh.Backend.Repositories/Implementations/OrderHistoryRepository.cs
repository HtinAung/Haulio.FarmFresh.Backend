using FarmFresh.Backend.Entities;
using FarmFresh.Backend.Repositories.Interfaces;
using FarmFresh.Backend.Shared;
using FarmFresh.Backend.Storages.SQLServer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<BaseListOutput<AppOrderHistory>> GetAllByUser(Guid userId, BaseListInput input)
        {
            if(! await _context.Users.AnyAsync(c => c.Id == userId && c.IsActive))
            {
                throw new Exception($"User with id {userId} couldn't be found / been de-activated");
            }

            int totalRows = 0;
            List<AppOrderHistory> result = new List<AppOrderHistory>();

            input.Query = input.Query.Trim();

            if (string.IsNullOrEmpty(input.Query))
            {
                totalRows = await _context
                                  .OrderHistories
                                  .CountAsync(c => c.UserId == userId && c.IsActive);

                result = await _context
                    .OrderHistories
                    .Where(c => c.UserId == userId && c.IsActive)
                    .Skip(input.SkipCount)
                    .Take(input.FetchSize)
                    .Include(c => c.User)
                    .Include(c => c.Store)
                    .AsNoTracking()
                    .ToListAsync();
            }
            else
            {
                totalRows = await _context
                                 .OrderHistories
                                 .CountAsync(c => c.UserId == userId && c.IsActive && c.Item.Contains(input.Query));

                result = await _context
                    .OrderHistories
                    .Where(c => c.UserId == userId && c.IsActive && c.Item.Contains(input.Query))
                    .Skip(input.SkipCount)
                    .Take(input.FetchSize)
                    .Include(c => c.User)
                    .Include(c => c.Store)
                    .AsNoTracking()
                    .ToListAsync();
            }
          

            return new BaseListOutput<AppOrderHistory>
            {
                TotalRows = totalRows,
                SkipCount = input.SkipCount,
                FetchSize = input.FetchSize,
                Query = input.Query,
                Rows = result
            };
        }

        public async Task<BaseListOutput<AppOrderHistory>> GetAllByStore(Guid storeId, BaseListInput input)
        {
            if (!await _context.Stores.AnyAsync(c => c.Id == storeId && c.IsActive))
            {
                throw new Exception($"Store with id {storeId} couldn't be found / been de-activated");
            }
            int totalRows = 0;
            List<AppOrderHistory> result = new List<AppOrderHistory>();

            input.Query = input.Query.Trim();

            if (string.IsNullOrEmpty(input.Query))
            {
                totalRows = await _context
                                  .OrderHistories
                                  .CountAsync(c => c.StoreId == storeId && c.IsActive);

                result = await _context
                    .OrderHistories
                    .Where(c => c.StoreId == storeId && c.IsActive)
                    .Skip(input.SkipCount)
                    .Take(input.FetchSize)
                    .Include(c => c.User)
                    .Include(c => c.Store)
                    .AsNoTracking()
                    .ToListAsync();
            }
            else
            {
                totalRows = await _context
                                  .OrderHistories
                                  .CountAsync(c => c.StoreId == storeId && c.IsActive && c.Item.Contains(input.Query));

                result = await _context
                    .OrderHistories
                    .Where(c => c.StoreId == storeId && c.IsActive && c.Item.Contains(input.Query))
                    .Skip(input.SkipCount)
                    .Take(input.FetchSize)
                    .Include(c => c.User)
                    .Include(c => c.Store)
                    .AsNoTracking()
                    .ToListAsync();
            }
          

            return new BaseListOutput<AppOrderHistory>
            {
                TotalRows = totalRows,
                SkipCount = input.SkipCount,
                FetchSize = input.FetchSize,
                Query = input.Query,
                Rows = result
            };
        }

        public async Task Insert(AppOrderHistory entity)
        {
            _context.OrderHistories.Add(entity);
            await _context.SaveChangesAsync();
        }
    }
}
