using EventBus.Messages.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using System.Net;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class OrderingController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrderingController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        /// <summary>
        /// Returns Orders by UserName
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet("[action]/{userName}", Name = "Orders")]
        [ProducesResponseType(typeof(IEnumerable<OrderQueryModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ResponseMessage<IEnumerable<OrderQueryModel>>>> Orders(string userName)
        {
            var query = new GetOrdersListQuery(userName);
            var orders = await _mediator.Send(query);
            if (orders.Count() == 0)
                return new ResponseMessage<IEnumerable<OrderQueryModel>>(HttpStatusCode.NotFound.ToString());
            return new ResponseMessage<IEnumerable<OrderQueryModel>>(orders);
        }
        /// <summary>
        /// Returns Order by OrderId
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        [HttpGet("[action]/{orderid}", Name = "Order")]
        [ProducesResponseType(typeof(OrderQueryModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ResponseMessage<OrderQueryModel>>> Order(int orderid)
        {
            var query = new GetOrder(orderid);
            var order = await _mediator.Send(query);
            if (order == null)
            {
                return new ResponseMessage<OrderQueryModel>(HttpStatusCode.NotFound.ToString());
            }
            return new ResponseMessage<OrderQueryModel>(order);
        }

        /// <summary>
        /// Update Order
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
        /// <summary>
        /// Detele Order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("[action]/{id}", Name = "DeleteOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteOrder(int orderid)
        {
            var command = new DeleteOrderCommand() { OrderId = orderid };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
