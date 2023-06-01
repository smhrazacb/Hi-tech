using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webhooks.API.Controllers;

namespace TestData
{
    public static class WebhookData
    {
        public static WebhookSubscriptionRequest GetWebhookSubscriptionRequestData(string url, string eventt)
        {
            return new WebhookSubscriptionRequest()
            {
                Url = url,
                Token = "abc",
                Event = eventt,
                GrantUrl = url,
            };
        }

    }
}
