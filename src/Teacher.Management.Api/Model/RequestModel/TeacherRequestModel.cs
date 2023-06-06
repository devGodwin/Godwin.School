using System.ComponentModel.DataAnnotations;

namespace Teacher.Management.Api.Model.RequestModel;

public class TeacherRequestModel
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
    [Required,EmailAddress] 
    public string EmailAddress { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Qualification { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Password { get; set; }
    [Required,Compare("Password")]
    public string ConfirmPassword { get; set; }
    
}