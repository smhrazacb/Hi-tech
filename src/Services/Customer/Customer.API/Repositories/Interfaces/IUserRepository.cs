using Customer.API.Entities;
using Customer.API.Entities.Dtos;

namespace Customer.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetUsers();
        public Task<int> UpdateUser(User user);
        public Task CreateUser(User user);
        public Task<int> DeleteUser(string id);
        public Task<User> GetUserById(string id);
        public Task<UserKey> GetUserKeys(string id);
        public Task<User> GetUserByEmail(string email);
    }
}
