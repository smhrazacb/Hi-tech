using WebHookTest.Model;

namespace WebHookTest.Services;

public interface IWebhooksRetriever
{

    Task<IEnumerable<WebhookSubscription>> GetSubscriptionsOfType(WebhookType type);
}
