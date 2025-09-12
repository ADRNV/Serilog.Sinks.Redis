using Seriog.Sink.Redis.Core.Options;
using Seriog.Sink.Redis.Core.Store;
using StackExchange.Redis;

namespace Serilog.Sink.Redis.Options
{
    public class RedisKeyValueOptions : IRedisKeyValueOptions
    {
        public bool HasDefaultKeyValues { get; private set; }

        public IKeyGenerator KeyGenerator { get; }

        public IReadOnlyDictionary<string, RedisKeyValueWriterBase<HashEntry>> WriteKeyValues { get; private set; }

        internal IDictionary<string, RedisKeyValueWriterBase<HashEntry>> DraftKeyValues { get; private set; }

        public RedisKeyValueOptions(bool hasDefaultKeyValues, IReadOnlyDictionary<string, RedisKeyValueWriterBase<HashEntry>>  writeKeyValues, IKeyGenerator keyGenerator)
        {
            HasDefaultKeyValues = hasDefaultKeyValues;
            
            if (HasDefaultKeyValues)
            {
                WriteKeyValues = new Dictionary<string, RedisKeyValueWriterBase<HashEntry>>
                {
                    { RedisDefaultKeys.RENDERED_MESSSAGE, new RedisRenderedMessageWriter() },
                    { RedisDefaultKeys.MESSAGE_TEMPLATE, new RedisRenderedMessageWriter() },
                    { RedisDefaultKeys.LEVEL, new RedisLevelWriter() },
                    { RedisDefaultKeys.TIMESTAMP, new RedisTimeStampWriter() },
                    { RedisDefaultKeys.EXCEPTION, new RedisExceptionWriter() }
                };
            }
            if(WriteKeyValues.Count == 0)
            {
                throw new InvalidOperationException("Options cant without props");
            }

            KeyGenerator = keyGenerator;
        }
    }
    
    public static class RedisDefaultKeys
    {
        public const string RENDERED_MESSSAGE = "message";

        public const string MESSAGE_TEMPLATE = "message_template";

        public const string LEVEL = "level";

        public const string TIMESTAMP = "timestamp";

        public const string EXCEPTION = "exception";

        public const string LOG_EVENT_SERIALIZED = "log_event";
    }
}
