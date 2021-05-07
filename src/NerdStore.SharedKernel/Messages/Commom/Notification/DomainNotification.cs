using MediatR;
using System;
using System.Linq;

namespace NerdStore.SharedKernel.Messages.Commom.Notification
{
    public class DomainNotification : Message, INotification
    {
        public DateTime TimeStamp { get; private set; }
        public Guid DomainNotificatinId { get; private set; }
        public string Key { get; private set; }
        public string Value { get; private set; }
        public int Version { get; private set; }

        public DomainNotification(string key, string value)
        {
            TimeStamp = DateTime.Now;
            DomainNotificatinId = Guid.NewGuid();
            Version = 1;
            Key = key;
            Value = value;
        }
    }
}