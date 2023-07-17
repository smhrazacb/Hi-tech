namespace EventBus.Messages.Common
{
    public static class EventBusConstants
    {
        public const string BasketCheckoutQueue = "basketcheckout-queue";
        public const string BasketDeleteQueue = "basketdelete-queue";
        public const string CatalogStockDelQueue = "catalogstockdel-queue";
        public const string CatalogItemPriceChangeQueue = "catalogitempricechange-queue";
        public const string CatalogStockUpdatedQueue = "catalogstockupdated-queue";
        public const string OrderInitiatedQueue = "orderinitiated-queue";
        public const string OrderConfirmQueue = "orderconfirm-queue";
        public const string OrderStatusChangedToCancelQueue = "orderstatuschangetocancel-queue";
        public const string OrderStatus = "orderstatus-queue";

    }
}
