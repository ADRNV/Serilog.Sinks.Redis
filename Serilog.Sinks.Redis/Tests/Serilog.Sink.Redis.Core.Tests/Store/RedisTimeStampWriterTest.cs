using Serilog.Events;
using StackExchange.Redis;
using System.Globalization;
using Seriog.Sink.Redis.Core.Store;

public class RedisTimeStampWriterTests
{ 
    private readonly RedisTimeStampWriter _writer = new RedisTimeStampWriter();
    private readonly DateTime _timestamp = DateTime.Now; 
    private readonly LogEvent _logEvent;

    public RedisTimeStampWriterTests()
    {
        _logEvent = new LogEvent(_timestamp, LogEventLevel.Information, null, MessageTemplate.Empty, new LogEventProperty[0]);
    }
    
    [Trait("SMOKE", "Timestamp")]
    [Fact]
    public void PrepareValue_Returns_CorrectHashEntry()
    {
        // Arrange
        var expectedHashEntry = new HashEntry(RedisDefaultKeys.Timestamp, _timestamp.ToString());

        // Act
        var result = _writer.PrepareValue(_logEvent);

        // Assert
        Assert.Equal(expectedHashEntry, result);
    }

    [Trait("SMOKE", "Timestamp")]
    [Fact]
    public void PrepareValue_WithCustomFormat_Returns_FormattedValue()
    {
        // Arrange
        var formatProvider = CultureInfo.InvariantCulture;
        var expectedHashEntry = new HashEntry(RedisDefaultKeys.Timestamp, _timestamp.ToString(formatProvider));

        // Act
        var result = _writer.PrepareValue(_logEvent, formatProvider);

        // Assert
        Assert.Equal(expectedHashEntry, result);
    }

    [Trait("SMOKE", "Timestamp")]
    [Fact]
    public void PrepareValue_WithNullFormat_UsesDefaultBehavior()
    {
        // Arrange
        var expectedHashEntry = new HashEntry(RedisDefaultKeys.Timestamp, _timestamp.ToString());

        // Act
        var result = _writer.PrepareValue(_logEvent, null);

        // Assert
        Assert.Equal(expectedHashEntry, result);
    }

    [Trait("SMOKE", "Timestamp")]
    [Fact]
    public void PrepareValue_WithDifferentCulture_Returns_CulturallyAwareString()
    {
        // Arrange
        var frenchCulture = new CultureInfo("fr-FR");
        var expectedHashEntry = new HashEntry(RedisDefaultKeys.Timestamp, _timestamp.ToString(frenchCulture));

        // Act
        var result = _writer.PrepareValue(_logEvent, frenchCulture);

        // Assert
        Assert.Equal(expectedHashEntry, result);
    }

    [Trait("SMOKE", "Timestamp")]
    [Fact]
    public void PrepareValue_WithSpecificCulture_Formats_AccordingToCulture()
    {
        // Arrange
        var russianCulture = new CultureInfo("ru-RU");
        var expectedHashEntry = new HashEntry(RedisDefaultKeys.Timestamp, _timestamp.ToString(russianCulture));

        // Act
        var result = _writer.PrepareValue(_logEvent, russianCulture);

        // Assert
        Assert.Equal(expectedHashEntry, result);
    }
}