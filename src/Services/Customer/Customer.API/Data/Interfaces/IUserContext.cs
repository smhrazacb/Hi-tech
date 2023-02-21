using Customer.API.Entities;

namespace Customer.API.Data.Interfaces
{
    public interface IUserContext
    {
        public IEnumerable<User> Users { get; set; }
    }
}
