using System.ComponentModel.DataAnnotations;

namespace Teacher.Management.Api.Model.RequestModel;

public class TeacherLoginModel
{
    [Required(AllowEmptyStrings = false)]
    public string TeacherId { get; set; }
    [Required]
    public string Password { get; set; }
}