using Microsoft.OpenApi.Models;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
// Config Enviroment 
builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);
// Configure JSON logging to the console.
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddOcelotWithSwaggerSupport(options =>
{
    options.Folder = ".";
});
builder.Services.AddOcelot();
builder.Services.AddSwaggerForOcelot(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwaggerForOcelotUI(options =>
{
    options.PathToSwaggerGenerator = "/swagger/docs";
}).UseOcelot().Wait();

app.MapGet("/", () => "Hello World!");

app.Run();
