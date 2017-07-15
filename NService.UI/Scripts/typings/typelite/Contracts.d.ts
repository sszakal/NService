
 
 
 

 

/// <reference path="Enums.ts" />

declare namespace NService.Contract.Commands {
	interface ChangeLoggingLevelCommand extends NService.Contract.Commands.CommandBase {
		MinimumLevel: Serilog.Events.LogEventLevel;
	}
	interface CommandBase {
		CorrelationId: System.Guid;
	}
	interface CreateUserCommand extends NService.Contract.Commands.CommandBase {
		EMail: string;
		FirstName: string;
		LastName: string;
		MiddleName: string;
		Password: string;
	}
	interface RemoteCommand {
		Data: string;
		Name: string;
	}
}
declare namespace NService.Contract.Queries.LoggingLevelQuery {
	interface LoggingLevelQueryResponse {
		Id: number;
		Level: string;
	}
}
declare namespace System {
	interface Guid {
	}
}




