using System;
using System.Collections.Generic;
using System.Linq;

namespace Loja.Core.Notifications
{
    public class Notificator : INotificator
    {
        private List<Notification> _notifications;

        public Notificator()
        {
            _notifications = new List<Notification>();
        }

        public List<Notification> GetNotifications()
        {
            return _notifications;
        }
        public List<Notification> GetFailNotifications()
        {
            return _notifications.FindAll(n => n.Type == NotificationType.Fail);
        }
        public bool HasFailNotification()
        {
            return GetNotifications().Any(n => n.Type == NotificationType.Fail);
        }
        public void Notify(Notification message)
        {
            _notifications.Add(message);
        }

        public void Dispose()
        {
            _notifications = new List<Notification>();
        }
    }
}