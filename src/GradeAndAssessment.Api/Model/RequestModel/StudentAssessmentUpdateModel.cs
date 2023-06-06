using System.ComponentModel.DataAnnotations;

namespace GradeAndAssessment.Api.Model.RequestModel;

public class StudentAssessmentUpdateModel
{
    [Required(AllowEmptyStrings = false)]
    public string TeacherName { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string CourseId { get; set; }
    [Required(AllowEmptyStrings = false)]
    public decimal Assignment { get; set; }
    [Required(AllowEmptyStrings = false)]
    public decimal Project { get; set; }
    [Required(AllowEmptyStrings = false)]
    public decimal MidsemExam { get; set; }
    [Required(AllowEmptyStrings = false)]
    public decimal EndOfSemExam { get; set; }
}