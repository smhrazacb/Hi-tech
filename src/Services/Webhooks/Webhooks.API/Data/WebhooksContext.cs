using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using Webhooks.API.Model;

namespace Webhooks.API.Data;

public class WebhooksContext : DbContext
{

    public WebhooksContext(DbContextOptions<WebhooksContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    public DbSet<WebhookSubscription> Subscriptions { get; set; }
}

