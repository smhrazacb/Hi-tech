using MassTransit;
using WebHookTest.Model;
using WebHookTest.Services;

namespace WebHookTest.Consumers
{
    public class OrderCompleteConsumer : IConsumer<CatalogStockDelEvent>
    {
        private readonly ILogger<OrderCompleteConsumer> _logger;
        private readonly IWebhooksRetriever _retriever;
        private readonly IWebhooksSender _sender;

        public OrderCompleteConsumer(ILogger<OrderCompleteConsumer> logger, IWebhooksRetriever retriever, IWebhooksSender sender)
        {
            _logger = logger;
            _retriever = retriever;
            _sender = sender;
        }
        public async Task Consume(ConsumeContext<CatalogStockDelEvent> context)
        {
            var subscriptions = await _retriever.GetSubscriptionsOfType(WebhookType.OrderShipped);
            _logger.LogInformation("Received OrderCompleteConsumer and got {SubscriptionCount} subscriptions to process", subscriptions.Count());
            var whook = new WebhookData(WebhookType.OrderShipped, context);
            await _sender.SendAll(subscriptions, whook);

            _logger.LogInformation($"CatalogStockDelEvent consumed successfully for orderID : {context.Message.OrderId}");
        }
    }
}