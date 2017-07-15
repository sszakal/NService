/// <reference path="Contracts.d.ts" />
var NService;
(function (NService) {
    var Contract;
    (function (Contract) {
        var Commands;
        (function (Commands) {
            var RemoteCommand = (function () {
                function RemoteCommand() {
                }
                return RemoteCommand;
            }());
            var MessageTypes = (function () {
                function MessageTypes() {
                }
                ;
                return MessageTypes;
            }());
            MessageTypes.CreateUserCommand = "NService.Contract.Commands.CreateUserCommand";
            MessageTypes.ChangeLoggingLevelCommand = "NService.Contract.Commands.ChangeLoggingLevelCommand";
            Commands.MessageTypes = MessageTypes;
        })(Commands = Contract.Commands || (Contract.Commands = {}));
    })(Contract = NService.Contract || (NService.Contract = {}));
})(NService || (NService = {}));
//# sourceMappingURL=CommandTypes.js.map