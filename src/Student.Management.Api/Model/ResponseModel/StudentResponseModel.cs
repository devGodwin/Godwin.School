

namespace Student.Management.Api.Model.ResponseModel;

public class StudentResponseModel
{ 
    public string IndexNumber { get; set; }   
    public string FirstName { get; set; } 
    public string LastName { get; set; }
    public string DateOfBirth { get; set; }
    public string Address { get; set; }
    public string ContactNumber { get; set; }
    public string EmailAddress { get; set; }
    public string EmergencyContactName { get; set; }
    public string EmergencyContactNumber { get; set; }
    public DateTime CreatedAt { get; set; } 
}