using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using Teacher.Management.Api.Model;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Teacher.Management.Api.Redis;

public class RedisService:IRedisService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly ILogger<RedisService> _logger;
    private readonly RedisConfig _redisConfig;

    public RedisService(IConnectionMultiplexer connectionMultiplexer, ILogger<RedisService>logger, IOptions<RedisConfig> redisConfig)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _logger = logger;
        _redisConfig = redisConfig.Value;
    }

    public async Task<bool> CacheNewTeacherAsync(CachedTeacher cachedTeacher)
    {
        try
        {
            var teacherKey = RedisConstants.GetTeacherRedisKeyByTeacherId(cachedTeacher.TeacherId);
            var cachedSuccessfully = await _connectionMultiplexer.GetDatabase().StringSetAsync(
                key:teacherKey,
                value: JsonConvert.SerializeObject(cachedTeacher),
                expiry: TimeSpan.FromDays(_redisConfig.DataExpiryDays));
            
            return cachedSuccessfully;
        }
        catch (Exception e)
        {
           _logger.LogError(e,"An error occured caching teacher by teacherKey:{teacherKey}",
               JsonConvert.SerializeObject(cachedTeacher,Formatting.Indented));
           return false;
        }
    }

    public async Task<CachedTeacher> GetTeacherAsync(string teacherId)
    {
        try
        {
            var teacherKey = RedisConstants.GetTeacherRedisKeyByTeacherId(teacherId);
            bool teacherKeyExist = await _connectionMultiplexer.GetDatabase().KeyExistsAsync(teacherKey);
            if (teacherKeyExist)
            {
                var teacherValue = await _connectionMultiplexer.GetDatabase().StringGetAsync(teacherKey);
        
                return JsonSerializer.Deserialize<CachedTeacher>(teacherValue);
            }

            return null;
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured getting teacher by teacherKey:{teacherKey}",teacherId);
            return null;
        }
    }

    public async Task<bool> DeleteTeacherAsync(string teacherId)
    {
        try
        {
            var teacherKey = RedisConstants.GetTeacherRedisKeyByTeacherId(teacherId);
            bool teacherKeyExist = await _connectionMultiplexer.GetDatabase().KeyExistsAsync(teacherKey);
            if (teacherKeyExist)
            {
                var deleteSuccessfully = await _connectionMultiplexer.GetDatabase().KeyDeleteAsync(teacherKey);

                return deleteSuccessfully;
            }

            return false;
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured deleting teacher by teacherKey:{teacherKey}",teacherId);
            return false;
        }
    }
}