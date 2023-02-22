using Customer.API.Data;
using Customer.API.Data.Interfaces;
using Customer.API.Extensions;
using Customer.API.Repositories;
using Customer.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<UserContext>(opt =>
        opt.UseNpgsql(builder.Configuration.GetValue<string>("DatabaseSettings:ConnectionString")));
builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Newtonsoftjson added due to "a cycle or if the object depth is larger than the maximum allowed depth of 32"
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
); ;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

app.UseAuthorization();

app.MapControllers();

// seed preConfigured data if db = null 
app.MigrateDatabase<UserContext>((context, services) =>
{
    var logger = services.GetService<ILogger<UserContextSeed>>();
    UserContextSeed
        .SeedAsync(context, logger)
        .Wait();
});

app.Run();
