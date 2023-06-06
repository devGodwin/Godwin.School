using Student.Management.Api.Model;

namespace Student.Management.Api.Redis;

public interface IRedisService
{
    Task<bool> CacheNewStudentAsync(CachedStudent cachedStudent);
    Task<CachedStudent> GetStudentAsync(string indexNumber);
    Task<bool> DeleteStudentAsync(string indexNumber);
}