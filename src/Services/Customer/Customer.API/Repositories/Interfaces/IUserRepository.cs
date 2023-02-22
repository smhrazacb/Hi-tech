using Customer.API.Entities;

namespace Customer.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetUsers();
        public Task<User> UpdateUser(User user);
        public Task CreateUser(User user);
        public Task<bool> DeleteUser(Guid guid);
        public Task<User> GetUser(Guid guid);
        public Task<User> GetUser(string email);
    }
}
