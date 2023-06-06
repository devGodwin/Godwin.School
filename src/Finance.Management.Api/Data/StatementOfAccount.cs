using System.ComponentModel.DataAnnotations;

namespace Finance.Management.Api.Data;

public class StatementOfAccount
{
    [Key] public string StatementId { get; set; } = Guid.NewGuid().ToString("N");
    public string AdminName { get; set; }
    public string IndexNumber { get; set; }
    public string AcademicYear { get; set; }  //ie. 2020/2021
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow; 
    public string Level { get; set; }  
    public string PaymentType { get; set; }
    public decimal Fees { get; set; } 
    public decimal AmountPaid { get; set; }
    public decimal Arrear { get; set; }
    public decimal Balance { get; set; }
    public string Bank { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }

}