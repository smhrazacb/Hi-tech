using EsparkIndent.Server.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EsparkIndent.Server
{
    public interface IUserRepository
    {
        public Task<IEnumerable<ApplicationUser>> GetUsers();
        public Task<int> UpdateUser(ApplicationUser user);
        public Task<IdentityResult> CreateUser(ApplicationUser user);
        public Task<int> DeleteUser(string id);
        public Task<ApplicationUser> GetUserById(string id);
        public Task<UserKey> GetUserKeys(string id);
        public Task<ApplicationUser> GetUserByEmail(string email);
    }
}
