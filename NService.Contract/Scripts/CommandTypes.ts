 


/// <reference path="Contracts.d.ts" />

namespace NService.Contract.Commands {
	type CommandType = "NService.Contract.Commands.CreateUserCommand"
					 | "NService.Contract.Commands.ChangeLoggingLevelCommand";

	class RemoteCommand {
		Name: CommandType;
		Data: CommandBase;
	}

	export class MessageTypes {
		public static CreateUserCommand : CommandType = "NService.Contract.Commands.CreateUserCommand";
		public static ChangeLoggingLevelCommand : CommandType = "NService.Contract.Commands.ChangeLoggingLevelCommand";
	}
}