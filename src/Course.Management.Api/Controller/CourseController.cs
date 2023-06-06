using Course.Management.Api.Model.Filter;
using Course.Management.Api.Model.RequestModel;
using Course.Management.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Course.Management.Api.Controller;
[ApiController]
[Route("api/[controller]")]
public class CourseController:ControllerBase
{
    private readonly ICourseServices _courseServices;

    public CourseController(ICourseServices courseServices)
    {
        _courseServices = courseServices;
    }
    
    /// <summary>
    /// Add course
    /// </summary>
    /// <param name="requestModel"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CourseRequestModel requestModel)
    {
        var response = await _courseServices.CreateCourseAsync(requestModel);
        return StatusCode(response.Code, response);
    }
    
    /// <summary>
    /// Filter courses
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpGet()]
    public async Task<IActionResult> GetCourses([FromQuery] CourseFilter filter)
    {
        var response = await _courseServices.GetCoursesAsync(filter);
        return StatusCode(response.Code, response);
    }
    
    /// <summary>
    /// Retrieve a course
    /// </summary>
    /// <param name="courseId"></param>
    /// <returns></returns>
    [HttpGet("{courseId:required}")]
    public async Task<IActionResult> GetCourseById([FromRoute] string courseId)
    {
        var response = await _courseServices.GetCourseByIdAsync(courseId);
        return StatusCode(response.Code, response);
    }

    /// <summary>
    /// Update a course
    /// </summary>
    /// <param name="courseId"></param>
    /// <param name="updateModel"></param>
    /// <returns></returns>
    [HttpPut("{courseId:required}")]
    public async Task<IActionResult> UpdateCourse([FromRoute] string courseId,
        [FromBody] CourseUpdateModel updateModel)
    {
        var response = await _courseServices.UpdateCourseAsync(courseId, updateModel);
        return StatusCode(response.Code, response);
    }

    /// <summary>
    /// Delete a course
    /// </summary>
    /// <param name="courseId"></param>
    /// <returns></returns>
    [HttpDelete("{courseId:required}")]
    public async Task<IActionResult> DeleteCourse([FromRoute] string courseId)
    {
        var response = await _courseServices.DeleteCourseAsync(courseId);
        return StatusCode(response.Code, response);
    }

}