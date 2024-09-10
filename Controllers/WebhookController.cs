using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI5.Models;

namespace WebAPI5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] WebhookPayload payload)
        {
            if (payload == null)
            {
                return BadRequest("Invalid payload");
            }

            // Process the webhook payload (e.g., log it, trigger other actions, etc.)
            // For example:
            // await _webhookService.ProcessPayload(payload);

            return Ok("Webhook received successfully");
        }
    }
}
