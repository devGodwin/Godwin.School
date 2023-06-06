using Teacher.Management.Api.Model;

namespace Teacher.Management.Api.Redis;

public interface IRedisService
{
    Task<bool> CacheNewTeacherAsync(CachedTeacher cachedTeacher);
    Task<CachedTeacher> GetTeacherAsync(string teacherId);
    Task<bool> DeleteTeacherAsync(string teacherId);
}