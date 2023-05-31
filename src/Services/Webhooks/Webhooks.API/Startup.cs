using EventBus.Messages.Common;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Webhooks.API.Data;
using Webhooks.API.EventBusConsumer;
using Webhooks.API.Services;

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
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddDbContext<WebhooksContext>(options =>
                options.UseSqlite($"Filename={Path.Combine("dbEsparkIndent-Server.sqlite3")}",sqliteOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(Program).Assembly.FullName);
                }));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(configRoot["EventBusSettings:HostAddress"]);
                    cfg.ReceiveEndpoint(EventBusConstants.CatalogItemPriceChangeEvent, c =>
                    {
                        c.ConfigureConsumer<CatalogItemPriceChangeConsumer>(ctx);
                    });
                });
            });
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
    }
}
