using Customer.API.Entities;

namespace Customer.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetUsers();
        public Task<User> UpdateUser(User user);
        public Task<User> CreateUser(User user);
        public Task<bool> DeleteUser(int id);
        public Task<User> GetUser(int id);
        public Task<User> GetUser(string email);
    }
}
