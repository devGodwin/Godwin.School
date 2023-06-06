using System.ComponentModel.DataAnnotations;

namespace Finance.Management.Api.Model.RequestModel;

public class StatementOfAccountRequestModel
{
    [Required(AllowEmptyStrings = false)]
    public string AdminName { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string IndexNumber { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string AcademicYear { get; set; }  //ie. 2020/2021
    [Required(AllowEmptyStrings = false)]
    public string Level { get; set; }  
    [Required(AllowEmptyStrings = false)]
    public string PaymentType { get; set; }
    [Required]
    public decimal Fees { get; set; }
    [Required]
    public decimal AmountPaid { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Bank { get; set; }
   
    [Required(AllowEmptyStrings = false)]         // use role base auth 
    public string Password { get; set; }          // * 
    [Required,Compare("Password")]      // *
    public string ConfirmPassword { get; set; }   // use role base auth
    
}