using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Webhooks.API.Data;
using Webhooks.API.Model;
using Webhooks.API.Services;

namespace Webhooks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebHooKsController : ControllerBase
    {
        private readonly WebhooksContext _dbContext;
        private readonly IWebhooksRetriever _retriever;
        private readonly IWebhooksSender _sender;
        private readonly IIdentityService _IdentityService;

        public WebHooKsController(WebhooksContext dbContext, IGrantUrlTesterService grantUrlTester, IWebhooksRetriever retriever, IWebhooksSender sender)
        {
            _dbContext = dbContext;
            _retriever = retriever;
            _sender = sender;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(WebhookSubscription), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetByUserAndId(int id)
        {
            var userId = "testuser";
            var subscription = await _dbContext.Subscriptions.SingleOrDefaultAsync(s => s.Id == id && s.UserId == userId);
            if (subscription != null)
            {
                return Ok(subscription);
            }
            return NotFound($"Subscriptions {id} not found");
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(418)]
        public async Task<IActionResult> SubscribeWebhook(WebhookSubscriptionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            //var grantOk = await _grantUrlTester.TestGrantUrl(request.Url, request.GrantUrl, request.Token ?? string.Empty);

            if (true)
            {
                var subscription = new WebhookSubscription()
                {
                    Date = DateTime.UtcNow,
                    DestUrl = request.Url,
                    Token = request.Token,
                    Type = Enum.Parse<WebhookType>(request.Event, ignoreCase: true),
                    UserId = "abc"//_identityService.GetUserIdentity()
                };

                _dbContext.Add(subscription);
                await _dbContext.SaveChangesAsync();
                return CreatedAtAction("GetByUserAndId", new { id = subscription.Id }, subscription);
            }
            else
            {
                return StatusCode(418, "Grant url can't be validated");
            }
        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UnsubscribeWebhook(int id)
        {
            var userId = _IdentityService.GetUserIdentity();
            var subscription = await _dbContext.Subscriptions.SingleOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (subscription != null)
            {
                _dbContext.Remove(subscription);
                await _dbContext.SaveChangesAsync();
                return Accepted();
            }

            return NotFound($"Subscriptions {id} not found");
        }
    }
}