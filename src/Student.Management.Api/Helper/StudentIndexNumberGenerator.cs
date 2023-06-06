

namespace Student.Management.Api.Helper;
public static class StudentIndexNumberGenerator
{
    public static string GenerateStudentIndexNumber(Data.Student student)
    {
        string lastThreeDigits = student.DbCounter.ToString("D3"); 
        string indexNumber = "07177" + lastThreeDigits;
        return indexNumber;
    }
}