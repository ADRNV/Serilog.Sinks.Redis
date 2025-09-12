using Seriog.Sink.Redis.Core.Store;
using StackExchange.Redis;

namespace Serilog.Sink.Redis.Writer.Store
{
    public class GuidV7Generator(IFormatProvider formatProvider) : IKeyGenerator
    {
        public IFormatProvider FormatProvider { get => formatProvider; }

        public RedisKey Generate(params object[] options)
        {
            var guid = Guid.CreateVersion7();

            return new RedisKey(guid.ToString());
        }
    }

    public class TimeStampKeyGenerator(IFormatProvider formatProvider) : IKeyGenerator
    {
        public IFormatProvider FormatProvider { get => formatProvider; }

        public RedisKey Generate(params object[] options)
        {
            var timeStamp = DateTime.Now;

            return new RedisKey(timeStamp.ToString(formatProvider));
        }
    }
}
