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
    public class StoreRepository : IStoreRepository
    {
        private readonly ApplicationDbContext _context;
        public StoreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AppStore> GetById(Guid id)
        {
            var entity = await _context.Stores.FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
            if(entity == null)
            {
                throw new Exception($"Store not found / been de-activated");
            }
            return entity;
        }

        public async Task<Guid> Insert(AppStore entity)
        {
            if(entity.UserId == Guid.Empty)
            {
                throw new Exception($"User id for the admin store cannot be empty!");
            }
            _context.Stores.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task ChangeName(Guid id, string newName)
        {
            var entity = await GetById(id);
            entity.Name = newName;
            if(!_context.Stores.Local.Any(c => c.Id == id))
            {
                _context.Stores.Attach(entity);
            }
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }

        public async Task SetInactive(Guid id)
        {
            var entity = await _context.Stores.FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
            if (entity == null)
            {
                throw new Exception($"Store with id: {id} is not found");
            }
            if (!_context.Stores.Local.Any(c => c.Id == entity.Id))
            {
                _context.Stores.Attach(entity);
            }
            entity.IsActive = false;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task SetActive(Guid id)
        {
            var entity = await _context.Stores.FirstOrDefaultAsync(c => c.Id == id && !c.IsActive);
            if (entity == null)
            {
                throw new Exception($"Store with id: {id} is not found");
            }
            if (!_context.Stores.Local.Any(c => c.Id == entity.Id))
            {
                _context.Stores.Attach(entity);
            }
            entity.IsActive = true;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
