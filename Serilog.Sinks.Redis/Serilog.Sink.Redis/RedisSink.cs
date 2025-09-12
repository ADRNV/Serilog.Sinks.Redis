using Serilog.Core;
using Serilog.Events;
using Seriog.Sink.Redis.Core.Store;
using StackExchange.Redis;

namespace Serilog.Sink.Redis
{
    public class RedisSink : ILogEventSink
    {
        private readonly IRedisDbWriter _redisDbWriter;
        public RedisSink(IConnectionMultiplexer multiplexer, IRedisDbWriter redisDbWriter)
        {
            _redisDbWriter = redisDbWriter;
        }
        
        public void Emit(LogEvent logEvent)
        {
            _redisDbWriter.Write(logEvent);
        }
    }
}
