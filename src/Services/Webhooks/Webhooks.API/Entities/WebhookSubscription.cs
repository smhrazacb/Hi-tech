namespace Webhooks.API.Entities;

public class WebhookSubscription
{
    public int Id { get; set; }

    public WebhookType Type { get; set; }
    public DateTimeOffset DateTimeStamp { get; set; }
    public string DestUrl { get; set; }
    public string Token { get; set; }
    public string UserId { get; set; }
}
