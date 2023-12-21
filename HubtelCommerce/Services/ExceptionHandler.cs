using HubtelCommerce.Dtos;
using System.Net;
using System.Text.Json;

namespace HubtelCommerce.Services
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var development = new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString());
                if (ex.InnerException != null)
                {
                    development.Details = ex.InnerException.StackTrace;
                    development.Message = ex.InnerException.Message;
                }
                var production = new ApiException(context.Response.StatusCode, "Internal Server Error, An unexpected error occurred.Please try again later");
                var response = _env.IsDevelopment() ? development : production;

                if (_env.IsProduction()) 
                {
                    _logger.LogError(response.Message +" : "+response.Details);
                }
                var option = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(response, option);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
