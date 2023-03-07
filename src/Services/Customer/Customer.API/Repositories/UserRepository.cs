using Customer.API.Data;
using Customer.API.Entities;
using Customer.API.Entities.Dtos;
using Customer.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Runtime.CompilerServices;

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
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task<int> DeleteUser(string id)
        {
            return await _context.Users
                 .Where(t => t.Id == id)
                 .ExecuteDeleteAsync();
        }
        public async Task<User> GetUserById(string id)
        {
            return await
            _context.Users.Where(user => user.Id == id)
            .Include(ur => ur.Address)
            .Include(user => user.Address.GeoData)
            .FirstAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await
            _context.Users.Where(user => user.Email == email)
            .Include(ur => ur.Address)
            .Include(user => user.Address.GeoData)
            .FirstAsync();
        }

        public async Task<UserKey> GetUserKeys(string id)
        {
            return await
             _context.Users.Where(user => user.Id == id)
             .Include(ur => ur.Address)
             .Include(user => user.Address.GeoData)
             .Select(user => new UserKey
             {
                 Id = user.Id,
                 AddressId = user.AddressId,
                 GeoDataId = user.Address.GeoDataId,
                 AddedDate = user.AddedDate
             })
             .FirstAsync();
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users
         .Include(user => user.Address)
         .Include(user => user.Address.GeoData)
         .ToListAsync();
        }
        public async Task<int> UpdateUser(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }
    }
}
