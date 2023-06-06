using System.ComponentModel.DataAnnotations;

namespace Teacher.Management.Api.Data;

public class Teacher
{
    [Key]
    public string TeacherId { get; set; } = Guid.NewGuid().ToString("N");
    public string FirstName { get; set; } 
    public string LastName { get; set; }
    public string DateOfBirth { get; set; }
    public string Address { get; set; }
    public string ContactNumber { get; set; }
    public string EmailAddress { get; set; }
    public string Qualification { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
}