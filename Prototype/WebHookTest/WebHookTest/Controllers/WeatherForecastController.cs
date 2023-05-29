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
    public class WeatherForecastController : ControllerBase
    {
        private readonly WebhooksContext _dbContext;
        private readonly IGrantUrlTesterService _grantUrlTester;

        public WeatherForecastController(WebhooksContext dbContext, IGrantUrlTesterService grantUrlTester)
        {
            _dbContext = dbContext;
            _grantUrlTester = grantUrlTester;
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

            var grantOk = await _grantUrlTester.TestGrantUrl(request.Url, request.GrantUrl, request.Token ?? string.Empty);

            if (grantOk)
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