using Serilog.Events;
using StackExchange.Redis;
using Seriog.Sink.Redis.Core.Store;

public class RedisLevelWriterTests
{
    private readonly RedisLevelWriter _writer = new RedisLevelWriter();
    private readonly DateTime _timestamp = DateTime.Now;
    private readonly LogEvent _logEvent;

    public RedisLevelWriterTests()
    {
        _logEvent = new LogEvent(_timestamp, LogEventLevel.Information, null, MessageTemplate.Empty, new LogEventProperty[0]);
    }

    [Trait("SMOKE", "Level")]
    [Fact]
    public void PrepareValue_Returns_CorrectHashEntry()
    {
        // Arrange
        var expectedHashEntry = new HashEntry(RedisDefaultKeys.Level, "Information");

        // Act
        var result = _writer.PrepareValue(_logEvent);

        // Assert
        Assert.Equal(expectedHashEntry, result);
    }

    [Trait("SMOKE", "Level")]
    [Theory]
    [InlineData(LogEventLevel.Verbose, "Verbose")]
    [InlineData(LogEventLevel.Debug, "Debug")]
    [InlineData(LogEventLevel.Warning, "Warning")]
    [InlineData(LogEventLevel.Error, "Error")]
    [InlineData(LogEventLevel.Fatal, "Fatal")]
    public void PrepareValue_WithDifferentLevels_Returns_CorrectValue(LogEventLevel level, string expectedLevel)
    {
        // Arrange
        var logEvent = new LogEvent(_timestamp, level, null, MessageTemplate.Empty, new LogEventProperty[0]);
        var expectedHashEntry = new HashEntry(RedisDefaultKeys.Level, expectedLevel);

        // Act
        var result = _writer.PrepareValue(logEvent);

        // Assert
        Assert.Equal(expectedHashEntry, result);
    }

    [Trait("SMOKE", "Level")]
    [Fact]
    public void PrepareValue_WithNullFormat_Returns_CorrectValue()
    {
        // Arrange
        var expectedHashEntry = new HashEntry(RedisDefaultKeys.Level, "Information");

        // Act
        var result = _writer.PrepareValue(_logEvent, null);

        // Assert
        Assert.Equal(expectedHashEntry, result);
    }
}