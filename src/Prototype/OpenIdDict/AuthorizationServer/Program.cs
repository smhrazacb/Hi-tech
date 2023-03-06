using AuthorizationServer.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DbContext>(options =>
{
    // Configure the context to use an in-memory store.
    options.UseInMemoryDatabase(nameof(DbContext));

    // Register the entity sets needed by OpenIddict.
    options.UseOpenIddict();
});

builder.Services.AddOpenIddict()

        // Register the OpenIddict core components.
        .AddCore(options =>
        {
            // Configure OpenIddict to use the EF Core stores/models.
            options.UseEntityFrameworkCore()
                .UseDbContext<DbContext>();
        })

        // Register the OpenIddict server components.
        .AddServer(options =>
        {
            options
                .AllowClientCredentialsFlow()
                .AllowAuthorizationCodeFlow()
                .RequireProofKeyForCodeExchange()
                .AllowRefreshTokenFlow();

            options.SetAuthorizationEndpointUris("/connect/authorize")
            .SetTokenEndpointUris("/connect/token")
            .SetUserinfoEndpointUris("/connect/userinfo");

            // Encryption and signing of tokens
            options
                .AddEphemeralEncryptionKey()
                .AddEphemeralSigningKey()
                .DisableAccessTokenEncryption();

            // Register scopes (permissions)
            options.RegisterScopes("api");

            // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
            options
                .UseAspNetCore()
                .EnableTokenEndpointPassthrough()
                .EnableAuthorizationEndpointPassthrough()
                .EnableUserinfoEndpointPassthrough();

        });



builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.LoginPath = "/account/login";
        });

builder.Services.AddHostedService<TestData>();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});
app.Run();
