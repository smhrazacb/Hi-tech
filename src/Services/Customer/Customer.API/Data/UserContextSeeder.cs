using Customer.API.Entities;

namespace Customer.API.Data
{
    public class UserContextSeeder
    {
        public static async Task SeedAsync(UserContext userContext)
        {
            if (!userContext.Users.Any())
            {
                userContext.Users.AddRange(GetPreconfiguredOrders());
                await userContext.SaveChangesAsync();
            }
        }
        private static IEnumerable<User> GetPreconfiguredOrders()
        {
            return new List<User>
            {
                new User() {UserFullName = "swn", EmailAddress = "ezozkme@gmail.com" }
            };
        }
    }
}
