using Customer.API.Data;
using Customer.API.Entities;
using Customer.API.Extensions;
using Customer.API.Repositories;
using Customer.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Quartz;
using System.Reflection;
using static OpenIddict.Abstractions.OpenIddictConstants;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
    options.UseOpenIddict();
});
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register the Identity services.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
})
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

            // Register the OpenIddict client components.
            .AddClient(options =>
            {
                // Note: this sample uses the code flow, but you can enable the other flows if necessary.
                options.AllowAuthorizationCodeFlow();

                // Register the signing and encryption credentials used to protect
                // sensitive data like the state tokens produced by OpenIddict.
                options.AddDevelopmentEncryptionCertificate()
                       .AddDevelopmentSigningCertificate();

                // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                options.UseAspNetCore()
                       .EnableStatusCodePagesIntegration()
                       .EnableRedirectionEndpointPassthrough();

                // Register the System.Net.Http integration and use the identity of the current
                // assembly as a more specific user agent, which can be useful when dealing with
                // providers that use the user agent as a way to throttle requests (e.g Reddit).
                options.UseSystemNetHttp()
                       .SetProductInformation(typeof(Program).Assembly);

                // Register the Web providers integrations.
                //
                // Note: to mitigate mix-up attacks, it's recommended to use a unique redirection endpoint
                // URI per provider, unless all the registered providers support returning a special "iss"
                // parameter containing their URL as part of authorization responses. For more information,
                // see https://datatracker.ietf.org/doc/html/draft-ietf-oauth-security-topics#section-4.4.
                options.UseWebProviders()
                       .UseGitHub()
                       .SetClientId("c4ade52327b01ddacff3")
                       .SetClientSecret("da6bed851b75e317bf6b2cb67013679d9467c122")
                       .SetRedirectUri("callback/login/github");
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

                // Register the signing and encryption credentials.
                options.AddDevelopmentEncryptionCertificate()
                       .AddDevelopmentSigningCertificate();

                // Force client applications to use Proof Key for Code Exchange (PKCE).
                options.RequireProofKeyForCodeExchange();

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
                // options.DisableAccessTokenEncryption();
            })

            // Register the OpenIddict validation components.
            .AddValidation(options =>
            {
                // Configure the audience accepted by this resource server.
                // The value MUST match the audience associated with the
                // "demo_api" scope, which is used by ResourceController.
                options.AddAudiences("resource_server");

                // Import the configuration from the local OpenIddict server instance.
                options.UseLocalServer();

                // Instead of validating the token locally by reading it directly,
                // introspection can be used to ask a remote authorization server
                // to validate the token (and its attached database entry).
                //
                // options.UseIntrospection()
                //        .SetIssuer("https://localhost:44395/")
                //        .SetClientId("resource_server")
                //        .SetClientSecret("80B552BB-4CD8-48DA-946E-0815E0147DD2");
                //
                // When introspection is used, System.Net.Http integration must be enabled.
                //
                // options.UseSystemNetHttp();

                // Register the ASP.NET Core host.
                options.UseAspNetCore();

                // For applications that need immediate access token or authorization
                // revocation, the database entry of the received tokens and their
                // associated authorizations can be validated for each API call.
                // Enabling these options may have a negative impact on performance.
                //
                // options.EnableAuthorizationEntryValidation();
                // options.EnableTokenEntryValidation();
            });

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Newtonsoftjson added due to "a cycle or if the object depth is larger than the maximum allowed depth of 32"
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
); ;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "User's API",
        Description = "To add items into shopping carts",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Register the worker responsible for seeding the database.
// Note: in a real world application, this step should be part of a setup script.
//builder.Services.AddHostedService<Worker>();

var app = builder.Build();

// Added due to 
//Cannot write DateTime with Kind=Local to PostgreSQL type 'timestamp
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// seed preConfigured data if db = null 
app.MigrateDatabase<ApplicationDbContext>((context, services) =>
{
    var logger = services.GetService<ILogger<AppicationContextSeed>>();
    AppicationContextSeed
        .SeedAsync(services, logger)
        .Wait();
});

app.Run();
