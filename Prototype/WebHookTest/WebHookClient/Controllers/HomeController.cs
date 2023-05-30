using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebhookClient;
using WebhookClient.Models;
using WebHookClient.Models;

namespace WebHookClient.Controllers
{
    [ApiController]
    [Route("webhook-received")]
    public class HomeController : ControllerBase
    {
        private readonly WebhookClientOptions _options;
        private readonly ILogger _logger;

        public HomeController(IOptions<WebhookClientOptions> options, ILogger<HomeController> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> NewWebhook(WebhookData hook)
        {
            string token = Request.Headers[HeaderNames.WebHookCheckHeader];
            _options.Token = "aaa";
            _logger.LogInformation("Received hook with token {Token}. My token is {MyToken}. Token validation is set to {ValidateToken}", token, _options.Token, _options.ValidateToken);

            if (!_options.ValidateToken || _options.Token == token)
            {
                _logger.LogInformation("Received hook is going to be processed");
                var newHook = new WebHookReceived()
                {
                    Data = hook.Payload,
                    When = hook.When,
                    Token = token
                };
                _logger.LogInformation("Received hook was processed.");
                return Ok(newHook);
            }

            _logger.LogInformation("Received hook is NOT processed - Bad Request returned.");
            return BadRequest();
        }
    }
}
