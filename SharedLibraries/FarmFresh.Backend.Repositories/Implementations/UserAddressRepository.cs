using FarmFresh.Backend.Entities;
using FarmFresh.Backend.Repositories.Interfaces;
using FarmFresh.Backend.Storages.SQLServer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace FarmFresh.Backend.Repositories.Implementations
{
    public class UserAddressRepository: IUserAddressRepository
    {
        private readonly ApplicationDbContext _context;
        public UserAddressRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AppUserAddress> GetByUserId(Guid userId)
        {
            var entity = await _context
                .Users
                .Include(c => c.Addresses)
                .FirstOrDefaultAsync(c => c.Id == userId && c.IsActive);
            if(entity == null)
            {
                throw new Exception($"User with id {userId} is not found / been de-activated");
            }
            return entity.Addresses.FirstOrDefault();
        }

        public async Task Update(AppUserAddress entity)
        {
            if(!await _context.Users.AnyAsync(c => c.Id == entity.UserId && c.IsActive))
            {
                throw new Exception($"User with id {entity.UserId} is not found / been de-activated");
            }
            if(!_context.UserAddresses.Local.Any(c => c.Id == entity.Id))
            {
                _context.UserAddresses.Attach(entity);
            }
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
