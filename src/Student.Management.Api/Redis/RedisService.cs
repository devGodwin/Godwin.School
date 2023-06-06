using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using Student.Management.Api.Model;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace Student.Management.Api.Redis;

public class RedisService:IRedisService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly ILogger<RedisService> _logger;
    private readonly RedisConfig _redisConfig;

    public RedisService(IConnectionMultiplexer connectionMultiplexer, ILogger<RedisService>logger,IOptions<RedisConfig> redisConfig)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _logger = logger;
        _redisConfig = redisConfig.Value;
    }

    public async Task<bool> CacheNewStudentAsync(CachedStudent cachedStudent)
    {
        try
        {
            var studentKey = RedisConstants.GetStudentRedisKeyByIndexNumber(cachedStudent.IndexNumber);
            var cachedSuccessfully = await _connectionMultiplexer.GetDatabase().StringSetAsync(
                key:studentKey,
                value: JsonConvert.SerializeObject(cachedStudent),
                expiry: TimeSpan.FromDays(_redisConfig.DataExpiryDays));
            
            return cachedSuccessfully;
        }
        catch (Exception e)
        {
           _logger.LogError(e,"An error occured caching student by studentKey:{studentKey}",
               JsonConvert.SerializeObject(cachedStudent,Formatting.Indented));
           return false;
        }
    }
    
    public async Task<CachedStudent> GetStudentAsync(string indexNumber)
    {
        try
        {
            var studentKey = RedisConstants.GetStudentRedisKeyByIndexNumber(indexNumber);
            bool studentKeyExist = await _connectionMultiplexer.GetDatabase().KeyExistsAsync(studentKey);
            if (studentKeyExist)
            {
                var studentValue = await _connectionMultiplexer.GetDatabase().StringGetAsync(studentKey);
        
                return JsonSerializer.Deserialize<CachedStudent>(studentValue);
            }

            return null;
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured getting student by studentKey:{studentKey}",indexNumber);
            return null;
        }
    }

    public async Task<bool> DeleteStudentAsync(string indexNumber)
    {
        try
        {
            var studentKey = RedisConstants.GetStudentRedisKeyByIndexNumber(indexNumber);
            bool studentKeyExist = await _connectionMultiplexer.GetDatabase().KeyExistsAsync(studentKey);
            if (studentKeyExist)
            {
                var deleteSuccessfully = await _connectionMultiplexer.GetDatabase().KeyDeleteAsync(studentKey);

                return deleteSuccessfully;
            }

            return false;
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured deleting student by studentKey:{studentKey}",indexNumber);
            return false;
        }
    }
}