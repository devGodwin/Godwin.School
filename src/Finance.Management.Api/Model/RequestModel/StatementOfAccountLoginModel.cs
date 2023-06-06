using System.ComponentModel.DataAnnotations;

namespace Finance.Management.Api.Model.RequestModel;

public class StatementOfAccountLoginModel
{
    [Required(AllowEmptyStrings = false)]
    public string AdminName { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Password { get; set; }
}