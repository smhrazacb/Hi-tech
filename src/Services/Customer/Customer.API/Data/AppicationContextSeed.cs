using Customer.API.Entities;
using Customer.API.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using System;
using System.Globalization;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Customer.API.Data
{
    public class AppicationContextSeed
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider, ILogger<AppicationContextSeed> logger)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();
           await RegisterApplicationsAsync(serviceProvider);
            //if (!context.Users.Any())
            //{
            //    string[] roles = new string[] { "Owner", "Administrator", "Manager", "Editor", "Buyer", "Business", "Seller", "Subscriber" };

            //    foreach (var user in PreconfiguredUser)
            //    {
            //        var userStore = new UserStore<ApplicationUser>(context);
            //        var result = await userStore.CreateAsync(user);
            //    }
            //    foreach (var user in PreconfiguredUser)
            //        //AssignRoles(serviceProvider, user.Email, roles);
            //        await context.SaveChangesAsync();
            //}
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

        static async Task RegisterApplicationsAsync(IServiceProvider provider)
        {
            var manager = provider.GetRequiredService<IOpenIddictApplicationManager>();

            if (await manager.FindByClientIdAsync("console") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "console",
                    ConsentType = ConsentTypes.Systematic,
                    DisplayName = "Console client application",
                    DisplayNames =
                    {
                        [CultureInfo.GetCultureInfo("fr-FR")] = "Application cliente console"
                    },
                    RedirectUris =
                    {
                        new Uri("http://localhost:49152/callback/login/local"),
                        new Uri("http://localhost:49153/callback/login/local"),
                        new Uri("http://localhost:49154/callback/login/local"),
                        new Uri("http://localhost:49155/callback/login/local"),
                        new Uri("http://localhost:49156/callback/login/local"),
                        new Uri("http://localhost:49157/callback/login/local"),
                        new Uri("http://localhost:49158/callback/login/local"),
                        new Uri("http://localhost:49159/callback/login/local"),
                        new Uri("http://localhost:49160/callback/login/local"),
                        new Uri("http://localhost:49161/callback/login/local"),
                        new Uri("http://localhost:49162/callback/login/local")
                    },
                    Permissions =
                    {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.ResponseTypes.Code,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles,
                        Permissions.Prefixes.Scope + "demo_api"
                    },
                    Requirements =
                    {
                        Requirements.Features.ProofKeyForCodeExchange
                    }
                });
            }

            if (await manager.FindByClientIdAsync("mvc") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "mvc",
                    ClientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3654",
                    ConsentType = ConsentTypes.Explicit,
                    DisplayName = "MVC client application",
                    DisplayNames =
                    {
                        [CultureInfo.GetCultureInfo("fr-FR")] = "Application cliente MVC"
                    },
                    RedirectUris =
                    {
                        new Uri("https://localhost:44381/callback/login/local")
                    },
                    PostLogoutRedirectUris =
                    {
                        new Uri("https://localhost:44381/callback/logout/local")
                    },
                    Permissions =
                    {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Logout,
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.ResponseTypes.Code,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles,
                        Permissions.Prefixes.Scope + "demo_api"
                    },
                    Requirements =
                    {
                        Requirements.Features.ProofKeyForCodeExchange
                    }
                });
            }

            if (await manager.FindByClientIdAsync("winforms") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "winforms",
                    ConsentType = ConsentTypes.Systematic,
                    DisplayName = "WinForms client application",
                    DisplayNames =
                    {
                        [CultureInfo.GetCultureInfo("fr-FR")] = "Application cliente WinForms"
                    },
                    RedirectUris =
                    {
                        new Uri("com.openiddict.sandbox.winforms.client:/callback/login/local")
                    },
                    Permissions =
                    {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.ResponseTypes.Code,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles,
                        Permissions.Prefixes.Scope + "demo_api"
                    },
                    Requirements =
                    {
                        Requirements.Features.ProofKeyForCodeExchange
                    }
                });
            }

            if (await manager.FindByClientIdAsync("wpf") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "wpf",
                    ConsentType = ConsentTypes.Systematic,
                    DisplayName = "WPF client application",
                    DisplayNames =
                    {
                        [CultureInfo.GetCultureInfo("fr-FR")] = "Application cliente WPF"
                    },
                    RedirectUris =
                    {
                        new Uri("com.openiddict.sandbox.wpf.client:/callback/login/local")
                    },
                    Permissions =
                    {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.ResponseTypes.Code,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles,
                        Permissions.Prefixes.Scope + "demo_api"
                    },
                    Requirements =
                    {
                        Requirements.Features.ProofKeyForCodeExchange
                    }
                });
            }

            // Note: when using introspection instead of local token validation,
            // an application entry MUST be created to allow the resource server
            // to communicate with OpenIddict's introspection endpoint.
            if (await manager.FindByClientIdAsync("resource_server") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "resource_server",
                    ClientSecret = "80B552BB-4CD8-48DA-946E-0815E0147DD2",
                    Permissions =
                    {
                        Permissions.Endpoints.Introspection
                    }
                });
            }

            // To test this sample with Postman, use the following settings:
            //
            // * Authorization URL: https://localhost:44395/connect/authorize
            // * Access token URL: https://localhost:44395/connect/token
            // * Client ID: postman
            // * Client secret: [blank] (not used with public clients)
            // * Scope: openid email profile roles
            // * Grant type: authorization code
            // * Request access token locally: yes
            if (await manager.FindByClientIdAsync("postman") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "postman",
                    ConsentType = ConsentTypes.Systematic,
                    DisplayName = "Postman",
                    RedirectUris =
                    {
                        new Uri("https://oauth.pstmn.io/v1/callback")
                    },
                    Permissions =
                    {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Device,
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.DeviceCode,
                        Permissions.GrantTypes.Password,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.ResponseTypes.Code,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles
                    }
                });
            }
        }
        public static IEnumerable<OpenIddictApplicationDescriptor> PreconfiguredOpenIddictApplicationDescriptors
        {
            get
            {
                var list = new List<OpenIddictApplicationDescriptor>();
                {
                    new OpenIddictApplicationDescriptor
                    {
                        ClientId = "oprderapi",
                        ClientSecret = "901562A5-E7FE-42CB-B10D-61EF6A8F3654",
                        ConsentType = ConsentTypes.Explicit,
                        DisplayName = "Order API",
                        Permissions =
                            {
                            Permissions.Endpoints.Token,
                            Permissions.GrantTypes.AuthorizationCode,
                            Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.ResponseTypes.Code,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles,
                        Permissions.Prefixes.Scope + "api"
                            },
                    };
                    new OpenIddictApplicationDescriptor
                    {
                        ClientId = "oprderapi",
                        ClientSecret = "901562A5-E7FE-42CB-B10D-61EF6A8F3654",
                        ConsentType = ConsentTypes.Explicit,
                        DisplayName = "Order API",
                        Permissions =
                            {
                            Permissions.Endpoints.Token,
                            Permissions.GrantTypes.AuthorizationCode,
                            Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.ResponseTypes.Code,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles,
                        Permissions.Prefixes.Scope + "api"
                            },
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