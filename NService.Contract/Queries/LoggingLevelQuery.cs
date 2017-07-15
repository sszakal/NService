using TypeLite;

namespace NService.Contract.Queries
{
    [TsClass]
    public class LoggingLevelQuery: QueryBase<LoggingLevelQuery.LoggingLevelQueryResponse>
    {
        public class LoggingLevelQueryResponse
        {
            public int Id { get; }
            public string Level { get; }

            public LoggingLevelQueryResponse(int id, string level)
            {
                Id = id;
                Level = level;
            }
        }
    }
}
