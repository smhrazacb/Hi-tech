using EsparkIndent.Server.Entities;
using EsparkIndent.Server.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using System.Configuration;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using static OpenIddict.Abstractions.OpenIddictConstants;

var builder = WebApplication.CreateBuilder(args);
// Add builder.Services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // Configure the context to use Microsoft SQL Server.
    options.UseNpgsql(builder.Configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

    // Register the entity sets needed by OpenIddict.
    // Note: use the generic overload if you need
    // to replace the default OpenIddict entities.
    options.UseOpenIddict();
});

// Register the Identity builder.Services.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// OpenIddict offers native integration with Quartz.NET to perform scheduled tasks
// (like pruning orphaned authorizations/tokens from the database) at regular intervals.
builder.Services.AddQuartz(options =>
{
    options.UseMicrosoftDependencyInjectionJobFactory();
    options.UseSimpleTypeLoader();
    options.UseInMemoryStore();
});

// Register the Quartz.NET service and configure it to block shutdown until jobs are complete.
builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

builder.Services.AddOpenIddict()

    // Register the OpenIddict core components.
    .AddCore(options =>
    {
        // Configure OpenIddict to use the Entity Framework Core stores and models.
        // Note: call ReplaceDefaultEntities() to replace the default OpenIddict entities.
        options.UseEntityFrameworkCore()
               .UseDbContext<ApplicationDbContext>();

        // Developers who prefer using MongoDB can remove the previous lines
        // and configure OpenIddict to use the specified MongoDB database:
        // options.UseMongoDb()
        //        .UseDatabase(new MongoClient().GetDatabase("openiddict"));

        // Enable Quartz.NET integration.
        options.UseQuartz();
    })
    // Register the OpenIddict server components.
    .AddServer(options =>
    {
        // Enable the authorization, device, introspection,
        // logout, token, userinfo and verification endpoints.
        options.SetAuthorizationEndpointUris("connect/authorize")
               .SetDeviceEndpointUris("connect/device")
               .SetIntrospectionEndpointUris("connect/introspect")
               .SetLogoutEndpointUris("connect/logout")
               .SetTokenEndpointUris("connect/token")
               .SetUserinfoEndpointUris("connect/userinfo")
               .SetVerificationEndpointUris("connect/verify");

        // Note: this sample uses the code, device code, password and refresh token flows, but you
        // can enable the other flows if you need to support implicit or client credentials.
        options.AllowAuthorizationCodeFlow()
               .AllowDeviceCodeFlow()
               .AllowPasswordFlow()
               .AllowRefreshTokenFlow();

        // Mark the "email", "profile", "roles" and "demo_api" scopes as supported scopes.
        options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles, "demo_api");

        // Set the lifetime of your tokens
        options.SetAccessTokenLifetime(TimeSpan.FromMinutes(5));
        options.SetRefreshTokenLifetime(TimeSpan.FromDays(1));
        // Register the signing and encryption credentials.
        //options.AddDevelopmentEncryptionCertificate()
        //       .AddDevelopmentSigningCertificate();
        options.AddEncryptionCertificate(LoadCertificate(
                "_encryption-certificate.pfx"));
        options.AddSigningCertificate(LoadCertificate("_signing-certificate.pfx"));
        // Force client applications to use Proof Key for Code Exchange (PKCE).
        options.RequireProofKeyForCodeExchange();

        // Disable ssl https for development 
        options.UseAspNetCore().DisableTransportSecurityRequirement();

        // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
        options.UseAspNetCore()
               .EnableStatusCodePagesIntegration()
               .EnableAuthorizationEndpointPassthrough()
               .EnableLogoutEndpointPassthrough()
               .EnableTokenEndpointPassthrough()
               .EnableUserinfoEndpointPassthrough()
               .EnableVerificationEndpointPassthrough();

        // Note: if you don't want to specify a client_id when sending
        // a token or revocation request, uncomment the following line:
        //
        // options.AcceptAnonymousClients();

        // Note: if you want to process authorization and token requests
        // that specify non-registered scopes, uncomment the following line:
        //
        // options.DisableScopeValidation();

        // Note: if you don't want to use permissions, you can disable
        // permission enforcement by uncommenting the following lines:
        //
        // options.IgnoreEndpointPermissions()
        //        .IgnoreGrantTypePermissions()
        //        .IgnoreResponseTypePermissions()
        //        .IgnoreScopePermissions();

        // Note: when issuing access tokens used by third-party APIs
        // you don't own, you can disable access token encryption:
        //
        //options.DisableAccessTokenEncryption();
    });


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Newtonsoftjson added due to "a cycle or if the object depth is larger than the maximum allowed depth of 32"
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
); ;

builder.Services.AddTransient<IEmailSender, AuthMessageSender>();
builder.Services.AddTransient<ISmsSender, AuthMessageSender>();



var app = builder.Build();

// for https disabling
app.UseForwardedHeaders();
//app.UseDeveloperExceptionPage();
//app.UseStatusCodePagesWithReExecute("/error");
// Added due to 
//Cannot write DateTime with Kind=Local to PostgreSQL type 'timestamp
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//app cors
//app.UseCors("corsapp");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// seed preConfigured data if db = null 
app.MigrateDatabase<ApplicationDbContext>((context, services) =>
{
    var logger = services.GetService<ILogger<AppicationContextSeed>>();
    AppicationContextSeed
        .SeedAsync(services, logger)
        .Wait();
});

app.Run();

string AssemblyDirectory()
{
    string codeBase = Assembly.GetExecutingAssembly().CodeBase;
    UriBuilder uri = new UriBuilder(codeBase);
    string path = Uri.UnescapeDataString(uri.Path);
    return Path.GetDirectoryName(path);
}
X509Certificate2 LoadCertificate(string thumbprint)
{
    var bytes = File.ReadAllBytes(thumbprint);
    return new X509Certificate2(bytes, "123456");
}