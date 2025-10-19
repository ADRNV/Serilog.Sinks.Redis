using Serilog.Events;
using StackExchange.Redis;

namespace Seriog.Sink.Redis.Core.Store
{
    public abstract class RedisKeyValueWriterBase<T>
    {
        public abstract T PrepareValue(LogEvent log, IFormatProvider? format = null);

    }

    public class RedisTimeStampWriter : RedisKeyValueWriterBase<HashEntry>
    {
        public override HashEntry PrepareValue(LogEvent log, IFormatProvider? format = null)
        {
            return new HashEntry(RedisDefaultKeys.Timestamp, log.Timestamp.DateTime.ToString(format));
        }
    }

    public class RedisLevelWriter : RedisKeyValueWriterBase<HashEntry>
    {
        public override HashEntry PrepareValue(LogEvent log, IFormatProvider? format = null)
        {
            return new HashEntry(RedisDefaultKeys.Level, log.Level.ToString());
        }
    }

    public class RedisMessageTemplateWriter : RedisKeyValueWriterBase<HashEntry>
    {
        public override HashEntry PrepareValue(LogEvent log, IFormatProvider? format = null)
        {
            return new HashEntry(RedisDefaultKeys.MessageTemplate, log.MessageTemplate.ToString());
        }
    }

    public class RedisRenderedMessageWriter : RedisKeyValueWriterBase<HashEntry>
    {
        public override HashEntry PrepareValue(LogEvent log, IFormatProvider? format = null)
        {
            return new HashEntry(RedisDefaultKeys.RenderedMesssage, log.RenderMessage(format));
        }
    }

    public class RedisExceptionWriter : RedisKeyValueWriterBase<HashEntry>
    {
        public override HashEntry PrepareValue(LogEvent log, IFormatProvider? format = null)
        {
            RedisValue exception = log.Exception != null ? log.Exception.ToString() : RedisValue.EmptyString;

            return new HashEntry(RedisDefaultKeys.Exception, exception);
        }
    }

    public class RedisPropertyWriter(string PropertyName) : RedisKeyValueWriterBase<HashEntry>
    {
        public override HashEntry PrepareValue(LogEvent log, IFormatProvider? format = null)
        {
            return log.Properties.TryGetValue(PropertyName, out var value) ? 
                new HashEntry(PropertyName, value.ToString()) : new HashEntry(PropertyName, RedisValue.Null);
        }
    }

    public static class RedisDefaultKeys
    {
        public const string Exception = "exception";

        public const string RenderedMesssage = "remdered_message";

        public const string Timestamp = "timestamp";
        
        public const string Level = "level";
        
        public const string MessageTemplate = "message_template";
    }
}
