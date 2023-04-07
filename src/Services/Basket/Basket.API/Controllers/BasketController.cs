using AutoMapper;
using Basket.API.Entities;
using Basket.API.Entities.Dtos;
using Basket.API.Repositories.Interfaces;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        public BasketController(IBasketRepository repository, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }
        /// <summary>
        /// <para>Returns a requested Basket if Id existed</para> 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet("{guid}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(Guid guid)
        {
            var basket = await _repository.GetBasket(guid);
            if (basket == null)
                return NotFound();
            return Ok(basket);
        }
        /// <summary>
        /// <para>Create a new empty Basket</para>
        /// </summary>
        /// <param name="shoppingCartDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> CreateBasket([FromBody] ShoppingCartDto shoppingCartDto)
        {
            var shoppingCart = _mapper.Map<ShoppingCart>(shoppingCartDto);
            shoppingCart.ShoppingCartId = Guid.NewGuid();
            return Ok(await _repository.UpdateBasket(shoppingCart));
        }
        /// <summary>
        /// <para>Update Basket if Id existed</para>
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart shoppingCart)
        {
            var basket = await _repository.GetBasket(shoppingCart.ShoppingCartId);
            if (basket == null)
                return NotFound();
            return Ok(await _repository.UpdateBasket(shoppingCart));
        }
        /// <summary>
        /// <para>Delete a Basket if existed</para> 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpDelete("{guid}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(Guid guid)
        {
            await _repository.DeleteBasket(guid);
            return Ok();
        }
        /// <summary>
        /// Process a order and returns its detail with Id which can be used for order querry
        /// </summary>
        /// <param name="basketCheckoutIdsDto"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BasketCheckoutEvent), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<BasketCheckoutEvent>> Checkout([FromBody] BasketCheckoutIdsDto basketCheckoutIdsDto)
        {
            // Get Basket
            var basket = await _repository.GetBasket(basketCheckoutIdsDto.BasketId);
            if (basket == null)
                return NotFound();
            // Validate basket price with current product price
            // send checkout event to rabbitmq
            var basketCheckoutEvent = _mapper.Map<BasketCheckoutEvent>(basket);
            basketCheckoutEvent.TotalPrice = basketCheckoutIdsDto.TotalPrice;
            basketCheckoutEvent.UserId = basketCheckoutIdsDto.UserId;
            await _publishEndpoint.Publish<BasketCheckoutEvent>(basketCheckoutEvent);

            // remove the basket
            await _repository.DeleteBasket(basket.ShoppingCartId);

            return Ok(basketCheckoutEvent);
        }
    }
}
