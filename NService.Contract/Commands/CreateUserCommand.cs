using Serilog.Events;
using TypeLite;

namespace NService.Contract.Commands
{
    [TsClass]
    public class CreateUserCommand: CommandBase
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string EMail { get; set; }
        public string Password { get; set; }
    }
}
