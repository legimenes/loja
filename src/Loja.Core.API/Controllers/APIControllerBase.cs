using System;
using System.Collections.Generic;
using System.Net;
using Loja.Core.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace Loja.Core.API.Controllers
{
    [ApiController]
    public abstract class APIControllerBase : ControllerBase
    {
        protected readonly INotificator _notifications;

        public APIControllerBase(INotificator notifications)
        {
            _notifications = notifications;
        }

        protected IActionResult APIResult(Exception exception)
        {
            Notification notification = new Notification(exception);
            _notifications.Notify(notification);

            //TODO: implementar log
            
            return APIResultError();
        }
        protected IActionResult APIResult(object result = null)
        {
            if (_notifications.HasFailNotification())
            {
                return APIResultError();
            }
            
            return Ok(result);
        }
        protected IActionResult APIResultFail(HttpStatusCode statusCode, object result = null)
        {
            return APIResultError(statusCode);
        }
        protected IActionResult APIResultSuccess(HttpStatusCode statusCode, object result = null)
        {
            return StatusCode((int)statusCode, result);
        }

        private IActionResult APIResultError(HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            List<string> messages = new List<string>();

            List<Notification> failNotifications = _notifications.GetFailNotifications();
            foreach (Notification fail in failNotifications)
            {
                messages.Add(fail.Value);
            }

            return StatusCode((int)statusCode, messages);
        }
    }
}