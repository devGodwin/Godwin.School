namespace Student.Management.Api.Redis;

public static class RedisConstants
{
    private const string StudentKeyByIndexNumber = "Godwin.School:student:{indexNumber}";

    public static string GetStudentRedisKeyByIndexNumber(string indexNumber) =>
        StudentKeyByIndexNumber.Replace("{indexNumber}", indexNumber);
}