using Customer.API.Entities;
using Customer.API.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Customer.API.Data
{

    public class Worker : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public Worker(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync();

            await Initialize(_serviceProvider, scope);
            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
            if (await manager.FindByClientIdAsync("OrderAPI") == null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "OrderAPI",
                    ClientSecret = "901562A5-E7FE-42CB-B10D-61EF6A8F3654",
                    ConsentType = ConsentTypes.Explicit,
                    DisplayName = "Order API",
                    Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Logout,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.ResponseTypes.Code,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles
                },
                    Requirements =
                {
                    Requirements.Features.ProofKeyForCodeExchange
                }
                });
            }
        }
        public static async Task Initialize(IServiceProvider serviceProvider, AsyncServiceScope scope)
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            string[] roles = new string[] { "Owner", "Administrator", "Manager", "Editor", "Buyer", "Business", "Seller", "Subscriber" };

            foreach (string role in roles)
            {
                var roleStore = new RoleStore<IdentityRole>(context);

                if (!context.Roles.Any(r => r.Name == role))
                {
                   await roleStore.CreateAsync(new IdentityRole(role));
                }
            }

            var defaultUserName = "admin@admin.com";
            var defaultPassword = "Esp@rk123";
            var user = new ApplicationUser
            {
                UserName = defaultUserName,
                Email = defaultUserName,
                NormalizedUserName = Faker.Name.FullName(Faker.NameFormats.WithPrefix),
                OrderType = (EOrderType)Faker.RandomNumber.Next(0, 1),
                UserStatus = (EUserStatus)Faker.RandomNumber.Next(0, 2),
                PhoneNumber = Faker.Phone.Number(),
                Address = new Address()
                {
                    Country = Faker.Address.Country(),
                    State = Faker.Address.UsState(),
                    City = Faker.Address.City(),
                    NearByArea = Faker.Address.StreetName(),
                    HouseShopPlotNo = Faker.Address.SecondaryAddress(),
                    Addressline1 = Faker.Address.StreetAddress(),
                    GeoData = new GeoData()
                    {
                        Latitude = Faker.RandomNumber.Next(-89, 89),
                        longitude = Faker.RandomNumber.Next(-180, 180),
                    }
                }
            };


            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user, "secret");
                user.PasswordHash = hashed;

                var userStore = new UserStore<ApplicationUser>(context);
                var result = userStore.CreateAsync(user);

            }

            AssignRoles(serviceProvider, user.Email, roles);

            context.SaveChangesAsync();
        }

        public static async Task<IdentityResult> AssignRoles(IServiceProvider services, string email, string[] roles)
        {
            UserManager<ApplicationUser> _userManager = services.GetService<UserManager<ApplicationUser>>();
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            var result = await _userManager.AddToRolesAsync(user, roles);

            return result;
        }
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}