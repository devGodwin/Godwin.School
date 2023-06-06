using System.ComponentModel.DataAnnotations;

namespace Course.Management.Api.Model.RequestModel;

public class CourseRequestModel
{
    
    [Required(AllowEmptyStrings = false)]
    public string CourseName{ get; set; }
    [Required(AllowEmptyStrings = false)] 
    public string CourseDescription { get; set; }

}