using GradeAndAssessment.Api.Model.Filter;
using GradeAndAssessment.Api.Model.RequestModel;
using GradeAndAssessment.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GradeAndAssessment.Api.Controller;
[ApiController]
[Route("api/[controller]")]
public class StudentAssessmentController:ControllerBase
{
    private readonly IStudentAssessmentServices _studentAssessmentServices;

    public StudentAssessmentController(IStudentAssessmentServices studentAssessmentServices)
    {
        _studentAssessmentServices = studentAssessmentServices;
    }
    
    /// <summary>
    /// Add a new student assessment
    /// </summary>
    /// <param name="requestModel"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateStudentAssessment([FromBody] StudentAssessmentRequestModel requestModel)
    {
        var response = await _studentAssessmentServices.CreateStudentAssessmentAsync(requestModel);
        return StatusCode(response.Code, response);
    }

    /// <summary>
    /// Login student assessment
    /// </summary>
    /// <param name="loginModel"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<IActionResult> LoginStudentAssessment([FromBody] StudentAssessmentLoginModel loginModel)
    {
        var response = await _studentAssessmentServices.LoginStudentAssessmentAsync(loginModel);
        return StatusCode(response.Code, response);
    }

    /// <summary>
    /// Filter student assessment
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpGet()]
    public async Task<IActionResult> GetStudentAssessments([FromQuery] StudentAssessmentFilter filter)
    {
        var response = await _studentAssessmentServices.GetStudentAssessmentsAsync(filter);
        return StatusCode(response.Code, response);
    }
    
    /// <summary>
    /// Retrieve a student assessment
    /// </summary>
    /// <param name="studentId"></param>
    /// <returns></returns>
    [HttpGet("{studentId:required}")]
    public async Task<IActionResult> GetStudentAssessmentById([FromRoute] string studentId)
    {
        var response = await _studentAssessmentServices.GetStudentAssessmentByIdAsync(studentId);
        return StatusCode(response.Code, response);
    }

    /// <summary>
    /// Update student assessment
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="updateModel"></param>
    /// <returns></returns>
    [HttpPut("{studentId:required}")]
    public async Task<IActionResult> UpdateStudentAssessment([FromRoute] string studentId,
        [FromBody] StudentAssessmentUpdateModel updateModel)
    {
        var response = await _studentAssessmentServices.UpdateStudentAssessmentAsync(studentId, updateModel);
        return StatusCode(response.Code, response);
    }

    /// <summary>
    /// Delete a student assessment
    /// </summary>
    /// <param name="studentId"></param>
    /// <returns></returns>
    [HttpDelete("{studentId:required}")]
    public async Task<IActionResult> DeleteStudentAssessment([FromRoute] string studentId)
    {
        var response = await _studentAssessmentServices.DeleteStudentAssessmentAsync(studentId);
        return StatusCode(response.Code, response);
    }

}