using EventBus.Messages.Common;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Webhooks.API.Data;
using Webhooks.API.EventBusConsumer;
using Webhooks.API.Extensions;
using Webhooks.API.Services;
using Webhooks.API.Services.Services;

namespace Webhooks.API
{
    public class Startup
    {
        public IConfiguration configRoot
        {
            get;
        }
        public Startup(IConfiguration configuration)
        {
            configRoot = configuration;

        }
        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.

            services.AddControllers();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // Add services to the container.
            services.AddTransient<IIdentityService, IdentityService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddDbContext<WebhooksContext>(options =>
               options.UseNpgsql(configRoot.GetConnectionString("WebhookConnectionString")));
            services.AddHttpClient("extendedhandlerlifetime").SetHandlerLifetime(Timeout.InfiniteTimeSpan);
            //add http client services
            services.AddHttpClient("GrantClient")
                    .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                        .AddTransient<IGrantUrlTesterService, GrantUrlTesterService>()
                        .AddTransient<IWebhooksRetriever, WebhooksRetriever>()
                        .AddTransient<IWebhooksSender, WebhooksSender>();

            // Add RabitMQ Configuration 
            // MassTransit-RabbitMQ Configuration
            //services.AddMassTransit(config =>
            //{
            //    config.AddConsumer<OrderStatusChangedConsumer>();

            //    config.UsingRabbitMq((ctx, cfg) =>
            //    {
            //        cfg.Host(configRoot["EventBusSettings:HostAddress"]);
            //        cfg.ReceiveEndpoint(EventBusConstants.OrderStatus, c =>
            //        {
            //            c.ConfigureConsumer<OrderStatusChangedConsumer>(ctx);
            //        });
            //    });
            //});
            services.AddMassTransit(config =>
            {
                //config.AddConsumer<CatalogItemPriceChangeConsumer>();
                config.AddConsumer<OrderStatusChangedConsumer>();

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(configRoot["EventBusSettings:HostAddress"]);
                    //cfg.ReceiveEndpoint(EventBusConstants.CatalogItemPriceChangeQueue, c =>
                    //{
                    //    c.ConfigureConsumer<CatalogItemPriceChangeConsumer>(ctx);
                    //});
                    cfg.ReceiveEndpoint(EventBusConstants.OrderStatus, c =>
                    {
                        c.ConfigureConsumer<OrderStatusChangedConsumer>(ctx);
                    });
                    //cfg.ReceiveEndpoint(EventBusConstants.OrderConfirmQueue, c =>
                    //{
                    //    c.ConfigureConsumer<OrderStatusChangedConsumer>(ctx);
                    //});
                    //cfg.ReceiveEndpoint(EventBusConstants.OrderStatusChangedToCancelQueue, c =>
                    //{
                    //    c.ConfigureConsumer<OrderStatusChangedConsumer>(ctx);
                    //}); 
                });
            });

            //Register the OpenIddict validation components.
            services.AddOpenIddict()
                .AddValidation(options =>
                {
                    options.AddEncryptionCertificate(LoadCertificate(
                            "_encryption-certificate.pfx"));
                    // Note: the validation handler uses OpenID Connect discovery
                    // to retrieve the address of the introspection endpoint.
                    options.SetIssuer(configRoot.GetValue<string>("IdentityUrl"));
                    //options.AddAudiences("catalog_server");
                    // Configure the validation handler to use introspection and register the client
                    // credentials used when communicating with the remote introspection endpoint.
                    options.UseIntrospection()
                    .SetClientSecret("80B552BB-4CD8-48DA-946E-0815E0147DD9")
                           .SetClientId("webhook_server");

                    // Register the System.Net.Http integration.
                    options.UseSystemNetHttp();

                    // Register the ASP.NET Core host.
                    options.UseAspNetCore();
                });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            });
            services.AddAuthorization();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Shopping Aggregator API",
                    Description = "For Aggregated",
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
                                Id = "Bearer"   },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
        }
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MigrateDatabase<WebhooksContext>((context, services) =>
            {
                var logger = services.GetService<ILogger<WebhookContextSeed>>();
                WebhookContextSeed
                    .SeedAsync(context, logger)
                    .Wait();
            });
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.Run();
        }
        X509Certificate2 LoadCertificate(string thumbprint)
        {
            var bytes = File.ReadAllBytes(thumbprint);
            return new X509Certificate2(bytes, "123456");
        }
    }
}
