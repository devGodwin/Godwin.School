using Finance.Management.Api.Model.RequestModel;
using Finance.Management.Api.Services.AuthServices;
using Microsoft.AspNetCore.Mvc;

namespace Finance.Management.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController: ControllerBase
{
    private readonly IAuthServices _authServices;
    
    public AuthController(IAuthServices authServices)
    {
        _authServices = authServices;
    }
    
    /// <summary>
    /// Add student statement of account
    /// </summary>
    /// <param name="requestModel"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateStatementOfAccount([FromBody] StatementOfAccountRequestModel requestModel)
    {
        var response = await _authServices.CreateStatementOfAccountAsync(requestModel);
        return StatusCode(response.Code, response);
    }

    /// <summary>
    /// Login Admin
    /// </summary>
    /// <param name="loginModel"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<IActionResult> LoginStatementOfAccount([FromBody] StatementOfAccountLoginModel loginModel)
    {
        var response = await _authServices.LoginStatementOfAccountAsync(loginModel);
        return StatusCode(response.Code, response);
    }
}