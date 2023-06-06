using System.ComponentModel.DataAnnotations;

namespace GradeAndAssessment.Api.Model.RequestModel;

public class StudentAssessmentRequestModel
{
    [Required(AllowEmptyStrings = false)]
    public string TeacherName { get; set; } 
    [Required(AllowEmptyStrings = false)]
    public string IndexNumber { get; set; } 
    [Required(AllowEmptyStrings = false)]
    public string CourseId { get; set; }   //  will replace with course code
    [Required(AllowEmptyStrings = false)]
    public decimal Assignment { get; set; }
    [Required(AllowEmptyStrings = false)]
    public decimal Project { get; set; }
    [Required(AllowEmptyStrings = false)]
    public decimal MidsemExam { get; set; }
    [Required(AllowEmptyStrings = false)]
    public decimal EndOfSemExam { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Password { get; set; }
    [Required,Compare("Password")]
    public string ConfirmPassword { get; set; }
    
}