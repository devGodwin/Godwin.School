namespace Teacher.Management.Api.Model.ResponseModel;

public class TeacherResponseModel
{
    public string TeacherId { get; set; } 
    public string FirstName { get; set; } 
    public string LastName { get; set; }
    public string DateOfBirth { get; set; }
    public string Address { get; set; }
    public string ContactNumber { get; set; }
    public string EmailAddress { get; set; }
    public string Qualification { get; set; }
    public DateTime CreatedAt { get; set; } 
}