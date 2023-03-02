using Customer.API.Entities;
using Customer.API.Entities.Dtos;

namespace Customer.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetUsers();
        public Task<int> UpdateUser(User user);
        public Task CreateUser(User user);
        public Task<int> DeleteUser(Guid guid);
        public Task<User> GetUser(Guid guid);
        public Task<UserKey> GetUserKeys(Guid guid);
        public Task<User> GetUser(string email);
    }
}
