using System.Globalization;
using Serilog.Events;
using StackExchange.Redis;
using Seriog.Sink.Redis.Core.Store;

public class RedisMessageWriterTests
{
    private readonly RedisMessageTemplateWriter _writer = new RedisMessageTemplateWriter();
    private readonly DateTime _timestamp = DateTime.Now;
    private readonly string _messageTemplate = "Test message {0}";

    private readonly LogEvent _logEvent;
    
    public RedisMessageWriterTests()
    {
        var logMessageTemplate = new MessageTemplate(_messageTemplate, []);
        _logEvent = new LogEvent(_timestamp, LogEventLevel.Information, null, logMessageTemplate, new LogEventProperty[0]);
    }

    [Trait("SMOKE", "Message")]
    [Fact]
    public void PrepareValue_Returns_CorrectHashEntry()
    {
        // Arrange

        var expectedHashEntry = new HashEntry(RedisDefaultKeys.MessageTemplate, _messageTemplate);

        // Act
        var result = _writer.PrepareValue(_logEvent);

        // Assert
        Assert.Equal(expectedHashEntry, result);
    }

    [Trait("SMOKE", "Message")]
    [Fact]
    public void PrepareValue_WithDifferentMessage_Returns_CorrectValue()
    {
        // Arrange
        var expectedHashEntry = new HashEntry(RedisDefaultKeys.MessageTemplate, _messageTemplate); 

        // Act
        var result = _writer.PrepareValue(_logEvent);

        // Assert
        Assert.Equal(expectedHashEntry, result);
    }

    [Trait("SMOKE", "Message")]
    [Fact]
    public void PrepareValue_WithNullFormat_Returns_CorrectValue()
    {
        // Arrange
        var expectedHashEntry = new HashEntry(RedisDefaultKeys.MessageTemplate, _messageTemplate);

        // Act
        var result = _writer.PrepareValue(_logEvent, null);

        // Assert
        Assert.Equal(expectedHashEntry, result);
    }
}