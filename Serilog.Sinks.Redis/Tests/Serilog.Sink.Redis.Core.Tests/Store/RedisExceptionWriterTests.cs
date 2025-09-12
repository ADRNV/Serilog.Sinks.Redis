using System.Globalization;
using Serilog.Events;
using StackExchange.Redis;
using Seriog.Sink.Redis.Core.Store;

public class RedisExceptionWriterTests
{
    private readonly RedisExceptionWriter _writer = new RedisExceptionWriter();
    private readonly DateTime _timestamp = DateTime.Now;
    private readonly LogEvent _logEventWithoutException;
    private readonly LogEvent _logEventWithException;

    public RedisExceptionWriterTests()
    {
        _logEventWithoutException = new LogEvent(_timestamp, LogEventLevel.Information, null, MessageTemplate.Empty, new LogEventProperty[0]);
        
        var exception = new InvalidOperationException("Test exception");
        _logEventWithException = new LogEvent(_timestamp, LogEventLevel.Error, exception, MessageTemplate.Empty, new LogEventProperty[0]);
    }

    [Trait("SMOKE", "Exception")]
    [Fact]
    public void PrepareValue_WithoutException_Returns_NullHashEntry()
    {
        // Arrange
        var expectedHashEntry = new HashEntry(RedisDefaultKeys.Exception, RedisValue.Null);

        // Act
        var result = _writer.PrepareValue(_logEventWithoutException);

        // Assert
        Assert.Equal(expectedHashEntry, result);
    }

    [Trait("SMOKE", "Exception")]
    [Fact]
    public void PrepareValue_WithException_Returns_ExceptionToString()
    {
        // Arrange
        var expectedExceptionString = _logEventWithException.Exception.ToString();
        var expectedHashEntry = new HashEntry(RedisDefaultKeys.Exception, expectedExceptionString);

        // Act
        var result = _writer.PrepareValue(_logEventWithException);

        // Assert
        Assert.Equal(expectedHashEntry, result);
    }

    [Trait("SMOKE", "Exception")]
    [Fact]
    public void PrepareValue_WithNullFormat_Returns_CorrectValue()
    {
        // Arrange
        var expectedHashEntry = new HashEntry(RedisDefaultKeys.Exception, RedisValue.Null);

        // Act
        var result = _writer.PrepareValue(_logEventWithoutException, null);

        // Assert
        Assert.Equal(expectedHashEntry, result);
    }
}