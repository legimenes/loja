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

        protected IActionResult APIResult(HttpStatusCode statusCode = HttpStatusCode.OK, object result = null)
        {
            if (_notifications.HasFailNotification())
            {
                return APIResultError();
            }
            
            return Ok(result);
        }
        protected IActionResult APIResult(Exception exception)
        {
            Notification notification = new Notification(exception);
            _notifications.Notify(notification);

            //TODO: implementar log
            
            return APIResultError();
        }

        private IActionResult APIResultError()
        {
            List<string> messages = new List<string>();

            List<Notification> failNotifications = _notifications.GetFailNotifications();
            foreach (Notification fail in failNotifications)
            {
                messages.Add(fail.Value);
            }
            
            return BadRequest(messages);
        }
    }
}