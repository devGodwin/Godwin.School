namespace Course.Management.Api.Model.Filter;

public class CourseFilter:BaseFilter
{
    public string CourseId { get; set; } 
    public string CourseName { get; set; } 
    public string CourseDescription { get; set; }

    public DateTime CreatedAt { get; set; } 
}