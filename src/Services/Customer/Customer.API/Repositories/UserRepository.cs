using Customer.API.Data;
using Customer.API.Data.Interfaces;
using Customer.API.Entities;
using Customer.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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
        }

        public Task<bool> DeleteUser(Guid guid)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUser(Guid guid)
        {
            return await
                 _context.Users.Where(user => user.Id == guid)
                 .FirstAsync();
        }

        public Task<User> GetUser(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            for (int i = 0; i < users.Count; i++)
            {
                users[i].Address = await _context.Addresses.Where(a => a.Id == users[i].AddressId).FirstOrDefaultAsync();
                users[i].Address.Contact = await _context.Contacts.Where(a => a.Id == users[i].Address.ContactId).FirstOrDefaultAsync();
                if (users[i].Address.GeoDataId != null)
                {
                    users[i].Address.GeoData = await _context.GeoDatas.Where(a => a.Id == users[i].Address.GeoDataId).FirstOrDefaultAsync();
                }
            }
            return users;
        }

        public Task<User> UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
