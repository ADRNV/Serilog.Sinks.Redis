using Microsoft.Extensions.Configuration;
using Serilog.Events;
using Serilog.Sink.Redis.Extensions;
using Serilog.Sink.Redis.Options;
using Serilog.Sink.Redis.Writer.Store;
using Seriog.Sink.Redis.Core.Options;
using Seriog.Sink.Redis.Core.Store;
using StackExchange.Redis;

namespace Serilog.Sink.Redis.Tests;

public class RedisSinkTests : IDisposable
{
    ILogger _logger;

    private IConnectionMultiplexer _connection;

    private IRedisKeyValueOptions _options;

    private TestKeyWriter _keyGenerator;

    IConfiguration _configuration;

    private string _crendentials;

    public RedisSinkTests()
    {
        _keyGenerator = new TestKeyWriter();

        _configuration = new ConfigurationBuilder()
            .AddJsonFile(Environment.CurrentDirectory+ "\\appConfig.json")
            .Build();

        _crendentials = _configuration.GetConnectionString("redisCredentials")!;

        _connection = ConnectionMultiplexer
            .Connect(_crendentials, c =>
            {
                c.AbortOnConnectFail = false;
                c.ConnectTimeout = 10_000;
            });

        _options = new RedisKeyValueOptionsBuilder()
            .HasDefaultKeyValues()
            .AddKeyGenerator(_keyGenerator)
            .Build();

        _connection.GetDatabase().Ping();
        
        _logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Redis(_connection, _options, LogEventLevel.Verbose, null)
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
        _connection = ConnectionMultiplexer.Connect(_crendentials, c =>
        {
            c.AbortOnConnectFail = false;
            c.AllowAdmin = true;
        });

        var key = $"AllLogValuesWithMessagesWritesCorrect:{logLevel.ToString()}";

        _keyGenerator.TestName = key;

        _logger.Write(logLevel, $"{logLevel} Test");

        var fact = _connection.GetDatabase().HashGetAll(new RedisKey(key));

        Assert.True(fact is not null);

    }

    [Theory]
    [InlineData(LogEventLevel.Verbose)]
    [InlineData(LogEventLevel.Debug)]
    [InlineData(LogEventLevel.Information)]
    [InlineData(LogEventLevel.Warning)]
    [InlineData(LogEventLevel.Fatal)]
    public void AllLogValuesWithMessagesWritesCorrect(LogEventLevel logLevel)
    {
        _logger = new LoggerConfiguration()
           .MinimumLevel.Verbose()
           .Redis(_connection, _options, LogEventLevel.Verbose, null)
           .CreateLogger();

        var key = $"AllLogValuesWithMessagesWritesCorrect:{logLevel.ToString()}";

        _keyGenerator.TestName = key;

        _connection = ConnectionMultiplexer.Connect(_crendentials, c =>
        {
            c.AbortOnConnectFail = false;
            c.AllowAdmin = true;
        });
        
        _logger.Write(logLevel, "Message from test {0}", "test value");

        var fact = _connection.GetDatabase().HashGetAll(new RedisKey(key));

        Assert.True(fact is not null);
    }

    public void Dispose()
    {
        _connection.GetServers()
            .First()
            .FlushDatabase();
    }
}