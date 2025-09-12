using Seriog.Sink.Redis.Core.Options;
using Seriog.Sink.Redis.Core.Store;
using StackExchange.Redis;

namespace Serilog.Sink.Redis.Options
{
    public class RedisKeyValueOptionsBuilder
    {
        internal Dictionary<string, RedisKeyValueWriterBase<HashEntry>> KeyWriters { get; }

        internal  IKeyGenerator KeyGenerator { get; set; }

        internal bool HasDefaultKeyValues { get; set; }

        public RedisKeyValueOptionsBuilder()
        {
            KeyWriters = new Dictionary<string, RedisKeyValueWriterBase<HashEntry>>();
        }
    }
}
