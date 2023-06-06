namespace GradeAndAssessment.Api.Redis;

public static class RedisConstants
{
    private const string StudentAssessmentKeyByStudentAssessmentId = "Godwin.School:studentAssessmentId:{studentAssessmentId}";

   public static string GetStudentAssessmentKeyByStudentAssessmentId(string studentAssessmentId) =>
        StudentAssessmentKeyByStudentAssessmentId.Replace("{studentAssessmentId}", studentAssessmentId);

}