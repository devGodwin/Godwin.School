using System.ComponentModel.DataAnnotations;

namespace GradeAndAssessment.Api.Data;

public class StudentAssessment
{
    [Key]
    public string AssessmentId { get; set; } = Guid.NewGuid().ToString("N");
    public string TeacherName { get; set; } 
    public string IndexNumber { get; set; }  
    public string CourseId { get; set; }   //  will replace with course code
    public decimal Assignment { get; set; }
    public decimal Project { get; set; }
    public decimal MidsemExam { get; set; }
    public decimal EndOfSemExam { get; set; }
    public decimal Score { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }

}