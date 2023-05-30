using Microsoft.EntityFrameworkCore;
using Webhooks.API.Model;

namespace Webhooks.API.Data;

public class WebhooksContext : DbContext
{

    public WebhooksContext(DbContextOptions<WebhooksContext> options) : base(options)
    {
    }
    public DbSet<WebhookSubscription> Subscriptions { get; set; }
}

