using System.Net;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using GradeAndAssessment.Api.Data;
using GradeAndAssessment.Api.Helper;
using GradeAndAssessment.Api.Model;
using GradeAndAssessment.Api.Model.Filter;
using GradeAndAssessment.Api.Model.RequestModel;
using GradeAndAssessment.Api.Model.Response;
using GradeAndAssessment.Api.Model.ResponseModel;
using GradeAndAssessment.Api.Redis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GradeAndAssessment.Api.Services;

public class StudentAssessmentServices:IStudentAssessmentServices
{
    private readonly StudentAssessmentContext _studentAssessmentContext;
    private readonly ILogger<StudentAssessmentServices> _logger;
    private readonly IMapper _mapper;
    private readonly IRedisService _redisService;

    public StudentAssessmentServices(StudentAssessmentContext studentAssessmentContext,ILogger<StudentAssessmentServices>logger,
        IMapper mapper,IRedisService redisService)
    {
        _studentAssessmentContext = studentAssessmentContext;
        _logger = logger;
        _mapper = mapper;
        _redisService = redisService;
    }
    public async Task<BaseResponse<StudentAssessmentResponseModel>> CreateStudentAssessmentAsync(StudentAssessmentRequestModel assessmentRequestModel)
    {
        try
        {
            var studentAssessmentExist = await _studentAssessmentContext.StudentAssessments.AnyAsync(x => x.IndexNumber.Equals(assessmentRequestModel.IndexNumber));
            if (studentAssessmentExist)
            {
                return CommonResponses.ErrorResponse.ConflictErrorResponse<StudentAssessmentResponseModel>("Student assessment is already added");
            }

            Authentication.CreatePasswordHash(assessmentRequestModel.Password,out byte[] passwordHash,out byte[] passwordSalt);
            
            var newStudentStudentAssessment = _mapper.Map<StudentAssessment>(assessmentRequestModel);

            newStudentStudentAssessment.Score = newStudentStudentAssessment.Assignment
                                                + newStudentStudentAssessment.Project
                                                + newStudentStudentAssessment.MidsemExam
                                                + newStudentStudentAssessment.EndOfSemExam;

            newStudentStudentAssessment.PasswordHash = passwordHash;
            newStudentStudentAssessment.PasswordSalt = passwordSalt;
            
            await _studentAssessmentContext.StudentAssessments.AddAsync(newStudentStudentAssessment);
            var rows = await _studentAssessmentContext.SaveChangesAsync();
            if (rows < 1)
            {
                CommonResponses.ErrorResponse.FailedDependencyErrorResponse<StudentAssessmentResponseModel>();
            }

            await _redisService.CacheNewStudentAssessmentAsync(
                _mapper.Map<CachedStudentAssessment>(newStudentStudentAssessment));
            
            return CommonResponses.SuccessResponse.CreatedResponse(_mapper.Map<StudentAssessmentResponseModel>(newStudentStudentAssessment));
        }
        catch (Exception e)
        {
           _logger.LogError(e,"An error occured adding student assessment\n{assessmentRequestModel}",JsonConvert.SerializeObject(assessmentRequestModel,Formatting.Indented));

           return CommonResponses.ErrorResponse.InternalServerErrorResponse<StudentAssessmentResponseModel>();
        }
    }

    public async Task<BaseResponse<StudentAssessmentResponseModel>> LoginStudentAssessmentAsync(StudentAssessmentLoginModel loginModel)
    {
        try
        {
            var assessmentIdExist = await _studentAssessmentContext.StudentAssessments.FirstOrDefaultAsync(x => x.AssessmentId.Equals(loginModel.AssessmentId));
            if (assessmentIdExist == null)
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<StudentAssessmentResponseModel>("Assessment Id is incorrect");
            }
            
            if (!Authentication.VerifyPasswordHash(loginModel.Password,assessmentIdExist.PasswordHash,assessmentIdExist.PasswordSalt))
            {
                return CommonResponses.ErrorResponse.ConflictErrorResponse<StudentAssessmentResponseModel>(
                    "Password is incorrect");
            }
            
            return CommonResponses.SuccessResponse.OkResponse(_mapper.Map<StudentAssessmentResponseModel>(assessmentIdExist), "Login successfully");
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured logging in student assessment\n{loginModel}",JsonConvert.SerializeObject(loginModel,Formatting.Indented));

            return CommonResponses.ErrorResponse.InternalServerErrorResponse<StudentAssessmentResponseModel>();
        }
    }

    public async Task<BaseResponse<PaginatedResponse<StudentAssessmentResponseModel>>> GetStudentAssessmentsAsync(StudentAssessmentFilter assessmentFilter)
    {
        try
        {
            var studentAssessmentQueryable = _studentAssessmentContext.StudentAssessments.AsNoTracking().AsQueryable();
            
            if (!string.IsNullOrEmpty(assessmentFilter.AssessmentId))
            {
                studentAssessmentQueryable = studentAssessmentQueryable.Where(x => x.AssessmentId.Equals(assessmentFilter.AssessmentId));
            }

            if (!string.IsNullOrEmpty(assessmentFilter.IndexNumber))
            {
                studentAssessmentQueryable = studentAssessmentQueryable.Where(x => x.IndexNumber.Equals(assessmentFilter.IndexNumber));
            }
            
            if (!string.IsNullOrEmpty(assessmentFilter.CourseId))
            {
                studentAssessmentQueryable = studentAssessmentQueryable.Where(x => x.CourseId.Equals(assessmentFilter.CourseId));
            }

            studentAssessmentQueryable = "desc".Equals(assessmentFilter.OrderBy, StringComparison.OrdinalIgnoreCase)
                ? studentAssessmentQueryable.OrderByDescending(x => x.CreatedAt)
                : studentAssessmentQueryable.OrderBy(x=>x.CreatedAt);

            var paginatedResponse = await 
                studentAssessmentQueryable.ToPagedListAsync(assessmentFilter.CurrentPage - 1, assessmentFilter.PageSize);

            return new BaseResponse<PaginatedResponse<StudentAssessmentResponseModel>>()
            {
                Code = (int)HttpStatusCode.OK,
                Message = "Retrieved successfully",
                Data = new PaginatedResponse<StudentAssessmentResponseModel>()
                {
                    CurrentPage = assessmentFilter.CurrentPage,
                    TotalPages = paginatedResponse.TotalPages,
                    PageSize = assessmentFilter.PageSize,
                    TotalRecords = paginatedResponse.TotalCount,
                    Data = paginatedResponse.Items.Select(x => _mapper.Map<StudentAssessmentResponseModel>(x)).ToList()
                }
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured getting students\n{studentFilter}",JsonConvert.SerializeObject(assessmentFilter,Formatting.Indented));
          
            return CommonResponses.ErrorResponse.FailedDependencyErrorResponse<PaginatedResponse<StudentAssessmentResponseModel>>();
        }
    }

    public async Task<BaseResponse<StudentAssessmentResponseModel>> GetStudentAssessmentByIdAsync(string indexNumber)
    {
        try
        {
            var assessmentExist = await _studentAssessmentContext.StudentAssessments.FirstOrDefaultAsync(x => x.IndexNumber.Equals(indexNumber));
            if (assessmentExist == null)
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<StudentAssessmentResponseModel>("Student assessment not found");
            }

            await _redisService.GetStudentAssessmentAsync(indexNumber);
            
            return CommonResponses.SuccessResponse.OkResponse(_mapper.Map<StudentAssessmentResponseModel>(assessmentExist));
        }
        catch (Exception e)
        {
           _logger.LogError(e,"An error occured getting student assessment by student index number:{indexNumber}",indexNumber);
          
           return CommonResponses.ErrorResponse.InternalServerErrorResponse<StudentAssessmentResponseModel>();
        }
    }

    public async Task<BaseResponse<StudentAssessmentResponseModel>> UpdateStudentAssessmentAsync(string indexNumber,
        StudentAssessmentUpdateModel assessmentUpdateModel)
    {
        try
        {
            var assessmentExist = await _studentAssessmentContext.StudentAssessments.FirstOrDefaultAsync(x => x.IndexNumber.Equals(indexNumber));
            if (assessmentExist == null)
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<StudentAssessmentResponseModel>("Student assessment not found");
            }

            var updateAssessment = _mapper.Map(assessmentUpdateModel, assessmentExist);
            
            updateAssessment.Score = updateAssessment.Assignment 
                                     + updateAssessment.Project 
                                     + updateAssessment.MidsemExam
                                     + updateAssessment.EndOfSemExam;
            
            _studentAssessmentContext.StudentAssessments.Update(updateAssessment);
            
            var rows = await _studentAssessmentContext.SaveChangesAsync();
            if (rows < 1)
            {
                CommonResponses.ErrorResponse.FailedDependencyErrorResponse<StudentAssessmentResponseModel>();
            }

            return CommonResponses.SuccessResponse.CreatedResponse(_mapper.Map<StudentAssessmentResponseModel>(updateAssessment));
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured updating student assessment\n{assessmentUpdateModel}",JsonConvert.SerializeObject(assessmentUpdateModel,Formatting.Indented));

            return CommonResponses.ErrorResponse.InternalServerErrorResponse<StudentAssessmentResponseModel>();
        }
    }

    public async Task<BaseResponse<EmptyResponse>> DeleteStudentAssessmentAsync(string indexNumber)
    {
        try
        {
            var assessmentExist = await _studentAssessmentContext.StudentAssessments.FirstOrDefaultAsync(x => x.IndexNumber.Equals(indexNumber));
            if (assessmentExist == null)
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<EmptyResponse>("Student assessment not found");
            }

            _studentAssessmentContext.StudentAssessments.Remove(assessmentExist);
        
            var rows = await _studentAssessmentContext.SaveChangesAsync();
            if (rows < 1)
            {
                return CommonResponses.ErrorResponse.FailedDependencyErrorResponse<EmptyResponse>();
            }

            await _redisService.DeleteStudentAssessmentAsync(indexNumber);

            return CommonResponses.SuccessResponse.DeletedResponse();
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured deleting student assessment by index number\n{indexNumber}",indexNumber);

            return CommonResponses.ErrorResponse.InternalServerErrorResponse<EmptyResponse>();
        }
    }
}