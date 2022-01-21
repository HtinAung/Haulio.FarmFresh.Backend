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
    public class ProductRepository:IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse<AppProduct>> GetAll(Guid storeId, BaseRequest request)
        {
            if(! await StoreExists(storeId))
            {
                throw new Exception($"Store not found / been de-activated");
            }

            int totalRows = await _context.Products.CountAsync(c => c.StoreId == storeId && c.IsActive);

            var result = await _context.Products.Where(c => c.StoreId == storeId && c.IsActive)
                .Skip(request.SkipCount)
                .Take(request.FetchSize)
                .AsNoTracking()
                .ToListAsync();

            return new BaseResponse<AppProduct>
            {
                TotalRows = totalRows,
                SkipCount = request.SkipCount,
                FetchSize = request.FetchSize,
                Rows = result
            };
        }

        private async Task<bool> StoreExists(Guid storeId)
        {
            return await _context.Stores.AnyAsync(c => c.Id == storeId && c.IsActive);
        }

        public async Task<AppProduct> GetById(Guid storeId, Guid productId)
        {
            if (!await StoreExists(storeId))
            {
                throw new Exception($"Store not found / been de-activated");
            }
            var result = await _context
                .Products
                .Include(c => c.Store)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.StoreId == storeId && c.Id == productId && c.IsActive);
            if(result == null)
            {
                throw new Exception($"Product not found / been de-activated");
            }
            return result;
        }

        public async Task Update(AppProduct entity)
        {
            if (! await _context.Products.AnyAsync(c => c.Id == entity.Id && c.IsActive))
            {
                throw new Exception($"Product with id: {entity.Id} is not found");
            }
            if (!_context.Products.Local.Any(c => c.Id == entity.Id))
            {
                _context.Products.Attach(entity);
            }
            _context.Entry(entity).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task Insert(AppProduct entity)
        {
            var store = await _context
                .Stores
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == entity.StoreId && c.IsActive);

            if (store == null)
            {
                throw new Exception($"Store not found / been de-activated");
            }

            store.Products.Add(entity);
            _context.Entry(store).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task SetInactive(Guid id)
        {
            var entity = await _context.Products.FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
            if (entity == null)
            {
                throw new Exception($"Product with id: {id} is not found");
            }
            if (!_context.Products.Local.Any(c => c.Id == entity.Id))
            {
                _context.Products.Attach(entity);
            }
            entity.IsActive = false;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }
        public async Task SetActive(Guid id)
        {
            var entity = await _context.Products.FirstOrDefaultAsync(c => c.Id == id && !c.IsActive);
            if (entity == null)
            {
                throw new Exception($"Product with id: {id} is not found");
            }
            if (!_context.Products.Local.Any(c => c.Id == entity.Id))
            {
                _context.Products.Attach(entity);
            }
            entity.IsActive = true;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }
    }
}
