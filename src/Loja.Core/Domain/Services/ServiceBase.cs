using System;
using System.Collections.Generic;
using FluentValidation.Results;
using Loja.Core.Notifications;

namespace Loja.Core.Domain.Services
{
    public abstract class ServiceBase
    {
        private readonly INotificator _notifications;

        protected ServiceBase(INotificator notifications)
        {
            _notifications = notifications;
        }

        protected void Notify(Exception exception)
        {
            _notifications.Notify(new Notification(exception));
        }
        protected void Notify(string notification)
        {
            _notifications.Notify(new Notification(NotificationType.Fail, notification));
        }
        protected void Notify(List<string> notifications)
        {
            foreach (var notification in notifications)
            {
                _notifications.Notify(new Notification(NotificationType.Fail, notification));
            }
        }
        protected void Notify(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                _notifications.Notify(new Notification(NotificationType.Fail, error.ErrorMessage));
            }
        }
    }
}