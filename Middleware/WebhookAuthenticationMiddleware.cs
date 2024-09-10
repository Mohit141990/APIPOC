namespace WebAPI5.Middleware
{
    public class WebhookAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public WebhookAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api/webhook"))
            {
                if (!context.Request.Headers.TryGetValue("X-Webhook-Secret", out var secret) || secret != "your-secret-token")
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return;
                }
            }

            await _next(context);
        }
    }
}
