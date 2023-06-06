using System.ComponentModel.DataAnnotations;

namespace Student.Management.Api.Model.RequestModel;

public class StudentLoginModel
{
    [Required(AllowEmptyStrings = false)]
    public string IndexNumber { get; set; }
    [Required]
    public string Password { get; set; }
}