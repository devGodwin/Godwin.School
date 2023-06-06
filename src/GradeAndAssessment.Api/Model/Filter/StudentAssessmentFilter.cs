namespace GradeAndAssessment.Api.Model.Filter;

public class StudentAssessmentFilter:BaseFilter
{
    public string AssessmentId { get; set; }
    public string IndexNumber { get; set; }  
    public string CourseId { get; set; }   //  will replace with course code
    public decimal Assignment { get; set; }
    public decimal Project { get; set; }
    public decimal MidsemExam { get; set; }
    public decimal EndOfSemExam { get; set; }
    public decimal Score { get; set; } 
    public DateTime CreatedAt { get; set; } 
}