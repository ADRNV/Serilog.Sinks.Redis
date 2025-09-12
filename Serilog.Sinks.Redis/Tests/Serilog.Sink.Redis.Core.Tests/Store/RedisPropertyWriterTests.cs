using Serilog.Events;
using Serilog.Parsing;
using StackExchange.Redis;
using Seriog.Sink.Redis.Core.Store;

public class RedisPropertyWriterTests
{
    private readonly DateTime _timestamp = DateTime.Now;
    private readonly LogEvent _logEventWithProperty;
    private readonly LogEvent _logEventWithoutProperty;

    public RedisPropertyWriterTests()
    {
        var logEventProperty = new LogEventProperty("TestProperty", new ScalarValue("TestValue"));
        _logEventWithProperty = new LogEvent(_timestamp, LogEventLevel.Information, null, MessageTemplate.Empty, [logEventProperty]);
        _logEventWithoutProperty = new LogEvent(_timestamp, LogEventLevel.Information, null, MessageTemplate.Empty, new LogEventProperty[0]);
    }

    [Trait("SMOKE", "Property")]
    [Fact]
    public void PrepareValue_WithExistingProperty_Returns_PropertyValue()
    {
        // Arrange
        var writer = new RedisPropertyWriter("TestProperty");
        var expectedHashEntry = new HashEntry("TestProperty", "\"TestValue\"");

        // Act
        var result = writer.PrepareValue(_logEventWithProperty);

        // Assert
        Assert.Equal(expectedHashEntry, result);
    }

    [Trait("SMOKE", "Property")]
    [Fact]
    public void PrepareValue_WithMissingProperty_Returns_Null()
    {
        // Arrange
        var writer = new RedisPropertyWriter("NonExistentProperty");
        var expectedHashEntry = new HashEntry("NonExistentProperty", RedisValue.Null);

        // Act
        var result = writer.PrepareValue(_logEventWithProperty);

        // Assert
        Assert.Equal(expectedHashEntry, result);
    }

    [Trait("SMOKE", "Property")]
    [Fact]
    public void PrepareValue_WithEmptyProperties_Returns_Null()
    {
        // Arrange
        var writer = new RedisPropertyWriter("TestProperty");
        var expectedHashEntry = new HashEntry("TestProperty", RedisValue.Null);

        // Act
        var result = writer.PrepareValue(_logEventWithoutProperty);

        // Assert
        Assert.Equal(expectedHashEntry, result);
    }

    [Trait("SMOKE", "Property")]
    [Fact]
    public void PrepareValue_WithNullFormat_Returns_CorrectValue()
    {
        // Arrange
        var writer = new RedisPropertyWriter("TestProperty");
        var expectedHashEntry = new HashEntry("TestProperty", "\"TestValue\"");

        // Act
        var result = writer.PrepareValue(_logEventWithProperty, null);

        // Assert
        Assert.Equal(expectedHashEntry, result);
    }

    [Trait("SMOKE", "Scalar property")]
    [Fact]
    public void PrepareValue_WithDifferentPropertyTypes_Returns_StringRepresentation()
    {
        // Arrange
        var intProperty = new List<LogEventProperty>
        {
            new LogEventProperty("IntProperty", new ScalarValue(42))
        };
        var logEvent = new LogEvent(_timestamp, LogEventLevel.Information, null, MessageTemplate.Empty, intProperty);
        
        var writer = new RedisPropertyWriter("IntProperty");
        var expectedHashEntry = new HashEntry("IntProperty", 42);

        // Act
        var result = writer.PrepareValue(logEvent);

        // Assert
        Assert.Equal(expectedHashEntry, result);
    }
    
    [Trait("SMOKE", "Collection property")]
    [Fact]
    public void PrepareValue_WithCollectionPropertyTypes_Returns_StringRepresentation()
    {
        // Arrange
        const string intArrayPropertyName = "IntArrayProperty";

        var messageTemplate = new MessageTemplate("Value is {IntArrayProperty}", [new TextToken(intArrayPropertyName)]);
        
        ScalarValue[] intArrayValue = [
            new ScalarValue(1), 
            new ScalarValue(2),
            new ScalarValue(3)
        ];
        
        var intArrayLogProperty = new List<LogEventProperty>
        {
            new LogEventProperty(intArrayPropertyName, new SequenceValue(intArrayValue))
        };
        
        var logEvent = new LogEvent(_timestamp, LogEventLevel.Information, null, messageTemplate, intArrayLogProperty);
        
        var writer = new RedisPropertyWriter(intArrayPropertyName);
        var expectedHashEntry = new HashEntry(intArrayPropertyName, logEvent.Properties[intArrayPropertyName].ToString());

        // Act
        var result = writer.PrepareValue(logEvent);

        // Assert
        Assert.Equal(expectedHashEntry, result);
    }
    
    [Trait("SMOKE", "Collection property")]
    [Fact]
    public void PrepareValue_WithStructPropertyTypes_Returns_StringRepresentation()
    {
        // Arrange 
        const string structPropertyName = "StructProperty";

        var messageTemplate = new MessageTemplate("Value is {StructProperty}", [new TextToken(structPropertyName)]);

        var structuredProperties = new LogEventProperty[]
        {
            new LogEventProperty("intProp", new ScalarValue(1)), 
            new LogEventProperty("decimal", new ScalarValue(3.1415)),
            new LogEventProperty("float", new ScalarValue(3.14)),
            new LogEventProperty("doobleProp", new ScalarValue(3.14159264)),
            new LogEventProperty("dateTimeProp", new ScalarValue(DateTime.Now)),
            new LogEventProperty("stringProp", new ScalarValue(Guid.NewGuid().ToString())),
            new LogEventProperty("nullProp", new ScalarValue(null)),
        };
        
        var intArrayLogProperty = new List<LogEventProperty>
        {
            new LogEventProperty(structPropertyName, new StructureValue(structuredProperties))
        };
        
        var logEvent = new LogEvent(_timestamp, LogEventLevel.Information, null, messageTemplate, intArrayLogProperty);
        
        var writer = new RedisPropertyWriter(structPropertyName);
        var expectedHashEntry = new HashEntry(structPropertyName, logEvent.Properties[structPropertyName].ToString());

        // Act
        var result = writer.PrepareValue(logEvent);

        // Assert
        Assert.Equal(expectedHashEntry, result);
    }
}