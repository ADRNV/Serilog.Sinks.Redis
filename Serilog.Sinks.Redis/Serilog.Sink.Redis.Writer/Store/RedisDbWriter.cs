using Serilog.Debugging;
using Serilog.Events;
using Seriog.Sink.Redis.Core.Options;
using Seriog.Sink.Redis.Core.Store;
using StackExchange.Redis;

namespace Serilog.Sink.Redis.Writer.Store
{
    public class RedisDbWriter(IConnectionMultiplexer multiplexer, IRedisKeyValueOptions options) : IRedisDbWriter
    {
        private readonly IKeyGenerator _keyGenerator = options.KeyGenerator;
        public void Write(LogEvent log, IFormatProvider? formatProvider = null)
        {
            HashEntry[] logEntry = new HashEntry[options.WriteKeyValues.Count];
            
            int entryIndex = 0;
            
            log.RenderMessage();
            
            foreach (var i in options.WriteKeyValues)
            {
                logEntry[entryIndex] = i.Value.PrepareValue(log, formatProvider);
                entryIndex++;
            }
            
            var key = _keyGenerator.Generate();

            using (multiplexer)
            {
                if (multiplexer.IsConnected)
                {
                    multiplexer.GetDatabase().HashSet(key, logEntry);
                    SelfLog.WriteLine("[{0}] Log message wrote [{1}] with ID [{2}]", arg0: log.Timestamp, arg1: log.SpanId, arg2: key);
                }
                else
                {
                    SelfLog.WriteLine("[{0}] Failed to wrote message [{1}] with ID [{2}]", arg0: log.Timestamp, arg1: log.SpanId, arg2: key);
                }
            }
            
        }
    }
}
