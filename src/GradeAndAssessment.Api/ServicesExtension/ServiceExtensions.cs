using GradeAndAssessment.Api.Redis;
using StackExchange.Redis;

namespace GradeAndAssessment.Api.ServicesExtension;

public static class ServiceExtensions
{
    public static void AddRedisCache(this IServiceCollection service, Action<RedisConfig>redisConfig)
    {
        if (service is null) throw new ArgumentNullException(nameof(service));

        service.Configure(redisConfig);

        var redisConfiguration = new RedisConfig();
        redisConfig.Invoke(redisConfiguration);

        var connectionMultiplexer = ConnectionMultiplexer.Connect(new ConfigurationOptions()
        {
            EndPoints = { redisConfiguration.BaseUrl },
            AllowAdmin = true,
            AbortOnConnectFail = false,
            ReconnectRetryPolicy = new LinearRetry(500),
            DefaultDatabase = redisConfiguration.Database
        });

        service.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);
        service.AddSingleton<IRedisService, RedisService>();
    }
}