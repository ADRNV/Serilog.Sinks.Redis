using Seriog.Sink.Redis.Core.Store;
using StackExchange.Redis;

namespace Serilog.Sink.Redis.Tests
{
    public class TestKeyWriter : IKeyGenerator
    {
        public IFormatProvider FormatProvider => throw new NotImplementedException();

        public string TestName {  get; set; }

        public RedisKey Generate(params object[] options)
        {
            return TestName;
        }
    }
}
