using GradeAndAssessment.Api.Model.Filter;
using GradeAndAssessment.Api.Model.RequestModel;
using GradeAndAssessment.Api.Model.Response;
using GradeAndAssessment.Api.Model.ResponseModel;

namespace GradeAndAssessment.Api.Services;

public interface IStudentAssessmentServices
{
    Task<BaseResponse<StudentAssessmentResponseModel >> CreateStudentAssessmentAsync(StudentAssessmentRequestModel assessmentRequestModel);
    Task<BaseResponse<StudentAssessmentResponseModel>> LoginStudentAssessmentAsync(StudentAssessmentLoginModel assessmentLoginModel);
    Task<BaseResponse<PaginatedResponse<StudentAssessmentResponseModel>>> GetStudentAssessmentsAsync(StudentAssessmentFilter assessmentFilter);
    Task<BaseResponse<StudentAssessmentResponseModel>> GetStudentAssessmentByIdAsync(string indexNumber);
    Task<BaseResponse<StudentAssessmentResponseModel>> UpdateStudentAssessmentAsync(string indexNumber, StudentAssessmentUpdateModel assessmentUpdateModel);
    Task<BaseResponse<EmptyResponse>> DeleteStudentAssessmentAsync(string indexNumber);

}