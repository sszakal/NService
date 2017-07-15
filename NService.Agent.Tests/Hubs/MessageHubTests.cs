using System.Threading.Tasks;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using NService.Agent.Hubs;
using NService.Application.Interfaces;
using NService.Contract.Commands;
using Serilog.Events;
using StructureMap;

namespace NService.Agent.Tests.Hubs
{
    [TestClass]
    public class MessageHubTests
    {
        private MessageHub _messageHub;
        private IContainer _container;
        private IMediator _mediator;
        private Mock<ICommandHandler<ChangeLoggingLevelCommand>> _commandHandlerMock;

        private static IContainer SetupStructureMap(ICommandHandler<ChangeLoggingLevelCommand> commandHandler)
        {
            var container = new Container(cfg =>
            {
                cfg.Scan(scanner =>
                {
                    scanner.TheCallingAssembly();
                    scanner.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<>));
                    scanner.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
                    scanner.ConnectImplementationsToTypesClosing(typeof(IAsyncRequestHandler<>));
                    scanner.ConnectImplementationsToTypesClosing(typeof(IAsyncRequestHandler<,>));
                    scanner.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
                    scanner.ConnectImplementationsToTypesClosing(typeof(IAsyncNotificationHandler<>));
                });

                cfg.For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
                cfg.For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
                cfg.For<ICommandHandler<ChangeLoggingLevelCommand>>().Use(commandHandler);
                cfg.For<IMediator>().Use<Mediator>();
            });
            return container;
        }

        [TestInitialize]
        public void Setup()
        {
            _commandHandlerMock = new Mock<ICommandHandler<ChangeLoggingLevelCommand>>();
            _container = SetupStructureMap(_commandHandlerMock.Object);
            _mediator = _container.GetInstance<IMediator>();
            _messageHub = new MessageHub(_mediator);
        }

        [TestMethod]
        public async Task ShouldHandleCommand()
        {
            var command = new ChangeLoggingLevelCommand
            {
                MinimumLevel = LogEventLevel.Error
            };

            var remoteCommand = new RemoteCommand
            {
                Name = typeof(ChangeLoggingLevelCommand).FullName,
                Data = JsonConvert.SerializeObject(command)
            };

            ChangeLoggingLevelCommand capturedCommand = null;
            _commandHandlerMock.Setup(m => m.Handle(It.IsAny<ChangeLoggingLevelCommand>())).Returns(Task.CompletedTask)
                .Callback<ChangeLoggingLevelCommand>(c => capturedCommand = c);

            var guid = await _messageHub.Handle(remoteCommand);

            _commandHandlerMock.Verify(m => m.Handle(It.IsAny<ChangeLoggingLevelCommand>()), Times.Once);
            Assert.IsNotNull(capturedCommand);
            Assert.AreEqual(LogEventLevel.Error, capturedCommand.MinimumLevel);
            Assert.IsNotNull(guid);
        }
    }
}
