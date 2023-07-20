using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using Webhooks.API.Entities;

namespace Webhooks.API.Data;

public class WebhooksContext : DbContext
{

    public WebhooksContext(DbContextOptions<WebhooksContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

    }
    public DbSet<WebhookSubscription> WebhookSubscription { get; set; }
}

