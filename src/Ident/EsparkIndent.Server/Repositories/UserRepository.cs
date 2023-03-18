using EsparkIndent.Server.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace EsparkIndent.Server
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IdentityResult> CreateUser(ApplicationUser user)
        {
            return await _userManager.CreateAsync(user);
            //_context.Users.Add(user);
            //await _context.SaveChangesAsync();
        }
        public async Task<int> DeleteUser(string id)
        {
            return await _context.Users
                 .Where(t => t.Id == id)
                 .ExecuteDeleteAsync();
        }
        public async Task<ApplicationUser> GetUserById(string id)
        {
            return await
            _context.Users.Where(user => user.Id == id)
            .Include(ur => ur.Address)
            .Include(user => user.Address.GeoData)
            .FirstAsync();
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
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

        public async Task<IEnumerable<ApplicationUser>> GetUsers()
        {
            return await _context.Users
         .Include(user => user.Address)
         .Include(user => user.Address.GeoData)
         .ToListAsync();
        }
        public async Task<int> UpdateUser(ApplicationUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }
    }
}
