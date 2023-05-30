using WebhookClient.Services;
using WebHookClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<WebhookClientOptions>(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IWebhooksClient, WebhooksClient>();
//add http client services
builder.Services.AddHttpClient("GrantClient")
        .SetHandlerLifetime(TimeSpan.FromMinutes(5));

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
