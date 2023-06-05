using Microsoft.EntityFrameworkCore;
using Webhooks.API.Data;
using Webhooks.API.Entities;

namespace Webhooks.API.Services;

public class WebhooksRetriever : IWebhooksRetriever
{
    private readonly WebhooksContext _db;
    public WebhooksRetriever(WebhooksContext db)
    {
        _db = db;
    }
    public async Task<IEnumerable<WebhookSubscription>> GetSubscriptions(WebhookType type)
    {
        var data = await _db.WebhookSubscription.Where(s => s.Type == type).ToListAsync();
        return data;
    }
    public async Task<IEnumerable<WebhookSubscription>> GetSubscriptions(WebhookType type, string userid)
    {
        var data = await _db.WebhookSubscription
            .Where(s => s.Type == type)
            .Where(s => s.UserId == userid)
            .ToListAsync();
        return data;
    }
}
