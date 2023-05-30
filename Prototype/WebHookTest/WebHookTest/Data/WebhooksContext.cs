using Microsoft.EntityFrameworkCore;
using WebHookTest.Model;

namespace WebHookTest.Data;

public class WebhooksContext : DbContext
{

    public WebhooksContext(DbContextOptions<WebhooksContext> options) : base(options)
    {
    }
    public DbSet<WebhookSubscription> Subscriptions { get; set; }
}

