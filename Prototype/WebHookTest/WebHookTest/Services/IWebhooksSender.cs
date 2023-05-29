using WebHookTest.Model;

namespace WebHookTest.Services;

public interface IWebhooksSender
{
    Task SendAll(IEnumerable<WebhookSubscription> receivers, WebhookData data);
}
