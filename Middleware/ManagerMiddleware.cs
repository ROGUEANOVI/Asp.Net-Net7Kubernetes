using System.Net;
using Newtonsoft.Json;

namespace Net7Kubernetes.Middleware
{
    public class ManagerMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<ManagerMiddleware> _logger;

        public ManagerMiddleware(RequestDelegate next, ILogger<ManagerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext content)
        {
            try
            {
                await _next(content);
            }
            catch (Exception ex)
            {
                await ManagerExceptionAsync(content, ex, _logger);
            }
        }

        private async Task ManagerExceptionAsync(HttpContext context, Exception ex, ILogger<ManagerMiddleware> logger)
        {
            object? errors = null;

            switch (ex)
            {
                case MiddlewareException me:
                    logger.LogError(ex, "Middleware Error");
                    errors = me.Errors;
                    context.Response.StatusCode = (int)me.Code;
                    break;

                case Exception e:
                    logger.LogError(ex, "Error Server");
                    errors = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
                
            }

            context.Response.ContentType = "application/json";
            
            var results = string.Empty;
            if(errors != null){
                results = JsonConvert.SerializeObject(new {errors});
            }

            await context.Response.WriteAsync(results);
        }
    }
}