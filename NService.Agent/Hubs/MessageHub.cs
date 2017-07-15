using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using NService.Contract.Commands;
using NService.Contract.Interfaces;

namespace NService.Agent.Hubs
{
    public class MessageHub: Hub
    {
        private readonly IMediator _mediator;
        private Dictionary<string, Type> _commmandTypes;

        public MessageHub(IMediator mediator)
        {
            var commandInterfaceType = typeof(ICommand);
            _commmandTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => !t.IsAbstract && !t.IsInterface && commandInterfaceType.IsAssignableFrom(t))
                .ToDictionary(k => k.FullName, v => v);

            _mediator = mediator;
        }

        public Task<Guid> Handle(RemoteCommand remoteCommand)
        {
            var command = Unwrapp(remoteCommand);
            _mediator.Send(command);
            return Task.FromResult(command.CorrelationId);
        }

        private CommandBase Unwrapp(RemoteCommand remoteCommand)
        {
            if(!_commmandTypes.ContainsKey(remoteCommand.Name)) throw new ArgumentException($"Cannot find command with name: {remoteCommand.Name}");
            var commandType = _commmandTypes[remoteCommand.Name];
            return (CommandBase) JsonConvert.DeserializeObject(remoteCommand.Data, commandType);
        }
    }
}
