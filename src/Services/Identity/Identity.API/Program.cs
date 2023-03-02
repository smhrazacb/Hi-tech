using Identity.API;
using IdentityServer4.Models;
using IdentityServer4.Test;
using IdentityServerHost.Quickstart.UI;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddIdentityServer()
 .AddInMemoryClients(Config.Clients)
 .AddInMemoryApiScopes(Config.ApiScopes)
 .AddInMemoryIdentityResources(Config.IdentityResources)
 .AddTestUsers(Config.TestUsers)
 .AddDeveloperSigningCredential();

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();

app.UseEndpoints(endponits =>
{
    endponits.MapDefaultControllerRoute();
});

app.Run();
