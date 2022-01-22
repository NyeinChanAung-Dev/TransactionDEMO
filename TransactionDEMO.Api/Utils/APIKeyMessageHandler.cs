using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionDEMO.Api.Utils
{
    public class APIKeyMessageHandler
    {
        private readonly RequestDelegate _next;
        public const string APIKeyToCheck = "G93979A51C8C46712DD2C8271587B262";
        public APIKeyMessageHandler(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.Keys.Contains("APIKey"))
            {
                context.Response.StatusCode = 400; //Bad Request                
                await context.Response.WriteAsync("API Key is missing");
                return;
            }
            {
                if (context.Request.Headers["APIKey"] != APIKeyToCheck)
                {
                    await context.Response.WriteAsync("Invalid API Key");
                    return;
                }
            }
            await _next.Invoke(context);
            return;
        }

    }

    #region ExtensionMethod
    public static class APIKeyValidatorsExtension
    {
        public static IApplicationBuilder ApplyAPIKeyValidation(this IApplicationBuilder app)
        {
            app.UseMiddleware<APIKeyMessageHandler>();
            return app;
        }
    }

    #endregion
}
