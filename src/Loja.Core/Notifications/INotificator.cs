using System;
using System.Collections.Generic;

namespace Loja.Core.Notifications
{
    public interface INotificator : IDisposable
    {
        List<Notification> GetNotifications();
        List<Notification> GetFailNotifications();
        bool HasFailNotification();
        void Notify(Notification message);
    }
}