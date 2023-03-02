using Customer.API.Data;
using Customer.API.Data.Interfaces;
using Customer.API.Entities;
using Customer.API.Entities.Dtos;
using Customer.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;

namespace Customer.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;

        public UserRepository(UserContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { }

        }
        public async Task<int> DeleteUser(Guid guid)
        {
            try
            {
                return await _context.Users
                     .Where(t => t.Id == guid)
                     .ExecuteDeleteAsync();
            }
            catch (Exception ex) { return -1; }
        }
        public async Task<User> GetUser(Guid guid)
        {
            try
            {
                return await
                _context.Users.Where(user => user.Id == guid)
                .Include(ur => ur.Address)
                .Include(user => user.Address.Contact)
                .Include(user => user.Address.GeoData)
                .FirstAsync();
            }
            catch (Exception ex) { return null; }
        }

        public async Task<User> GetUser(string email)
        {
            try
            {
                return await
                _context.Users.Where(user => user.EmailAddress == email)
                .Include(ur => ur.Address)
                .Include(user => user.Address.Contact)
                .Include(user => user.Address.GeoData)
                .FirstAsync();
            }
            catch (Exception ex) { return null; }
        }

        public async Task<UserKey> GetUserKeys(Guid guid)
        {
            try
            {
                return await
                 _context.Users.Where(user => user.Id == guid)
                 .Include(ur => ur.Address)
                 .Include(user => user.Address.Contact)
                 .Include(user => user.Address.GeoData)
                 .Select(user => new UserKey
                 {
                     Id = user.Id,
                     AddressId = user.AddressId,
                     ContactId = user.Address.ContactId,
                     GeoDataId = user.Address.GeoDataId,
                     AddedDate = user.AddedDate
                 })
                 .FirstAsync();
            }
            catch (Exception ex) { return null; }
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            try
            {
                return await _context.Users
             .Include(user => user.Address)
             .Include(user => user.Address.Contact)
             .Include(user => user.Address.GeoData)
             .ToListAsync();
            }
            catch (Exception ex) { return null; }
        }
        public async Task<int> UpdateUser(User user)
        {
            try
            {
                _context.Entry(user).State = EntityState.Modified;
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex) { return -1; }
        }
    }
}
