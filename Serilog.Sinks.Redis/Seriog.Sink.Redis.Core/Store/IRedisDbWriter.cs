using Serilog.Events;

namespace Seriog.Sink.Redis.Core.Store
{
    public interface IRedisDbWriter
    {
        public void Write(LogEvent log, IFormatProvider? formatProvider = null);
    }
}
