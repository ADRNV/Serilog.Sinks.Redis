using Serilog.Events;
using StackExchange.Redis;

namespace Seriog.Sink.Redis.Core.Store
{
    public interface IRedisDbWriter
    {
        public RedisKey Write(LogEvent log, IFormatProvider? formatProvider = null);

    }
}
