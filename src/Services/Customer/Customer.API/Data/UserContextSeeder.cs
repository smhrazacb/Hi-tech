using Customer.API.Entities;
using Customer.API.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using System;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Customer.API.Data
{
    public class UserContextSeed
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider, ILogger<UserContextSeed> logger)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();
            if (!context.Users.Any())
            {
                string[] roles = new string[] { "Owner", "Administrator", "Manager", "Editor", "Buyer", "Business", "Seller", "Subscriber" };

                foreach (var user in PreconfiguredUser)
                {
                    var userStore = new UserStore<ApplicationUser>(context);
                    var result = await userStore.CreateAsync(user);
                }
                foreach (var user in PreconfiguredUser)
                    AssignRoles(serviceProvider, user.Email, roles);
                await context.SaveChangesAsync();
            }
            await using var scope = serviceProvider.CreateAsyncScope();
            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
            foreach (var item in PreconfiguredOpenIddictApplicationDescriptors)
            {
                if (await manager.FindByClientIdAsync(item.ClientId) == null)
                {
                    await manager.CreateAsync(item);
                }
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(ApplicationDbContext).Name);
            }
        }
        public static async Task<IdentityResult> AssignRoles(IServiceProvider services, string email, string[] roles)
        {
            UserManager<ApplicationUser> _userManager = services.GetService<UserManager<ApplicationUser>>();
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            var result = await _userManager.AddToRolesAsync(user, roles);
            return result;
        }
        public static IEnumerable<OpenIddictApplicationDescriptor> PreconfiguredOpenIddictApplicationDescriptors
        {
            get
            {
                var list = new List<OpenIddictApplicationDescriptor>();
                {
                    new OpenIddictApplicationDescriptor
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
                        Requirements = { Requirements.Features.ProofKeyForCodeExchange }
                    };
                }
                return list;
            }
        }


        public static IEnumerable<ApplicationUser> PreconfiguredUser
        {
            get
            {
                var userlist = new List<ApplicationUser>();
                {
                    for (int i = 0; i < 5; i++)
                    {
                        var user = new ApplicationUser()
                        {
                            Email = Faker.Internet.Email(),
                            UserName = Faker.Internet.Email(),
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
                        user.PasswordHash = new PasswordHasher<ApplicationUser>()
                            .HashPassword(user, "admin");
                        userlist.Add(user);
                    }
                    return userlist;
                }
            }
        }
    }
}