using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Student.Management.Api.Helper;

namespace Student.Management.Api.Data;

public class Student
{
    [Key] public string IndexNumber { get; set; } 
    public int DbCounter { get; set; }  
    public string FirstName { get; set; } 
    public string LastName { get; set; }
    public string DateOfBirth { get; set; }
    public string Address { get; set; }
    public string ContactNumber { get; set; }
    public string EmailAddress { get; set; }
    public string EmergencyContactName { get; set; }
    public string EmergencyContactNumber { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
}