using Seriog.Sink.Redis.Core.Store;
using StackExchange.Redis;

namespace Seriog.Sink.Redis.Core.Options
{
    public interface IRedisKeyValueOptions
    {
        public bool HasDefaultKeyValues { get; }

        public IKeyGenerator KeyGenerator { get; }

        public IReadOnlyDictionary<string, RedisKeyValueWriterBase<HashEntry>> WriteKeyValues { get; }
    }
}
