namespace Finance.Management.Api.Model.Filter;

public class StatementOfAccountFilter:BaseFilter
{
    public string StatementId { get; set; } 
    public string AdminName { get; set; }
    public string IndexNumber { get; set; }
    public string AcademicYear { get; set; }  
    public DateTime PaymentDate { get; set; } 
    public string Level { get; set; }  
    public string PaymentType { get; set; }
    public decimal Fees { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal Arrear { get; set; }
    public decimal Balance { get; set; }
    public string Bank { get; set; }
}