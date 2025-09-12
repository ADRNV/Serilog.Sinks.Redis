using StackExchange.Redis;

namespace Seriog.Sink.Redis.Core.Store
{
    public interface IKeyGenerator
    {
        IFormatProvider FormatProvider { get; }

        public RedisKey Generate(params object[] options);
    }
}
