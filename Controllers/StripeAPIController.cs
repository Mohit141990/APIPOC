using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeAPIController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public StripeAPIController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("create-stripe-payment")]
        public async Task<IActionResult> CreatePayment([FromQuery] StripeModel model)
        {
            var secretKey = _configuration["Stripe:SecretKey"];
            StripeConfiguration.ApiKey = secretKey;

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Test Product",
                        },
                        UnitAmount = model.Amount,
                    },
                    Quantity = 1,
                },
            },
                Mode = "payment",
                SuccessUrl = "https://localhost:7187/api/Stripe/success-stripe-payment?sessionId=" + "{CHECKOUT_SESSION_ID}",
                CancelUrl = "https://localhost:7187/api/Stripe/cancel-stripe-payment",
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            return Ok(new { sessionId = session.Id, url = session.Url });

            //var options = new PaymentIntentCreateOptions
            //{
            //    Amount = model.Amount,
            //    Currency = "USD",
            //    PaymentMethodTypes = new List<string> { "card" },
            //};

            //var service = new PaymentIntentService();
            //PaymentIntent paymentIntent = await service.CreateAsync(options);

            //return Ok(new { clientSecret = paymentIntent.ClientSecret });
        }

        [HttpGet("success-stripe-payment")]
        public async Task<IActionResult> Success(string sessionId)
        {
            var secretKey = _configuration["Stripe:SecretKey"];
            StripeConfiguration.ApiKey = secretKey;

            var service = new SessionService();
            Session session = await service.GetAsync(sessionId);

            return Ok(session);
        }
    }
}
