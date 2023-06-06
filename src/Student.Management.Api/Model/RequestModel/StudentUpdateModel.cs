using System.ComponentModel.DataAnnotations;

namespace Student.Management.Api.Model.RequestModel;

public class StudentUpdateModel
{
    [Required(AllowEmptyStrings = false)]
    public string FirstName { get; set; } 
    [Required(AllowEmptyStrings = false)]
    public string LastName { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string DateOfBirth { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Address { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string ContactNumber { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string EmailAddress { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string EmergencyContactName { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string EmergencyContactNumber { get; set; }
}