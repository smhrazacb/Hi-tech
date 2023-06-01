namespace Webhooks.API.Model;

public enum WebhookType
{
    CatalogItemPriceChange = 1,
    OrderStatus = 2,
    StockDeducted = 3,
    BasketDeleted = 4,
}
