using Serilog.Core;
using Serilog.Events;
using Serilog.Sink.Redis.Writer.Store;
using Seriog.Sink.Redis.Core.Options;
using StackExchange.Redis;

namespace Serilog.Sink.Redis.Extensions;


public static class LoggerSinkConfigurationExtensions
{
    public static LoggerConfiguration Redis(
        this LoggerConfiguration loggerConfiguration,
        IConnectionMultiplexer multiplexer, 
        IRedisKeyValueOptions options,
        LogEventLevel restrictedToMinLevel = LevelAlias.Minimum,
        LoggingLevelSwitch? loggingLevelSwitch = null)
    {
        var redisDbWriter = new RedisDbWriter(multiplexer, options);
        var redisSink = new RedisSink(multiplexer, redisDbWriter);
        
        loggerConfiguration.WriteTo.Sink(redisSink, restrictedToMinLevel, loggingLevelSwitch);

        return loggerConfiguration;
    }
}