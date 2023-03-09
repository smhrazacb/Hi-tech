using Customer.API.Entities;
using Customer.API.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Customer.API.Data
{
    public class UserContextSeed
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider, ILogger<UserContextSeed> logger)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();

            if (!context.Users.Any())
            {        string[] roles = new string[] { "Owner", "Administrator", "Manager", "Editor", "Buyer", "Business", "Seller", "Subscriber" };

                foreach (var user in PreconfiguredUser)
                {
                    var userStore = new UserStore<ApplicationUser>(context);
                    var result = userStore.CreateAsync(user);
                    AssignRoles(serviceProvider, user.Email, roles);
                }
                await context.SaveChangesAsync();
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
                            NormalizedUserName = Faker.Name.FullName(Faker.NameFormats.WithPrefix),
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