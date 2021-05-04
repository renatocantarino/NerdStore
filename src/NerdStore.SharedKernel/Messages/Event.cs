using MediatR;
using System;

namespace NerdStore.SharedKernel.Messages
{
    /// <summary>
    /// todo evento e uma mensagem que entrega uma notificação para alguem processar
    /// </summary>
    public abstract class Event : Message, INotification
    {
        public DateTime TimeStamp { get; private set; }

        public Event()
        {
            TimeStamp = DateTime.Now;
        }
    }
}