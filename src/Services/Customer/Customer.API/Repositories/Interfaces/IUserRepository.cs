using Customer.API.Entities;
using Customer.API.Entities.Dtos;

namespace Customer.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<IEnumerable<ApplicationUser>> GetUsers();
        public Task<int> UpdateUser(ApplicationUser user);
        public Task CreateUser(ApplicationUser user);
        public Task<int> DeleteUser(string id);
        public Task<ApplicationUser> GetUserById(string id);
        public Task<UserKey> GetUserKeys(string id);
        public Task<ApplicationUser> GetUserByEmail(string email);
    }
}
