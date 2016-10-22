using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace FaaS.MVC.Middleware
{
    public class GuardianMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GuardianMiddleware> _logger;

        public GuardianMiddleware(RequestDelegate next, ILogger<GuardianMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation($"Request is OK - {context.Request.GetDisplayUrl()}\n");
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.Clear();
                await context.Response.WriteAsync($"Error occured - {ex.Message}\n");
            }
        }
    }
}