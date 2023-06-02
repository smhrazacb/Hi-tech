using EventBus.Messages.Events.Order;
using MassTransit;
using Webhooks.API.Entities;
using Webhooks.API.Services;

namespace Webhooks.API.EventBusConsumer
{
    public class OrderStatusChangedConsumer : IConsumer<OrderStatusChangedEvent>
    {
        private readonly ILogger<OrderStatusChangedConsumer> _logger;
        private readonly IWebhooksRetriever _retriever;
        private readonly IWebhooksSender _sender;

        public OrderStatusChangedConsumer(ILogger<OrderStatusChangedConsumer> logger, IWebhooksRetriever retriever, IWebhooksSender sender)
        {
            _logger = logger;
            _retriever = retriever;
            _sender = sender;
        }

        public async Task Consume(ConsumeContext<OrderStatusChangedEvent> context)
        {
            var subscriptions = await _retriever.GetSubscriptions(WebhookType.OrderStatus, context.Message.UserId);
            _logger.LogInformation("Received OrderStatusChangeEvent and got {SubscriptionsCount} subscriptions to process", subscriptions.Count());
            var whook = new WebhookData(WebhookType.OrderStatus, context.Message);
            await _sender.SendAll(subscriptions, whook);
        }
    }
}
