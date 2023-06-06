using Finance.Management.Api.Model.Filter;
using Finance.Management.Api.Model.RequestModel;
using Finance.Management.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finance.Management.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class StatementOfAccountController:ControllerBase
{
    private readonly IStatementOfAccountServices _statementOfAccountServices;

    public StatementOfAccountController(IStatementOfAccountServices statementOfAccountServices)
    {
        _statementOfAccountServices = statementOfAccountServices;
    }

    /// <summary>
    /// Filter student statement of account
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpGet()]
    public async Task<IActionResult> GetStatementOfAccounts([FromQuery] StatementOfAccountFilter filter)
    {
        var response = await _statementOfAccountServices.GetStatementOfAccountsAsync(filter);
        return StatusCode(response.Code, response);
    }
    
    /// <summary>
    /// Retrieve a student statement of account
    /// </summary>
    /// <param name="indexNumber"></param>
    /// <returns></returns>
    [HttpGet("{indexNumber:required}")]
    public async Task<IActionResult> GetStatementOfAccountById([FromRoute] string indexNumber)
    {
        var response = await _statementOfAccountServices.GetStatementOfAccountByIdAsync(indexNumber);
        return StatusCode(response.Code, response);
    }

    /// <summary>
    /// Update a student statement of account
    /// </summary>
    /// <param name="indexNumber"></param>
    /// <param name="updateModel"></param>
    /// <returns></returns>
    [HttpPut("{indexNumber:required}")]
    public async Task<IActionResult> UpdateStatementOfAccount([FromRoute] string indexNumber,
        [FromBody] StatementOfAccountUpdateModel updateModel)
    {
        var response = await _statementOfAccountServices.UpdateStatementOfAccountAsync(indexNumber, updateModel);
        return StatusCode(response.Code, response);
    }

    /// <summary>
    /// Delete  student statement of account
    /// </summary>
    /// <param name="indexNumber"></param>
    /// <returns></returns>
    [HttpDelete("{indexNumber:required}")]
    public async Task<IActionResult> DeleteStudent([FromRoute] string indexNumber)
    {
        var response = await _statementOfAccountServices.DeleteStatementOfAccountAsync(indexNumber);
        return StatusCode(response.Code, response);
    }

}