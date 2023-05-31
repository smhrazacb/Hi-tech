using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using Webhooks.API.Model;
using Webhooks.API.Services;

namespace Webhooks.API.EventBusConsumer
{
    public class CatalogItemPriceChangeConsumer : IConsumer<CatalogItemPriceChangeEvent>
    {
        private readonly ILogger<CatalogItemPriceChangeConsumer> _logger;
        private readonly IWebhooksRetriever _retriever;
        private readonly IWebhooksSender _sender;

        public CatalogItemPriceChangeConsumer(ILogger<CatalogItemPriceChangeConsumer> logger, IWebhooksRetriever retriever, IWebhooksSender sender)
        {
            _logger = logger;
            _retriever = retriever;
            _sender = sender;
        }
        public async Task Consume(ConsumeContext<CatalogItemPriceChangeEvent> context)
        {
            var subscriptions = await _retriever.GetSubscriptionsOfType(WebhookType.CatalogItemPriceChange);
            _logger.LogInformation("Received CatalogItemPriceChange event and got {SubscriptionsCount} subscriptions to process", subscriptions.Count());
            var whook = new WebhookData(WebhookType.CatalogItemPriceChange, context.Message);
            await _sender.SendAll(subscriptions, whook);
        }
    }
}
