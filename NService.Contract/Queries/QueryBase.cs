using System;
using NService.Contract.Interfaces;
using TypeLite;

namespace NService.Contract.Queries
{
    [TsIgnore]
    public abstract class QueryBase<TResponse> : IQuery<TResponse>
    {
        public Guid CorrelationId { get; }

        public QueryBase(): this(Guid.NewGuid())
        {
        }

        public QueryBase(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
    }
}
