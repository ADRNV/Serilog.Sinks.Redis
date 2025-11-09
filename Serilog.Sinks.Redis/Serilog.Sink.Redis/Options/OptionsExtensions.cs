using Seriog.Sink.Redis.Core.Options;
using Seriog.Sink.Redis.Core.Store;
using StackExchange.Redis;

namespace Serilog.Sink.Redis.Options
{
    public static class OptionsExtensions
    {
        public static RedisKeyValueOptionsBuilder AddKeyValue(this RedisKeyValueOptionsBuilder builder, string propertyName, RedisKeyValueWriterBase<HashEntry> writer)
        {
            builder.KeyWriters.Add(propertyName, writer);

            return builder;
        }

        public static RedisKeyValueOptionsBuilder HasDefaultKeyValues(this RedisKeyValueOptionsBuilder builder)
        {
            builder.HasDefaultKeyValues = true;

            return builder;
        }

        public static RedisKeyValueOptionsBuilder AddKeyGenerator(this RedisKeyValueOptionsBuilder builder, IKeyGenerator keyGenerator)
        {
            builder.KeyGenerator = keyGenerator;

            return builder;
        }

        public static IRedisKeyValueOptions Build(this RedisKeyValueOptionsBuilder builder)
        {
            return new RedisKeyValueOptions(builder.HasDefaultKeyValues, builder.KeyWriters, builder.KeyGenerator);
        }
    }
}
