using Microsoft.AspNetCore.Mvc;
using Teacher.Management.Api.Model.Filter;
using Teacher.Management.Api.Model.RequestModel;
using Teacher.Management.Api.Services;

namespace Teacher.Management.Api.Controller;
[ApiController]
[Route("api/[controller]")]
public class TeacherController:ControllerBase
{
    private readonly ITeacherServices _teacherServices;

    public TeacherController(ITeacherServices teacherServices)
    {
        _teacherServices = teacherServices;
    }
    
    /// <summary>
    /// Register teacher
    /// </summary>
    /// <param name="requestModel"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> RegisterTeacher([FromBody] TeacherRequestModel requestModel)
    {
        var response = await _teacherServices.RegisterTeacherAsync(requestModel);
        return StatusCode(response.Code, response);
    }

    /// <summary>
    /// Login teacher
    /// </summary>
    /// <param name="loginModel"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<IActionResult> LoginTeacher([FromBody] TeacherLoginModel loginModel)
    {
        var response = await _teacherServices.LoginTeacherAsync(loginModel);
        return StatusCode(response.Code, response);
    }

    /// <summary>
    /// Filter teachers
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpGet()]
    public async Task<IActionResult> GetTeachers([FromQuery] TeacherFilter filter)
    {
        var response = await _teacherServices.GetTeachersAsync(filter);
        return StatusCode(response.Code, response);
    }
    
    /// <summary>
    /// Retrieve a teacher
    /// </summary>
    /// <param name="teacherId"></param>
    /// <returns></returns>
    [HttpGet("{teacherId:required}")]
    public async Task<IActionResult> GetTeacherById([FromRoute] string teacherId)
    {
        var response = await _teacherServices.GetTeacherByIdAsync(teacherId);
        return StatusCode(response.Code, response);
    }

    /// <summary>
    /// Update a teacher
    /// </summary>
    /// <param name="teacherId"></param>
    /// <param name="updateModel"></param>
    /// <returns></returns>
    [HttpPut("{teacherId:required}")]
    public async Task<IActionResult> UpdateTeacher([FromRoute] string teacherId,
        [FromBody] TeacherUpdateModel updateModel)
    {
        var response = await _teacherServices.UpdateTeacherAsync(teacherId, updateModel);
        return StatusCode(response.Code, response);
    }

    /// <summary>
    /// Delete a teacher
    /// </summary>
    /// <param name="teacherId"></param>
    /// <returns></returns>
    [HttpDelete("{teacherId:required}")]
    public async Task<IActionResult> DeleteTeacher([FromRoute] string teacherId)
    {
        var response = await _teacherServices.DeleteTeacherAsync(teacherId);
        return StatusCode(response.Code, response);
    }

}