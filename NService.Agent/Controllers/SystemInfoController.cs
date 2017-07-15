using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace NService.Agent.Controllers
{
    public class SystemInfoController: ApiController
    {
        public Task<string> GetCurrentDateTime()
        {
            return Task.FromResult(DateTime.Now.ToLongDateString());
        }
    }
}
