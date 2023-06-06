using System.ComponentModel.DataAnnotations;

namespace GradeAndAssessment.Api.Model.RequestModel;

public class StudentAssessmentLoginModel
{
    [Required(AllowEmptyStrings = false)]
    public string AssessmentId { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Password { get; set; }
}