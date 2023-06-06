namespace Teacher.Management.Api.Redis;

public static class RedisConstants
{
    private const string TeacherKeyByTeacherId = "Godwin.School:teacher:{teacherId}";

    public static string GetTeacherRedisKeyByTeacherId(string teacherId) =>
        TeacherKeyByTeacherId.Replace("{teacherId}", teacherId);
}