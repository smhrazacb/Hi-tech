using EventBus.Messages.Events;
using MassTransit;
using Webhooks.API.Model;
using Webhooks.API.Services;

namespace Webhooks.API.EventBusConsumer
{
    public class OrderStatusConsumer : IConsumer<OrderStatusChangeEvent>
    {
        private readonly ILogger<CatalogItemPriceChangeConsumer> _logger;
        private readonly IWebhooksRetriever _retriever;
        private readonly IWebhooksSender _sender;

        public OrderStatusConsumer(ILogger<CatalogItemPriceChangeConsumer> logger, IWebhooksRetriever retriever, IWebhooksSender sender)
        {
            _logger = logger;
            _retriever = retriever;
            _sender = sender;
        }

        public async Task Consume(ConsumeContext<OrderStatusChangeEvent> context)
        {
            var subscriptions = await _retriever.GetSubscriptionsOfType(WebhookType.OrderStatus);
            _logger.LogInformation("Received OrderStatus event and got {SubscriptionsCount} subscriptions to process", subscriptions.Count());
            var whook = new WebhookData(WebhookType.OrderStatus, context.Message);
            await _sender.SendAll(subscriptions, whook);
        }
    }
}
