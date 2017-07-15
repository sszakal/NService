using System.Net.Http.Formatting;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;

[assembly: OwinStartup(typeof(NService.Agent.OwinStartup))]

namespace NService.Agent
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.UseCors(CorsOptions.AllowAll);
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());

            var jsonSettings = config.Formatters.JsonFormatter.SerializerSettings;
            jsonSettings.Formatting = Formatting.Indented;
            jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            appBuilder.UseWebApi(config);

            var hubConfiguration = new HubConfiguration();
            hubConfiguration.EnableDetailedErrors = true;
            hubConfiguration.EnableJavaScriptProxies = true;

            appBuilder.MapSignalR("/signalr", hubConfiguration);

            var fileServerOptions = new FileServerOptions
            {
                EnableDefaultFiles = true,
                RequestPath = PathString.Empty,
                EnableDirectoryBrowsing = true,
                DefaultFilesOptions = { DefaultFileNames = { "index.html" },
                    RequestPath = PathString.Empty,
                    FileSystem = new PhysicalFileSystem("UI/") },
                FileSystem = new PhysicalFileSystem("UI/")
            };

            appBuilder.UseFileServer(fileServerOptions);
        }
    }
}
