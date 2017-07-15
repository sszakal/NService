using System;
using MediatR;
using Quartz;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using StructureMap;
using Topshelf;
using Topshelf.StructureMap;
using NService.Agent.Logging;
using NService.Agent.Logging.LogProviders;
using NService.Agent.Mediatr;
using NService.Application.CommandHandlers;
using Serilog.Core;
using TopShelf.Owin;
using WebApi.StructureMap;

namespace NService.Agent
{
    public static class Program
    {
        public class SampleJob : IJob
        {
            private readonly ILogger _logger = Log.Logger;
            private readonly IMediator _mediator;

            public SampleJob(ILogger logger, IMediator mediator)
            {
                _mediator = mediator;
                _logger = logger;
            }

            public void Execute(IJobExecutionContext context)
            {
                _logger.Information("Job {@JobName} Started", new { JobName = nameof(SampleJob) });
                _logger.Information("Job {@JobName} Completed", new { JobName = nameof(SampleJob) });
            }
        }

        private static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                //Configure Logging
                var levelSwitch = new LoggingLevelSwitch();
                levelSwitch.MinimumLevel = LogEventLevel.Debug;
                Log.Logger = SetupLogger(levelSwitch);
                LogProvider.SetCurrentLogProvider(new SerilogLogProvider());
                //Configure IoC Container

                var container = SetupStructureMap(levelSwitch);

                x.UseSerilog();
                x.UseStructureMap(container);

                x.Service<NServiceAgent>(s =>
                {
                    s.ConstructUsingStructureMap();
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());

                    s.OwinEndpoint(app =>
                    {
                        app.Domain = "localhost";
                        app.Port = 8080;
                        app.UseDependencyResolver(new DependencyResolver(container));
                        app.ConfigureAppBuilder(appBuilder => new OwinStartup().Configuration(appBuilder));
                    });

                    //s.UseQuartzStructureMap();
                    //s.ScheduleQuartzJob(q =>
                    //    q.WithJob(() => JobBuilder.Create<SampleJob>().Build())
                    //            .AddTrigger(() =>
                    //            TriggerBuilder.Create()
                    //                .WithSimpleSchedule(builder => builder
                    //                    .WithIntervalInSeconds(5)
                    //                    .RepeatForever())
                    //                .Build())
                    //);
                });
                x.RunAsLocalSystem();

                x.SetDescription("NService Agent");
                x.SetDisplayName("NService Agent");
                x.SetServiceName("NService"); 
            });
        }

        //private static Action<IAppBuilder> AppBuilderConfigurator()
        //{
        //    return appBuilder =>
        //    {
        //        appBuilder.UseCors(CorsOptions.AllowAll);
        //        HttpConfiguration config = new HttpConfiguration();
        //        config.MapHttpAttributeRoutes();
        //        config.Formatters.Clear();
        //        config.Formatters.Add(new JsonMediaTypeFormatter());

        //        var jsonSettings = config.Formatters.JsonFormatter.SerializerSettings;
        //        jsonSettings.Formatting = Formatting.Indented;
        //        jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        //        config.Routes.MapHttpRoute(
        //            name: "DefaultApi",
        //            routeTemplate: "api/{controller}/{id}",
        //            defaults: new { id = RouteParameter.Optional }
        //        );
        //        appBuilder.UseWebApi(config);

        //        var hubConfiguration = new HubConfiguration();
        //        hubConfiguration.EnableDetailedErrors = true;
        //        hubConfiguration.EnableJavaScriptProxies = true;

        //        appBuilder.MapSignalR("/signalr", hubConfiguration);

        //        var fileServerOptions = new FileServerOptions
        //        {
        //            EnableDefaultFiles = true,
        //            RequestPath = PathString.Empty,
        //            EnableDirectoryBrowsing = true,
        //            DefaultFilesOptions = { DefaultFileNames = { "index.html" },
        //                RequestPath = PathString.Empty,
        //                FileSystem = new PhysicalFileSystem("UI/") },
        //            FileSystem = new PhysicalFileSystem("UI/")
        //        };

        //        appBuilder.UseFileServer(fileServerOptions);
        //    };
        //}

        private static Container SetupStructureMap(LoggingLevelSwitch loggingLevelSwitch)
        {
            var container = new Container(cfg =>
            {
                cfg.Scan(scanner =>
                {
                    scanner.TheCallingAssembly();
                    scanner.AssemblyContainingType<ChangeLoggingLevelCommandHandler>();
                    scanner.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<>));
                    scanner.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
                    scanner.ConnectImplementationsToTypesClosing(typeof(IAsyncRequestHandler<>));
                    scanner.ConnectImplementationsToTypesClosing(typeof(IAsyncRequestHandler<,>));
                    scanner.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
                    scanner.ConnectImplementationsToTypesClosing(typeof(IAsyncNotificationHandler<>));
                });

                cfg.For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
                cfg.For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
                cfg.For(typeof(IPipelineBehavior<,>)).Add(typeof(LoggingBehavior<,>));
                cfg.For<IMediator>().Use<Mediator>();
                cfg.For<ILogger>().Use(Log.Logger).Singleton();
                cfg.For<LoggingLevelSwitch>().Use(loggingLevelSwitch).Singleton();
            });
            return container;
        }

        private static ILogger SetupLogger(LoggingLevelSwitch levelSwitch)
        {
            return new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .Enrich.WithProperty("Application", "NService")
                .Enrich.WithMachineName()
                .Enrich.WithProcessName()
                .Enrich.WithThreadId()
                .WriteTo.ColoredConsole()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                {
                    AutoRegisterTemplate = true,
                    MinimumLogEventLevel = LogEventLevel.Information,
                    CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true),
                    IndexFormat = @"logs-{0:yyyy.MM.dd}"
                })
                .MinimumLevel.Debug()
                .CreateLogger();
        }
    }
}
