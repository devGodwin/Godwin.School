using GradeAndAssessment.Api.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace GradeAndAssessment.Api.Redis;

public class RedisService:IRedisService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly ILogger<RedisService> _logger;
    private readonly RedisConfig _redisConfig;

    public RedisService(IConnectionMultiplexer connectionMultiplexer, IOptions<RedisConfig> redisConfig,ILogger<RedisService> logger)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _logger = logger;
        _redisConfig = redisConfig.Value;
    }
    public async Task<bool> CacheNewStudentAssessmentAsync(CachedStudentAssessment cachedStudentAssessment)
    {
        try
        {
            var assessmentKey =
                RedisConstants.GetStudentAssessmentKeyByStudentAssessmentId(cachedStudentAssessment.IndexNumber);
            var cachedSuccessfully = await _connectionMultiplexer.GetDatabase().StringSetAsync(
                key: assessmentKey,
                value:JsonConvert.SerializeObject(cachedStudentAssessment),
                expiry: TimeSpan.FromDays(_redisConfig.DataExpiryDays));

            return cachedSuccessfully;
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured caching student assessment\n{cachedStudentAssessment}",
                JsonConvert.SerializeObject(cachedStudentAssessment,Formatting.Indented));
            
            return false;
        }

    }

    public async Task<CachedStudentAssessment> GetStudentAssessmentAsync(string indexNumber)
    {
        try
        {
            var assessmentKey = RedisConstants.GetStudentAssessmentKeyByStudentAssessmentId(indexNumber);
            var assessmentExist = await _connectionMultiplexer.GetDatabase().KeyExistsAsync(assessmentKey);
            if (assessmentExist)
            {
                var assessmentValue =  await _connectionMultiplexer.GetDatabase().StringGetAsync(assessmentKey);
          
                return JsonSerializer.Deserialize<CachedStudentAssessment>(assessmentValue);
            }

            return null;
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured getting student assessment by assessmentKey:{assessmentKey}", indexNumber);

            return null;
        }
    }

    public async Task<bool> DeleteStudentAssessmentAsync(string indexNumber)
    {
        try
        {
            var assessmentKey = RedisConstants.GetStudentAssessmentKeyByStudentAssessmentId(indexNumber);
            var assessmentExist = await _connectionMultiplexer.GetDatabase().KeyExistsAsync(assessmentKey);
            if (assessmentExist)
            { 
                return await _connectionMultiplexer.GetDatabase().KeyDeleteAsync(assessmentKey);
            }

            return false;
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured deleting student assessment by assessmentKey:{assessmentKey}",indexNumber);
            
            return false;
        }
    }
}