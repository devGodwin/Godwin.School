using System.ComponentModel.DataAnnotations;

namespace Course.Management.Api.Model.RequestModel;

public class CourseUpdateModel
{
    [Required(AllowEmptyStrings = false)]
    public string CourseName { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string CourseDescription { get; set; }

}