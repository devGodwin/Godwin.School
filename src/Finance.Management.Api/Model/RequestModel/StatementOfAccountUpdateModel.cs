using System.ComponentModel.DataAnnotations;

namespace Finance.Management.Api.Model.RequestModel;

public class StatementOfAccountUpdateModel
{
    [Required(AllowEmptyStrings = false)]
    public string AcademicYear { get; set; }  //ie. 2020/2021
    [Required(AllowEmptyStrings = false)]
    public DateTime PaymentDate { get; set; } 
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
}