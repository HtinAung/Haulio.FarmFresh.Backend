﻿using FarmFresh.Backend.Entities;
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
    public class ProductCategoryRepository: IProductCategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AppProductCategory> GetById(Guid id)
        {
            var entity = await _context.ProductCategories.FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
            if(entity == null)
            {
                throw new Exception($"Product Category with id: {id} is not found");
            }
            return entity;
        }

        public async Task<BaseListOutput<AppProductCategory>> GetAll(BaseListInput input)
        {
            int totalRows = 0;
            List<AppProductCategory> result = new List<AppProductCategory>();

            input.Query = input.Query.Trim();

            if (string.IsNullOrEmpty(input.Query))
            {

                totalRows = await _context.ProductCategories.CountAsync(c => c.IsActive);
                result = await _context.ProductCategories.Where(c => c.IsActive)
                    .Skip(input.SkipCount)
                    .Take(input.FetchSize)
                    .AsNoTracking()
                    .ToListAsync();
            }
            else
            {

                totalRows = await _context.ProductCategories.CountAsync(c => c.IsActive && c.Name.Contains(input.Query));
                result = await _context.ProductCategories.Where(c => c.IsActive && c.Name.Contains(input.Query))
                    .Skip(input.SkipCount)
                    .Take(input.FetchSize)
                    .AsNoTracking()
                    .ToListAsync();
            }

            return new BaseListOutput<AppProductCategory>
            {
                TotalRows = totalRows,
                Query = input.Query,
                SkipCount = input.SkipCount,
                FetchSize = input.FetchSize,
                Rows = result
            };
        }

        public async Task BulkInsert(IEnumerable<AppProductCategory> entities)
        {
            await _context.ProductCategories.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task Update(AppProductCategory entity)
        {
            if(!_context.ProductCategories.Any(c => c.Id == entity.Id && c.IsActive))
            {
                throw new Exception($"Product Category with id: {entity.Id} is not found");
            }
            if(!_context.ProductCategories.Local.Any(c => c.Id == entity.Id))
            {
                _context.ProductCategories.Attach(entity);
            }
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }


        public async Task SetInactive(Guid id)
        {
            var entity = await _context.ProductCategories.FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
            if (entity == null)
            {
                throw new Exception($"Product Category with id: {id} is not found");
            }
            if (!_context.ProductCategories.Local.Any(c => c.Id == entity.Id))
            {
                _context.ProductCategories.Attach(entity);
            }
            entity.IsActive = false;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }
        public async Task SetActive(Guid id)
        {
            var entity = await _context.ProductCategories.FirstOrDefaultAsync(c => c.Id == id && !c.IsActive);
            if (entity == null)
            {
                throw new Exception($"Product Category with id: {id} is not found");
            }
            if (!_context.ProductCategories.Local.Any(c => c.Id == entity.Id))
            {
                _context.ProductCategories.Attach(entity);
            }
            entity.IsActive = true;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }
    }
}
