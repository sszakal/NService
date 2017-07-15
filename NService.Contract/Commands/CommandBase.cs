using System;
using NService.Contract.Interfaces;
using TypeLite;

namespace NService.Contract.Commands
{
    [TsClass]
    public abstract class CommandBase: ICommand
    {
        [TsProperty]
        public Guid CorrelationId { get; }

        public CommandBase(): this(Guid.NewGuid())
        {
        }

        public CommandBase(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
    }
}
