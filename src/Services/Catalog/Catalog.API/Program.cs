using Catalog.API.Data;
using Catalog.API.Data.Interfaces;
using Catalog.API.Repositories;
using Catalog.API.Repositories.Interfaces;
using Catalog.API.Services;
using Catalog.API.Utilities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

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


////Register the OpenIddict validation components.
//builder.Services.AddOpenIddict()
//    .AddValidation(options =>
//    {
//        options.AddEncryptionCertificate(LoadCertificate(
//                "_encryption-certificate.pfx"));    
//        // Note: the validation handler uses OpenID Connect discovery
//        // to retrieve the address of the introspection endpoint.
//        options.SetIssuer(builder.Configuration.GetValue<string>("IdentityUrl"));
//        //options.AddAudiences("catalog_server");
//        // Configure the validation handler to use introspection and register the client
//        // credentials used when communicating with the remote introspection endpoint.
//        options.UseIntrospection()
//        .SetClientSecret("80B552BB-4CD8-48DA-946E-0815E0147DD2")
//               .SetClientId("catalog_server");

//        // Register the System.Net.Http integration.
//        options.UseSystemNetHttp();

//        // Register the ASP.NET Core host.
//        options.UseAspNetCore();
//    });

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
//});
//builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Catalog API",
        Description = "To browse update items",
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
    });
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
X509Certificate2 LoadCertificate(string thumbprint)
{
    var bytes = File.ReadAllBytes(thumbprint);
    return new X509Certificate2(bytes, "123456");
}