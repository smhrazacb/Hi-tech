using EventBus.Messages.Events;
using MassTransit;
using Webhooks.API.Model;
using Webhooks.API.Services;

namespace Webhooks.API.EventBusConsumer
{
    public class OrderStatusChangedToCancelConsumer : IConsumer<OrderStatusChangedToCancelEvent>
    {
        private readonly ILogger<CatalogItemPriceChangeConsumer> _logger;
        private readonly IWebhooksRetriever _retriever;
        private readonly IWebhooksSender _sender;

        public OrderStatusChangedToCancelConsumer(ILogger<CatalogItemPriceChangeConsumer> logger, IWebhooksRetriever retriever, IWebhooksSender sender)
        {
            _logger = logger;
            _retriever = retriever;
            _sender = sender;
        }

        public async Task Consume(ConsumeContext<OrderStatusChangedToCancelEvent> context)
        {
            var subscriptions = await _retriever.GetSubscriptionsOfType(WebhookType.OrderStatus);
            _logger.LogInformation("Received OrderStatusChangedToCancel event and got {SubscriptionsCount} subscriptions to process", subscriptions.Count());
            var whook = new WebhookData(WebhookType.OrderStatus, context.Message);
            await _sender.SendAll(subscriptions, whook);
        }
    }
}
