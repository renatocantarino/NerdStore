using System;

namespace NerdStore.SharedKernel.DomainEvents
{
    public class Event : Messages.Event
    {
        public Event(Guid aggregateId)
        {
            this.AggregateId = aggregateId;
        }
    }
}