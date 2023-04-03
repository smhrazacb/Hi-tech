using Catalog.API.Data;
using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using Catalog.API.Repositories.Interfaces;
using Catalog.API.Services;
using Catalog.API.Utilities;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using Sylvan.Data;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddScoped<IProductContext, ProductContext>();
builder.Services.AddScoped<IProductRepositoryR, ProductRepositoryR>();
builder.Services.AddScoped<IProductRepositoryW, ProductRepositoryW>();
builder.Services.AddScoped<ICSV2Category, CSV2Category>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IUriService>(o =>
{
    var accessor = o.GetRequiredService<IHttpContextAccessor>();
    var request = accessor.HttpContext.Request;
    var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
    return new UriService(uri);
});
builder.Services.AddControllers();


//Register the OpenIddict validation components.
builder.Services.AddOpenIddict()
    .AddValidation(options =>
    {
        // Note: the validation handler uses OpenID Connect discovery
        // to retrieve the address of the introspection endpoint.
        options.SetIssuer(builder.Configuration.GetValue<string>("IdentityUrl"));
        //options.AddAudiences("catalog_server");

        // Configure the validation handler to use introspection and register the client
        // credentials used when communicating with the remote introspection endpoint.
        options.UseIntrospection()
        .SetClientSecret("80B552BB-4CD8-48DA-946E-0815E0147DD2")
               .SetClientId("catalog_server");

        // Register the System.Net.Http integration.
        options.UseSystemNetHttp();

        // Register the ASP.NET Core host.
        options.UseAspNetCore();
    });

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});
builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Catalog API",
        Description = "To browse update edit delete products",
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
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();
app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger()
    .UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product.API v1");
        c.OAuthClientId("catalogswagger");
        c.OAuthAppName("Ordering Swagger UI");
        c.OAuth2RedirectUrl("https://localhost:8000/swagger/oauth2-redirect.html");
        c.OAuthClientSecret("901564A5-E7FE-42CB-B10D-61EF6A8F365");
        //c.SupportedSubmitMethods(SubmitMethod.Post);
        c.OAuthUsePkce();
        ;
    });
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
