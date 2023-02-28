using AutoMapper;
using Basket.API.Entities;
using Basket.API.Entities.Dtos;
using Basket.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static StackExchange.Redis.Role;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly IMapper _mapper;
        public BasketController(IBasketRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        /// <summary>
        /// <para>Returns a new empty Basket</para> 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket()
        {
            return Ok(new ShoppingCart());
        }
        /// <summary>
        /// <para>Returns a requested Basket if Id existed</para> 
        /// <para>Returns a new empty Basket if id doesn't existed</para> 
        /// <para>Returns a new empty Basket if Id is null</para> 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string id)
        {
            var basket = await _repository.GetBasket(id);
            return Ok(basket ?? new ShoppingCart());
        }
        /// <summary>
        /// <para>Update Basket if Id existed</para>
        /// <para>Create a new with requested items Basket if Id not existed</para> 
        /// </summary>
        /// <param name="shoppingCartDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCartDto shoppingCartDto)
        {
            var shoppingCart = _mapper.Map<ShoppingCart>(shoppingCartDto);
            return Ok(await _repository.UpdateBasket(shoppingCart));
        }
        /// <summary>
        /// <para>Delete a Basket if existed</para> 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string id)
        {
            await _repository.DeleteBasket(id);
            return Ok();
        }
    }
}
