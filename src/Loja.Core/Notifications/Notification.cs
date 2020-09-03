using System;

namespace Loja.Core.Notifications
{
    public class Notification
    {
        public Guid Id { get; private set; }
        public NotificationType Type { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public string Value { get; private set; }
        public Exception Exception { get; private set; }

        public Notification(NotificationType type, string value)
        {
            Id = Guid.NewGuid();
            Type = type;
            TimeStamp = DateTime.UtcNow;
            Value = value;
        }
        public Notification(Exception exception)
        {
            Id = Guid.NewGuid();
            Type = NotificationType.Fail;
            TimeStamp = DateTime.UtcNow;
            Value = $"Ocorreu uma exceção não tratada. Código {Id}. Mensagem: {exception.Message}";
            Exception = exception;
        }
    }
}