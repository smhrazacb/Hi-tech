using Webhooks.API.Entities;

namespace Webhooks.API.Services;

public interface IWebhooksRetriever
{

    Task<IEnumerable<WebhookSubscription>> GetSubscriptions(WebhookType type);
    Task<IEnumerable<WebhookSubscription>> GetSubscriptions(WebhookType type, string userid);
}
