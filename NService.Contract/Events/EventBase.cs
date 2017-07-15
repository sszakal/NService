using System;
using NService.Contract.Interfaces;
using TypeLite;

namespace NService.Contract.Events
{
    [TsIgnore]
    public abstract class EventBase: IEvent
    {
        public Guid CorrelationId { get; }

        public EventBase(): this(Guid.NewGuid())
        {
        }

        public EventBase(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
    }
}
