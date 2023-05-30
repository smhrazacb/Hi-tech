﻿namespace WebHookClient;

public class WebhookClientOptions
{
    public string Token { get; set; }
    public string IdentityUrl { get; set; }
    public string CallBackUrl { get; set; }
    public string WebhooksUrl { get; set; }
    public string SelfUrl { get; set; }

    public bool ValidateToken { get; set; }

}
