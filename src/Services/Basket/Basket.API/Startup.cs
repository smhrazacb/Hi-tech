﻿using Basket.API.Services.Interfaces;
using Basket.API.Services;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using Basket.API.Services.Services;
using MassTransit;
using Basket.API.EventBusConsumer;
using EventBus.Messages.Common;
using Basket.API.Mapper;

namespace Basket.API
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
            services.AddControllers().AddNewtonsoftJson();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IIdentityService, IdentityService>();

            // Add RabitMQ Configuration 
            // MassTransit-RabbitMQ Configuration
            services.AddMassTransit(config =>
            {
                config.AddConsumer<BasketDeleteConsumer>();

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(configRoot["EventBusSettings:HostAddress"]);
                    cfg.ReceiveEndpoint(EventBusConstants.BasketDeleteQueue, c =>
                    {
                        c.ConfigureConsumer<BasketDeleteConsumer>(ctx);
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
                    .SetClientSecret("80B552BB-4CD8-48DA-946E-0815E0147DD3")
                           .SetClientId("basket_server");

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
                    Title = "Basket API",
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
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
            // Redis Configuration
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configRoot.GetValue<string>("CacheSettings:ConnectionString");
            });
            services.AddAutoMapper(typeof(MappingProfile));
        }
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(x => { x.SwaggerEndpoint("/swagger/v1/swagger.yaml", "Basket API"); });
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
