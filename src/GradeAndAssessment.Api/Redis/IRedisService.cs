using GradeAndAssessment.Api.Model;

namespace GradeAndAssessment.Api.Redis;

public interface IRedisService
{
    Task<bool> CacheNewStudentAssessmentAsync(CachedStudentAssessment cachedStudentAssessment);
    Task<CachedStudentAssessment> GetStudentAssessmentAsync(string indexNumber);
    Task<bool> DeleteStudentAssessmentAsync(string indexNumber);
}