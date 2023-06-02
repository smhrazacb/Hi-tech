using EventBus.Messages.Common;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;
using System.Security.Cryptography.X509Certificates;
using Webhooks.API.Data;
using Webhooks.API.EventBusConsumer;
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
            services.AddSwaggerGen();
            services.AddDbContext<WebhooksContext>(options =>
                options.UseSqlite($"Filename={Path.Combine("dbEsparkIndent-Server.sqlite3")}",sqliteOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(Startup).Assembly.FullName);
                }));

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
            services.AddMassTransit(config =>
            {
                config.AddConsumer<CatalogItemPriceChangeConsumer>();
                config.AddConsumer<OrderStatusChangedConsumer>();

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(configRoot["EventBusSettings:HostAddress"]);
                    cfg.ReceiveEndpoint(EventBusConstants.CatalogItemPriceChangeEvent, c =>
                    {
                        c.ConfigureConsumer<CatalogItemPriceChangeConsumer>(ctx);
                    });
                    cfg.ReceiveEndpoint(EventBusConstants.OrderStatusChangedToPaidEvent, c =>
                    {
                        c.ConfigureConsumer<OrderStatusChangedConsumer>(ctx);
                    });
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
        }
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(x => { x.SwaggerEndpoint("/swagger/v1/swagger.yaml", "Catalog API"); });
            }
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
        X509Certificate2 LoadCertificate(string thumbprint)
        {
            var bytes = File.ReadAllBytes(thumbprint);
            return new X509Certificate2(bytes, "123456");
        }
    }
}
