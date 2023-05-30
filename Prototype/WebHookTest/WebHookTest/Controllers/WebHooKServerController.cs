using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebHookTest.Data;
using WebHookTest.Model;
using WebHookTest.Services;

namespace WebHookTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebHooKServerController : ControllerBase
    {
        private readonly WebhooksContext _dbContext;
        private readonly IGrantUrlTesterService _grantUrlTester;
        private readonly IWebhooksRetriever _retriever;
        private readonly IWebhooksSender _sender;

        public WebHooKServerController(WebhooksContext dbContext, IGrantUrlTesterService grantUrlTester, IWebhooksRetriever retriever, IWebhooksSender sender)
        {
            _dbContext = dbContext;
            _grantUrlTester = grantUrlTester;
            _retriever = retriever;
            _sender = sender;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GenerateTestEvent()
        {
            var subscriptions = await _retriever.GetSubscriptionsOfType(WebhookType.OrderPaid);
            var whook = new WebhookData(WebhookType.OrderPaid, "hook Received");
            await _sender.SendAll(subscriptions, whook);
            return Ok();
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
    }
}