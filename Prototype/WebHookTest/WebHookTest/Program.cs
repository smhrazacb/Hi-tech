using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebHookTest.Data;
using WebHookTest.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<WebhooksContext>(options =>
                options.UseSqlite($"Filename={Path.Combine("dbEsparkIndent-Server.sqlite3")}"));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpClient("extendedhandlerlifetime").SetHandlerLifetime(Timeout.InfiniteTimeSpan);
//add http client services
builder.Services.AddHttpClient("GrantClient")
        .SetHandlerLifetime(TimeSpan.FromMinutes(5));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
            .AddTransient<IGrantUrlTesterService, GrantUrlTesterService>()
            .AddTransient<IWebhooksRetriever, WebhooksRetriever>()
            .AddTransient<IWebhooksSender, WebhooksSender>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.Run();


