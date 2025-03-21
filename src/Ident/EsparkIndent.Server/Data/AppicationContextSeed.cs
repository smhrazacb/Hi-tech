﻿using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace EsparkIndent.Server.Entities
{
    public class AppicationContextSeed
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider, ILogger<AppicationContextSeed> logger)
        {
            await using var scope = serviceProvider.CreateAsyncScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync(CancellationToken.None);

            await RegisterApplicationsAsync(scope.ServiceProvider);
            await RegisterScopesAsync(scope.ServiceProvider);
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

            if (await manager.FindByClientIdAsync("spa_clientp") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "spa_clientp",
                    ClientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3654",
                    ConsentType = ConsentTypes.Explicit,
                    DisplayName = "SPA client application",
                    RedirectUris =
                    {
                        new Uri("https://oauth.pstmn.io/v1/callback")
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
                        Permissions.Prefixes.Scope + "order_api",
                        Permissions.Prefixes.Scope + "catalog_api",
                        Permissions.Prefixes.Scope + "basket_api",
                        Permissions.Prefixes.Scope + "webhook_api",
                    },
                    Requirements =
                    {
                        Requirements.Features.ProofKeyForCodeExchange
                    }
                });
            }
            if (await manager.FindByClientIdAsync("shopping_aggrigator_server") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "shopping_aggrigator_server",
                    ClientSecret = "secret",
                    ConsentType = ConsentTypes.Explicit,

                    Permissions =
                    {
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.GrantTypes.ClientCredentials,

                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles,
                        Permissions.Prefixes.Scope + "order_api",
                        Permissions.Prefixes.Scope + "catalog_api",
                        Permissions.Prefixes.Scope + "basket_api",                    }
                });
            }
            if (await manager.FindByClientIdAsync("TestId") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "TestId",
                    ClientSecret = "secret",
                    ConsentType = ConsentTypes.Explicit,

                    Permissions =
                    {
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.GrantTypes.ClientCredentials,

                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles,
                        Permissions.Prefixes.Scope + "order_api",
                        Permissions.Prefixes.Scope + "catalog_api",
                        Permissions.Prefixes.Scope + "webhook_api",
                        Permissions.Prefixes.Scope + "basket_api",                    }
                });
            }
            // Note: when using introspection instead of local token validation,
            // an application entry MUST be created to allow the resource server
            // to communicate with OpenIddict's introspection endpoint.

            if (await manager.FindByClientIdAsync("webhook_server") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "webhook_server",
                    ClientSecret = "80B552BB-4CD8-48DA-946E-0815E0147DD9",
                    Permissions =
                    {
                        Permissions.Endpoints.Introspection,
                    }
                });
            }
            if (await manager.FindByClientIdAsync("catalog_server") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "catalog_server",
                    ClientSecret = "80B552BB-4CD8-48DA-946E-0815E0147DD2",
                    Permissions =
                    {
                        Permissions.Endpoints.Introspection,
                    }
                });
            }
            if (await manager.FindByClientIdAsync("basket_server") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "basket_server",
                    ClientSecret = "80B552BB-4CD8-48DA-946E-0815E0147DD3",
                    Permissions =
                    {
                        Permissions.Endpoints.Introspection,
                    }
                });
            }
            if (await manager.FindByClientIdAsync("order_server") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "order_server",
                    ClientSecret = "80B552BB-4CD8-48DA-946E-0815E0147DD4",
                    Permissions =
                    {
                        Permissions.Endpoints.Introspection,
                    }
                });
            }
        }
        static async Task RegisterScopesAsync(IServiceProvider provider)
        {
            var manager = provider.GetRequiredService<IOpenIddictScopeManager>();

            if (await manager.FindByNameAsync("webhook_api") is null)
            {
                await manager.CreateAsync(new OpenIddictScopeDescriptor
                {
                    DisplayName = "Webhook API access",
                    Name = "webhook_api",
                    Resources =
                    {
                        "webhook_server"
                    }
                });
            }
            if (await manager.FindByNameAsync("catalog_api") is null)
            {
                await manager.CreateAsync(new OpenIddictScopeDescriptor
                {
                    DisplayName = "Catalog API access",
                    Name = "catalog_api",
                    Resources =
                    {
                        "catalog_server"
                    }
                });
            }
            if (await manager.FindByNameAsync("basket_api") is null)
            {
                await manager.CreateAsync(new OpenIddictScopeDescriptor
                {
                    DisplayName = "Baket API access",
                    Name = "basket_api",
                    Resources =
                    {
                        "basket_server"
                    }
                });
            }
            if (await manager.FindByNameAsync("order_api") is null)
            {
                await manager.CreateAsync(new OpenIddictScopeDescriptor
                {
                    DisplayName = "Order API access",
                    Name = "order_api",
                    Resources =
                    {
                        "order_server"
                    }
                });
            }
        }

    }

}