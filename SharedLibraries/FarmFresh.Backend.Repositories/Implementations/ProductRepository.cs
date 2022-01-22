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

        public async Task<BaseListOutput<AppProduct>> GetAll(Guid storeId, ProductListInput input)
        {
            if(! await StoreExists(storeId))
            {
                throw new Exception($"Store not found / been de-activated");
            }

            List<AppProduct> result = new List<AppProduct>();
            int totalRows = 0;
            
            input.Query = input.Query.Trim();
            input.Category = input.Category.Trim();

            switch (input.Category)
            {
                case GlobalConstants.CategoryNewKeyword:
                    {
                        (totalRows, result) = await QueryProductsByNewCategory(storeId, input);
                        break;
                    }
                case GlobalConstants.CategoryOnSalesKeyword:
                    {
                        (totalRows, result) = await QueryProductsByOnSalesCategory(storeId, input);
                        break;
                    }
                case GlobalConstants.CategoryStoreKeyword:
                    {
                        (totalRows, result) = await QueryProductsByStoreCategory(storeId, input);
                        break;
                    }
                default:
                    {
                        (totalRows, result) = await QueryProductsByDatabaseDataCategory(storeId, input);

                        break;
                    }
            }

            return new BaseListOutput<AppProduct>
            {
                TotalRows = totalRows,
                SkipCount = input.SkipCount,
                FetchSize = input.FetchSize,
                Rows = result
            };
        }


        public async Task<AppProduct> GetById(Guid storeId, Guid productId, bool includeRelatonships=true)
        {
            if (!await StoreExists(storeId))
            {
                throw new Exception($"Store not found / been de-activated");
            }
            AppProduct result = null;

            if (includeRelatonships)
            {
                await _context
                .Products
                .Include(c => c.Store)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.StoreId == storeId && c.Id == productId && c.IsActive);
            }
            else
            {
                await _context
                .Products
                .FirstOrDefaultAsync(c => c.StoreId == storeId && c.Id == productId && c.IsActive);
            }

            if(result == null)
            {
                throw new Exception($"Product not found");
            }
            return result;
        }

        public async Task Update(AppProduct entity)
        {
            if (!await StoreExists(entity.StoreId))
            {
                throw new Exception($"Store not found / been de-activated");
            }
            if (! await _context.Products.AnyAsync(c => c.Id == entity.Id && c.IsActive))
            {
                throw new Exception($"Product with id: {entity.Id} is not found");
            }
            if (entity.AvailableAmount < 0)
            {
                throw new Exception($"Product with id: {entity.Id} has 0 available amount");

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
            if (entity.AvailableAmount < 0)
            {
                throw new Exception($"Product with id: {entity.Id} cannot have < 0 available amount");

            }
            store.Products.Add(entity);
            _context.Entry(store).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task<int> GetProductAvailableAmount(Guid storeId, Guid productId)
        {
            if (!await StoreExists(storeId))
            {
                throw new Exception($"Store not found / been de-activated");
            }
            var entity = await _context.Products.FirstOrDefaultAsync(c => c.StoreId == storeId && c.Id == productId && c.IsActive);
            if (entity == null)
            {
                throw new Exception($"Product not found");
            }
            return entity.AvailableAmount;
        }

        public async Task UpdateProductAvailableAmount(Guid storeId, Guid productId, int currentAmount)
        {
            var entity = await GetById(storeId, productId, false);
            entity.AvailableAmount = currentAmount;
            await Update(entity);
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


        private async Task<bool> StoreExists(Guid storeId)
        {
            return await _context.Stores.AnyAsync(c => c.Id == storeId && c.IsActive);
        }


        private async Task<(int Total, List<AppProduct>)> QueryProductsByNewCategory(Guid storeId, ProductListInput input)
        {
            List<AppProduct> result = new List<AppProduct>();
            int totalRows = 0;
            if (string.IsNullOrEmpty(input.Query))
            {

                totalRows = await _context.Products.CountAsync(c => c.StoreId == storeId && c.AvailableAmount > 0 && c.IsActive);

                result = await _context
                    .Products
                    .Include(c => c.Category)
                    .Include(c => c.Store)
                    .Where(c => c.StoreId == storeId && c.AvailableAmount > 0 && c.IsActive)
                    .OrderByDescending(c => c.ModifiedDate)
                    .Skip(input.SkipCount)
                    .Take(input.FetchSize)
                    .AsNoTracking()
                    .ToListAsync();
            }
            else
            {

                totalRows = await _context
                    .Products
                    .CountAsync(c => c.StoreId == storeId && c.AvailableAmount > 0 && c.IsActive && c.Name.StartsWith(input.Query, StringComparison.InvariantCultureIgnoreCase));

                result = await _context
                    .Products
                    .Include(c => c.Category)
                    .Include(c => c.Store)
                    .Where(c => c.StoreId == storeId && c.AvailableAmount > 0 && c.IsActive && c.Name.StartsWith(input.Query, StringComparison.InvariantCultureIgnoreCase))
                    .OrderByDescending(c => c.ModifiedDate)
                    .Skip(input.SkipCount)
                    .Take(input.FetchSize)
                    .AsNoTracking()
                    .ToListAsync();
            }
            return (totalRows, result);
        }

        private async Task<(int Total, List<AppProduct>)> QueryProductsByOnSalesCategory(Guid storeId, ProductListInput input)
        {
            List<AppProduct> result = new List<AppProduct>();
            int totalRows = 0;
            if (string.IsNullOrEmpty(input.Query))
            {

                totalRows = await _context
                    .Products
                    .CountAsync(c => c.StoreId == storeId && c.AvailableAmount > 0 && c.IsActive);

                result = await _context
                    .Products
                    .Include(c => c.Category)
                    .Include(c => c.Store)
                    .Where(c => c.StoreId == storeId && c.AvailableAmount > 0 && c.IsActive)
                    .Skip(input.SkipCount)
                    .Take(input.FetchSize)
                    .AsNoTracking()
                    .ToListAsync();
            }
            else
            {

                totalRows = await _context
                    .Products
                    .CountAsync(c => c.StoreId == storeId && c.AvailableAmount > 0 && c.IsActive && c.Name.StartsWith(input.Query, StringComparison.InvariantCultureIgnoreCase));

                result = await _context
                    .Products
                    .Include(c => c.Category)
                    .Include(c => c.Store)
                    .Where(c => c.StoreId == storeId && c.AvailableAmount > 0 && c.IsActive && c.Name.StartsWith(input.Query, StringComparison.InvariantCultureIgnoreCase))
                    .Skip(input.SkipCount)
                    .Take(input.FetchSize)
                    .AsNoTracking()
                    .ToListAsync();
            }
            return (totalRows, result);
        }

        private async Task<(int Total, List<AppProduct>)> QueryProductsByStoreCategory(Guid storeId, ProductListInput input)
        {
            List<AppProduct> result = new List<AppProduct>();
            int totalRows = 0;
            if (string.IsNullOrEmpty(input.Query))
            {

                totalRows = await _context.Products.CountAsync(c => c.StoreId == storeId && c.AvailableAmount > 0 && c.IsActive);

                result = await _context
                    .Products
                    .Include(c => c.Category)
                    .Include(c => c.Store)
                    .Where(c => c.StoreId == storeId && c.AvailableAmount > 0 && c.IsActive)
                    .OrderBy(c => c.Store.Name)
                    .Skip(input.SkipCount)
                    .Take(input.FetchSize)
                    .AsNoTracking()
                    .ToListAsync();
            }
            else
            {

                totalRows = await _context
                    .Products
                    .CountAsync(c => c.StoreId == storeId && c.AvailableAmount > 0 && c.IsActive && c.Name.StartsWith(input.Query, StringComparison.InvariantCultureIgnoreCase));

                result = await _context
                    .Products
                    .Include(c => c.Category)
                    .Include(c => c.Store)
                    .Where(c => c.StoreId == storeId && c.AvailableAmount > 0 && c.IsActive && c.Name.StartsWith(input.Query, StringComparison.InvariantCultureIgnoreCase))
                    .OrderBy(c => c.Store.Name)
                    .Skip(input.SkipCount)
                    .Take(input.FetchSize)
                    .AsNoTracking()
                    .ToListAsync();
            }
            return (totalRows, result);
        }

        private async Task<(int Total, List<AppProduct>)> QueryProductsByDatabaseDataCategory(Guid storeId, ProductListInput input)
        {
            List<AppProduct> result = new List<AppProduct>();
            int totalRows = 0;
            if (string.IsNullOrEmpty(input.Query))
            {

                totalRows = await _context
                    .Products
                    .Include(c => c.Category)
                    .CountAsync(c => c.StoreId == storeId && c.AvailableAmount > 0 && c.Category.Name.StartsWith(input.Category, StringComparison.InvariantCultureIgnoreCase) && c.IsActive);

                result = await _context
                    .Products
                    .Include(c => c.Category)
                    .Include(c => c.Store)
                    .Where(c => c.StoreId == storeId && c.AvailableAmount > 0 && c.Category.Name.StartsWith(input.Category, StringComparison.InvariantCultureIgnoreCase) && c.IsActive)
                    .OrderByDescending(c => c.ModifiedDate)
                    .Skip(input.SkipCount)
                    .Take(input.FetchSize)
                    .AsNoTracking()
                    .ToListAsync();
            }
            else
            {

                totalRows = await _context
                    .Products
                    .Include(c => c.Category)
                    .CountAsync(c => c.StoreId == storeId && c.AvailableAmount > 0 && c.Category.Name.StartsWith(input.Category, StringComparison.InvariantCultureIgnoreCase) && c.Name.StartsWith(input.Query, StringComparison.InvariantCultureIgnoreCase) && c.IsActive);

                result = await _context
                    .Products
                    .Include(c => c.Category)
                    .Include(c => c.Store)
                    .Where(c => c.StoreId == storeId && c.AvailableAmount > 0 && c.Category.Name.StartsWith(input.Category, StringComparison.InvariantCultureIgnoreCase) && c.Name.StartsWith(input.Query, StringComparison.InvariantCultureIgnoreCase) && c.IsActive)
                    .OrderByDescending(c => c.ModifiedDate)
                    .Skip(input.SkipCount)
                    .Take(input.FetchSize)
                    .AsNoTracking()
                    .ToListAsync();
            }
            return (totalRows, result);
        }
    }
}
