using FarmFresh.Backend.Repositories.Interfaces;
using FarmFresh.Backend.Storages.SQLServer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FarmFresh.Backend.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ChangeName(Guid id, string newName)
        {
            var entity = await _context
               .Users
               .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
            if (entity == null)
            {
                throw new Exception($"User with id {id} is not found / been de-activated");
            }
            entity.FullName = newName;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }

        public async Task SetInactive(Guid id)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
            if (entity == null)
            {
                throw new Exception($"User with id: {id} is not found");
            }
            if (!_context.Users.Local.Any(c => c.Id == entity.Id))
            {
                _context.Users.Attach(entity);
            }
            entity.IsActive = false;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }


        public async Task SetActive(Guid id)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(c => c.Id == id && !c.IsActive);
            if (entity == null)
            {
                throw new Exception($"User with id: {id} is not found");
            }
            if (!_context.Users.Local.Any(c => c.Id == entity.Id))
            {
                _context.Users.Attach(entity);
            }
            entity.IsActive = true;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }
    }
}
