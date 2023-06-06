using System.ComponentModel.DataAnnotations;

namespace Course.Management.Api.Data;

 public class Course
{
    [Key]
    public string CourseId { get; set; } = Guid.NewGuid().ToString("N");
    public string CourseName { get; set; } 
    public string CourseDescription { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
}