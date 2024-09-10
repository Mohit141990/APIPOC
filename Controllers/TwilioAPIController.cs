using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI5.Services;

namespace WebAPI5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwilioAPIController : ControllerBase
    {
        private readonly IRestAPIServices _restAPIService;

        public TwilioAPIController(IRestAPIServices restAPIService)
        {
            _restAPIService = restAPIService;
        }

        [HttpPost("Send-SMS")]
        public async Task<IActionResult> SendSms([FromQuery] string to, [FromBody] string message)
        {
            try
            {
                await _restAPIService.SendSmsAsync(to, message);
                return Ok("SMS sent successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Make-Call")]
        public async Task<IActionResult> MakeCall([FromQuery] string to)
        {
            try
            {
                string url = "http://demo.twilio.com/docs/voice.xml";
                await _restAPIService.MakeCallAsync(to, url);
                return Ok("Call initiated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
