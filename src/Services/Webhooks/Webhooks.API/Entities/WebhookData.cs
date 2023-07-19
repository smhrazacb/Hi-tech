using System.Text.Json;

namespace Webhooks.API.Entities;

public class WebhookData
{
    public DateTimeOffset When { get; }

    public string Payload { get; }

    public string Type { get; }

    public WebhookData(WebhookType hookType, object data)
    {
        When = DateTimeOffset.UtcNow;
        Type = hookType.ToString();
        Payload = JsonSerializer.Serialize(data);
    }
}
