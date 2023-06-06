using System.ComponentModel.DataAnnotations;

namespace Teacher.Management.Api.Model.Filter;

public class BaseFilter
{
    [Range(1,int.MaxValue)] 
    public int CurrentPage { get; set; } = 1;
    [Range(1,100)]
    public int PageSize { get; set; } = 10;
    public string OrderBy { get; set; } = "desc";
}