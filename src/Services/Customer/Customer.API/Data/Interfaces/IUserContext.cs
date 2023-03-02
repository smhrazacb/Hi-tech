using Customer.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Customer.API.Data.Interfaces
{
    public interface IUserContext
    {
        public DbSet<User> Users { get; }
        public DbSet<Address> Addresses { get; }
        public DbSet<Contact> Contacts { get; }
        public DbSet<GeoData> GeoDatas { get; }
    }
}
