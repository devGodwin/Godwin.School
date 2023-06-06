using Student.Management.Api.Model.Filter;
using Student.Management.Api.Model.RequestModel;
using Student.Management.Api.Model.Response;
using Student.Management.Api.Model.ResponseModel;

namespace Student.Management.Api.Services;

public interface IStudentServices
{
    Task<BaseResponse<StudentResponseModel>> RegisterStudentAsync(StudentRequestModel studentRequestModel);
    Task<BaseResponse<StudentResponseModel>> LoginStudentAsync(StudentLoginModel loginModel);
    Task<BaseResponse<PaginatedResponse<StudentResponseModel>>> GetStudentsAsync(StudentFilter studentFilter);
    Task<BaseResponse<StudentResponseModel>> GetStudentByIdAsync(string indexNumber);
    Task<BaseResponse<StudentResponseModel>> UpdateStudentAsync(string indexNumber, StudentUpdateModel studentUpdateModel);
    Task<BaseResponse<EmptyResponse>> DeleteStudentAsync(string indexNumber);

}