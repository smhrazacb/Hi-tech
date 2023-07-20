using AutoMapper;
using EventBus.Messages.Events.Catalog;
using MassTransit;
using Webhooks.API.Entities;
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
            var subscriptions = await _retriever.GetSubscriptions(WebhookType.CatalogItemPriceChange);
            _logger.LogInformation("Received CatalogItemPriceChange event and got {SubscriptionsCount} subscriptions to process", subscriptions.Count());
            var whook = new WebhookData(WebhookType.CatalogItemPriceChange, context.Message);
            await _sender.SendAll(subscriptions, whook);
        }
    }
}
