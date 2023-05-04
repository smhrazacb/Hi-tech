using Catalog.API.Entities.Dtos;
using Catalog.API.Repositories.Interfaces;
using EventBus.Messages.Events;
using MassTransit;

namespace Catalog.API.EventBusConsumer
{
    public class CatalogDeleteConsumer : IConsumer<CatalogStockDelEvent>
    {
        private readonly ILogger<CatalogDeleteConsumer> _logger;
        private readonly IProductRepositoryW _repositoryW;
        private readonly IProductRepositoryR _repositoryR;

        public CatalogDeleteConsumer(ILogger<CatalogDeleteConsumer> logger, IProductRepositoryW repositoryW, IProductRepositoryR repositoryR)
        {
            _logger = logger;
            _repositoryW = repositoryW;
            _repositoryR = repositoryR;
        }

        public async Task Consume(ConsumeContext<CatalogStockDelEvent> context)
        {
            var orderStockReport = new List<OrderStockStatus>();
            // Get Products
            foreach (var item in context.Message.ShoppingItems)
            {
                var category = await _repositoryR.GetProductById(item.ProductId);

                if (category == null)
                {
                    var tmsg = $"Productid {item.ProductId} not found";
                    _logger.LogError(tmsg);
                    orderStockReport.Add(new OrderStockStatus(item, tmsg));
                    continue;
                }
                if (category.SubCategory.Product.Stock < item.Quantity)
                {
                    var tmsg = $"Productid {item.ProductId} Stock unavailable. Orderd : {item.Quantity} available : {category.SubCategory.Product.Stock}";
                    _logger.LogError(tmsg);
                    orderStockReport.Add(new OrderStockStatus(item, tmsg));
                    continue;
                }
                else
                {
                    category.SubCategory.Product.Stock = (uint)(category.SubCategory.Product.Stock - item.Quantity);
                    await _repositoryW.UpdateProduct(category);
                    var tmsg = $"Productid {item.ProductId} available stock : {category.SubCategory.Product.Stock} ({category.SubCategory.Product.Stock + item.Quantity})";
                    _logger.LogInformation(tmsg);
                }
            }
            _logger.LogInformation($"CatalogStockDelEvent consumed successfully for orderID : {context.Message.OrderId}");
        }
    }
}



