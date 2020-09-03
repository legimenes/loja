using System;
using System.Net;
using System.Threading.Tasks;
using Loja.Core.Notifications;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Loja.Core.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                await HandleRequestException(httpContext, exception);
            }
        }

        private async Task HandleRequestException(HttpContext context, Exception exception)
        {
            Notification notification = new Notification(exception);
            string[] messages = new string[1];
            messages[0] = notification.Value;

            //TODO: implementar log

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(messages));
        }
    }
}