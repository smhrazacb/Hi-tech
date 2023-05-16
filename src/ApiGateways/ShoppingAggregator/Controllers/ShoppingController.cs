using EventBus.Messages.Common;
using Microsoft.AspNetCore.Mvc;
using ShoppingAggregator.Models;
using ShoppingAggregator.Services.Interfaces;
using System.Net;

namespace ShoppingAggregator.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ShoppingController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public ShoppingController(ICatalogService catalogService, IBasketService basketService, IOrderService orderService)
        {
            _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
            _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        }

        [HttpGet("{userName}", Name = "GetShopping")]
        [ProducesResponseType(typeof(ResponseMessage<ShoppingModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ResponseMessage<ShoppingModel>>> GetShopping(string userName)
        {
            var basket = await _basketService.GetBasket(userName);
            if (basket.Succeeded)
            {
                for (int i = 0; i < basket.Data.ShoppingItems.Count(); i++)
                {
                    var basPro = basket.Data.ShoppingItems.ElementAt(i);
                    var product = await _catalogService.GetCatalog(basPro.ProductId);

                    basPro.CategoryName = product.Data.CategoryName;
                    basPro.ModifiedDate = product.Data.ModifiedDate;
                    basPro.SubCategoryName = product.Data.SubCategory.SubCategoryName;
                    basPro.Packaging = product.Data.SubCategory.Product.Packaging;
                    basPro.Name = product.Data.SubCategory.Product.Name;
                    basPro.AdditionalFields = product.Data.SubCategory.Product.AdditionalFields;
                    basket.Data.ShoppingItems.ElementAt(i).Equals(basPro);
                }
            }
            var orders = await _orderService.GetOrdersByUserName(userName);
            var shoppingModel = new ShoppingModel
            {
                UserName = userName,
                BasketWithProducts = basket.Data,
                Orders = orders
            };

            return Ok(shoppingModel);
        }
    }
}
