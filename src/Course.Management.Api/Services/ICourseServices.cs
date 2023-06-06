using Course.Management.Api.Model.Filter;
using Course.Management.Api.Model.RequestModel;
using Course.Management.Api.Model.Response;
using Course.Management.Api.Model.ResponseModel;

namespace Course.Management.Api.Services;

public interface ICourseServices
{
    Task<BaseResponse<CourseResponseModel>> CreateCourseAsync(CourseRequestModel courseRequestModel);
    Task<BaseResponse<PaginatedResponse<CourseResponseModel>>> GetCoursesAsync(CourseFilter  courseFilter);
    Task<BaseResponse<CourseResponseModel>> GetCourseByIdAsync(string courseId);
    Task<BaseResponse<CourseResponseModel>> UpdateCourseAsync(string courseId, CourseUpdateModel courseUpdateModel);
    Task<BaseResponse<EmptyResponse>> DeleteCourseAsync(string courseId);

}