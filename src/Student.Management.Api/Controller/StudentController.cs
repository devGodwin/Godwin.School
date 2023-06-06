using Microsoft.AspNetCore.Mvc;
using Student.Management.Api.Model.Filter;
using Student.Management.Api.Model.RequestModel;
using Student.Management.Api.Services;

namespace Student.Management.Api.Controller;
[ApiController]
[Route("api/[controller]")]
public class StudentController:ControllerBase
{
    private readonly IStudentServices _studentServices;

    public StudentController(IStudentServices studentServices)
    {
        _studentServices = studentServices;
    }
    
    /// <summary>
    /// Register student
    /// </summary>
    /// <param name="requestModel"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> RegisterStudent([FromBody] StudentRequestModel requestModel)
    {
        var response = await _studentServices.RegisterStudentAsync(requestModel);
        return StatusCode(response.Code, response);
    }

    /// <summary>
    /// Login student
    /// </summary>
    /// <param name="loginModel"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<IActionResult> LoginStudent([FromBody] StudentLoginModel loginModel)
    {
        var response = await _studentServices.LoginStudentAsync(loginModel);
        return StatusCode(response.Code, response);
    }

    /// <summary>
    /// Filter students
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpGet()]
    public async Task<IActionResult> GetStudents([FromQuery] StudentFilter filter)
    {
        var response = await _studentServices.GetStudentsAsync(filter);
        return StatusCode(response.Code, response);
    }
    
    /// <summary>
    /// Retrieve a student
    /// </summary>
    /// <param name="indexNumber"></param>
    /// <returns></returns>
    [HttpGet("{indexNumber:required}")]
    public async Task<IActionResult> GetStudentById([FromRoute] string indexNumber)
    {
        var response = await _studentServices.GetStudentByIdAsync(indexNumber);
        return StatusCode(response.Code, response);
    }

    /// <summary>
    /// Update a student
    /// </summary>
    /// <param name="indexNumber"></param>
    /// <param name="updateModel"></param>
    /// <returns></returns>
    [HttpPut("{indexNumber:required}")]
    public async Task<IActionResult> UpdateStudent([FromRoute] string indexNumber,
        [FromBody] StudentUpdateModel updateModel)
    {
        var response = await _studentServices.UpdateStudentAsync(indexNumber, updateModel);
        return StatusCode(response.Code, response);
    }

    /// <summary>
    /// Delete a student
    /// </summary>
    /// <param name="indexNumber"></param>
    /// <returns></returns>
    [HttpDelete("{indexNumber:required}")]
    public async Task<IActionResult> DeleteStudent([FromRoute] string indexNumber)
    {
        var response = await _studentServices.DeleteStudentAsync(indexNumber);
        return StatusCode(response.Code, response);
    }

}