using System.Text;

namespace INFINITE.CORE.API.Handler
{
    public class AuditLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public AuditLogMiddleware(RequestDelegate next, ILogger<AuditLogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var requestBody = await GetRequestBody(context.Request);
                _logger.LogInformation($"Http Request Information: {Environment.NewLine}" +
                                   $"Schema:{context.Request.Scheme} " +
                                   $"Host: {context.Request.Host} " +
                                   $"Method: {context.Request.Method} " +
                                   $"Path: {context.Request.Path} " +
                                   $"QueryString: {context.Request.QueryString} " +
                                   $"Request Body: {requestBody}");
                
                Stream originalBody = context.Response.Body;

                try
                {
                    using (var memStream = new MemoryStream())
                    {
                        context.Response.Body = memStream;

                        await _next(context);

                        memStream.Position = 0;
                        string responseBody = new StreamReader(memStream).ReadToEnd();
                        _logger.LogInformation($"Response: {context.Request.Path} - {context.Response.StatusCode}");
                        _logger.LogInformation($"Response Body: {responseBody}");
                        memStream.Position = 0;
                        await memStream.CopyToAsync(originalBody);
                    }

                }
                finally
                {
                    context.Response.Body = originalBody;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Middleware API Error");
            }
        }

        private async Task<string> GetRequestBody(HttpRequest request)
        {
            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var requestBody = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin);
            return requestBody;
        }
    }
}
