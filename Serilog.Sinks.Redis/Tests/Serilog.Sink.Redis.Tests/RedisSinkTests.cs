using Moq;
using Serilog.Events;
using Serilog.Sink.Redis.Extensions;
using Serilog.Sink.Redis.Options;
using Seriog.Sink.Redis.Core.Options;
using Seriog.Sink.Redis.Core.Store;
using StackExchange.Redis;

namespace Serilog.Sink.Redis.Tests;

public class RedisSinkTests
{
    private readonly Mock<IConnectionMultiplexer> _multiplexerMock;
    private readonly Mock<IDatabase> _databaseMock;
    private readonly ILogger _logger;
    private readonly TestKeyWriter _keyGenerator;
    private readonly IRedisKeyValueOptions _options;

    public RedisSinkTests()
    {
        _multiplexerMock = new Mock<IConnectionMultiplexer>();
        _databaseMock = new Mock<IDatabase>();
        _keyGenerator = new TestKeyWriter();

        _multiplexerMock.Setup(m => m.IsConnected).Returns(true);
        _multiplexerMock
            .Setup(m => m.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
            .Returns(_databaseMock.Object);

        _options = new RedisKeyValueOptionsBuilder()
            .HasDefaultKeyValues()
            .AddKeyGenerator(_keyGenerator)
            .Build();

        _logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Redis(_multiplexerMock.Object, _options, LogEventLevel.Verbose, null)
            .CreateLogger();
    }

    [Theory]
    [InlineData(LogEventLevel.Verbose)]
    [InlineData(LogEventLevel.Debug)]
    [InlineData(LogEventLevel.Information)]
    [InlineData(LogEventLevel.Warning)]
    [InlineData(LogEventLevel.Fatal)]
    public void AllLogValuesWritesCorrect(LogEventLevel logLevel)
    {
        var key = $"AllLogValuesWritesCorrect:{logLevel}";
        _keyGenerator.TestName = key;

        _logger.Write(logLevel, $"{logLevel} Test");

        _databaseMock.Verify(
            db => db.HashSet(
                It.Is<RedisKey>(k => k == new RedisKey(key)),
                It.IsAny<HashEntry[]>(),
                It.IsAny<CommandFlags>()),
            Times.Once);
    }

    [Theory]
    [InlineData(LogEventLevel.Verbose)]
    [InlineData(LogEventLevel.Debug)]
    [InlineData(LogEventLevel.Information)]
    [InlineData(LogEventLevel.Warning)]
    [InlineData(LogEventLevel.Fatal)]
    public void AllLogValuesWithMessagesWritesCorrect(LogEventLevel logLevel)
    {
        var key = $"AllLogValuesWithMessagesWritesCorrect:{logLevel}";
        _keyGenerator.TestName = key;

        _logger.Write(logLevel, "Message from test {Value}", "test value");

        _databaseMock.Verify(
            db => db.HashSet(
                It.Is<RedisKey>(k => k == new RedisKey(key)),
                It.IsAny<HashEntry[]>(),
                It.IsAny<CommandFlags>()),
            Times.Once);
    }
}
