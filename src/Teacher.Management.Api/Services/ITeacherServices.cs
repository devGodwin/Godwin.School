using Teacher.Management.Api.Model.Filter;
using Teacher.Management.Api.Model.RequestModel;
using Teacher.Management.Api.Model.Response;
using Teacher.Management.Api.Model.ResponseModel;

namespace Teacher.Management.Api.Services;

public interface ITeacherServices
{
    Task<BaseResponse<TeacherResponseModel>> RegisterTeacherAsync(TeacherRequestModel teacherRequestModel);
    Task<BaseResponse<TeacherResponseModel>> LoginTeacherAsync(TeacherLoginModel loginModel);
    Task<BaseResponse<PaginatedResponse<TeacherResponseModel>>> GetTeachersAsync(TeacherFilter  teacherFilter);
    Task<BaseResponse<TeacherResponseModel>> GetTeacherByIdAsync(string teacherId);
    Task<BaseResponse<TeacherResponseModel>> UpdateTeacherAsync(string teacherId, TeacherUpdateModel teacherUpdateModel);
    Task<BaseResponse<EmptyResponse>> DeleteTeacherAsync(string teacherId);

}