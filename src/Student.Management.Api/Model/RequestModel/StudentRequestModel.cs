using System.ComponentModel.DataAnnotations;

namespace Student.Management.Api.Model.RequestModel;

public class StudentRequestModel
{
    [Required]
    public int DbCounter { get; set; }  
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
    [Required,EmailAddress] 
    public string EmailAddress { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string EmergencyContactName { get; set; }
    [Required(AllowEmptyStrings = false)] 
    public string EmergencyContactNumber { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Password { get; set; }
    [Required,Compare("Password")]
    public string ConfirmPassword { get; set; }
}