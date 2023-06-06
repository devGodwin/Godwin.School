using System.Net;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Student.Management.Api.Data;
using Student.Management.Api.Helper;
using Student.Management.Api.Model;
using Student.Management.Api.Model.Filter;
using Student.Management.Api.Model.RequestModel;
using Student.Management.Api.Model.Response;
using Student.Management.Api.Model.ResponseModel;
using Student.Management.Api.Redis;

namespace Student.Management.Api.Services;

public class StudentServices:IStudentServices
{
    private readonly StudentContext _studentContext;
    private readonly ILogger<StudentServices> _logger;
    private readonly IMapper _mapper;
    private readonly IRedisService _redisService;

    public StudentServices(StudentContext studentContext,ILogger<StudentServices>logger,IMapper mapper,IRedisService redisService)
    {
        _studentContext = studentContext;
        _logger = logger;
        _mapper = mapper;
        _redisService = redisService;
    }
    public async Task<BaseResponse<StudentResponseModel>> RegisterStudentAsync(StudentRequestModel studentRequestModel)
    {
        try
        {
            var studentExist = await _studentContext.Students.AnyAsync(x => x.EmailAddress.Equals(studentRequestModel.EmailAddress));
            if (studentExist)
            {
                return CommonResponses.ErrorResponse.ConflictErrorResponse<StudentResponseModel>("Student is already registered");
            }

            var dbCounterExist = await _studentContext.Students.AnyAsync(x => x.DbCounter.Equals(studentRequestModel.DbCounter));
            if (dbCounterExist)
            {
                return CommonResponses.ErrorResponse.ConflictErrorResponse<StudentResponseModel>(
                    "DbCounter already exist");
            }

            Authentication.CreatePasswordHash(studentRequestModel.Password,out byte[] passwordHash, out byte[] passwordSalt);
            
            var newStudent = _mapper.Map<Data.Student>(studentRequestModel);
            newStudent.PasswordHash = passwordHash;
            newStudent.PasswordSalt = passwordSalt;

           newStudent.IndexNumber = StudentIndexNumberGenerator.GenerateStudentIndexNumber(newStudent);

            await _studentContext.Students.AddAsync(newStudent);
            var rows = await _studentContext.SaveChangesAsync();
            if (rows < 1)
            {
                CommonResponses.ErrorResponse.FailedDependencyErrorResponse<StudentResponseModel>();
            }

            await _redisService.CacheNewStudentAsync(_mapper.Map<CachedStudent>(newStudent));

            return CommonResponses.SuccessResponse.CreatedResponse(_mapper.Map<StudentResponseModel>(newStudent));
        }
        catch (Exception e)
        {
           _logger.LogError(e,"An error occured registering student\n{studentRequestModel}",JsonConvert.SerializeObject(studentRequestModel,Formatting.Indented));

           return CommonResponses.ErrorResponse.InternalServerErrorResponse<StudentResponseModel>();
        }
    }

    public async Task<BaseResponse<StudentResponseModel>> LoginStudentAsync(StudentLoginModel loginModel)
    {
        try
        {
            var studentExist = await _studentContext.Students.FirstOrDefaultAsync(x => x.IndexNumber.Equals(loginModel.IndexNumber));
            if (studentExist == null)
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<StudentResponseModel>("Index number is incorrect");
            }

            if (!Authentication.VerifyPasswordHash(loginModel.Password, studentExist.PasswordHash, studentExist.PasswordSalt))
            {
                return CommonResponses.ErrorResponse.ConflictErrorResponse<StudentResponseModel>("Password is incorrect");
            }
            
            return CommonResponses.SuccessResponse.OkResponse(_mapper.Map<StudentResponseModel>(studentExist),
                "Login successfully");
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured logging in student\n{loginModel}",JsonConvert.SerializeObject(loginModel,Formatting.Indented));

            return CommonResponses.ErrorResponse.InternalServerErrorResponse<StudentResponseModel>();
        }
    }

    public async Task<BaseResponse<PaginatedResponse<StudentResponseModel>>> GetStudentsAsync(StudentFilter studentFilter)
    {
        try
        {
            var studentQueryable = _studentContext.Students.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(studentFilter.IndexNumber))
            {
                studentQueryable = studentQueryable.Where(x => x.IndexNumber.Equals(studentFilter.IndexNumber));
            }

            if (!string.IsNullOrEmpty(studentFilter.FirstName))
            {
                studentQueryable = studentQueryable.Where(x => x.FirstName.Equals(studentFilter.FirstName));
            }

            if (!string.IsNullOrEmpty(studentFilter.LastName))
            {
                studentQueryable = studentQueryable.Where(x => x.LastName.Equals(studentFilter.LastName));
            }

            if (!string.IsNullOrEmpty(studentFilter.DateOfBirth))
            {
                studentQueryable = studentQueryable.Where(x => x.DateOfBirth.Equals(studentFilter.DateOfBirth));
            }

            if (!string.IsNullOrEmpty(studentFilter.Address))
            {
                studentQueryable = studentQueryable.Where(x => x.Address.Equals(studentFilter.Address));
            }

            if (!string.IsNullOrEmpty(studentFilter.ContactNumber))
            {
                studentQueryable = studentQueryable.Where(x => x.ContactNumber.Equals(studentFilter.ContactNumber));
            }

            if (!string.IsNullOrEmpty(studentFilter.EmailAddress))
            {
                studentQueryable = studentQueryable.Where(x => x.EmailAddress.Equals(studentFilter.EmailAddress));
            }

            if (!string.IsNullOrEmpty(studentFilter.EmergencyContactName))
            {
                studentQueryable =
                    studentQueryable.Where(x => x.EmergencyContactName.Equals(studentFilter.EmergencyContactName));
            }

            if (!string.IsNullOrEmpty(studentFilter.EmergencyContactNumber))
            {
                studentQueryable =
                    studentQueryable.Where(x => x.EmergencyContactNumber.Equals(studentFilter.EmergencyContactNumber));
            }

            studentQueryable = "desc".Equals(studentFilter.OrderBy, StringComparison.OrdinalIgnoreCase)
                ? studentQueryable.OrderByDescending(x => x.CreatedAt)
                : studentQueryable.OrderBy(x=>x.CreatedAt);

            var paginatedResponse = await 
                studentQueryable.ToPagedListAsync(studentFilter.CurrentPage - 1, studentFilter.PageSize);

            return new BaseResponse<PaginatedResponse<StudentResponseModel>>()
            {
                Code = (int)HttpStatusCode.OK,
                Message = "Retrieved successfully",
                Data = new PaginatedResponse<StudentResponseModel>()
                {
                    CurrentPage = studentFilter.CurrentPage,
                    TotalPages = paginatedResponse.TotalPages,
                    PageSize = studentFilter.PageSize,
                    TotalRecords = paginatedResponse.TotalCount,
                    Data = paginatedResponse.Items.Select(x => _mapper.Map<StudentResponseModel>(x)).ToList()
                }
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured getting students\n{studentFilter}",JsonConvert.SerializeObject(studentFilter,Formatting.Indented));
          
            return CommonResponses.ErrorResponse.FailedDependencyErrorResponse<PaginatedResponse<StudentResponseModel>>();
        }
    }

    public async Task<BaseResponse<StudentResponseModel>> GetStudentByIdAsync(string indexNumber)
    {
        try
        {
            var studentExist = await _studentContext.Students.FirstOrDefaultAsync(x => x.IndexNumber.Equals(indexNumber));
            if (studentExist == null)
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<StudentResponseModel>("Index number not found");
            }

            await _redisService.GetStudentAsync(indexNumber);
            
            return CommonResponses.SuccessResponse.OkResponse(_mapper.Map<StudentResponseModel>(studentExist));
        }
        catch (Exception e)
        {
           _logger.LogError(e,"An error occured getting student by index number:{indexNumber}",indexNumber);
          
           return CommonResponses.ErrorResponse.InternalServerErrorResponse<StudentResponseModel>();
        }
    }

    public async Task<BaseResponse<StudentResponseModel>> UpdateStudentAsync(string indexNumber,
        StudentUpdateModel studentUpdateModel)
    {
        try
        {
            var studentExist = await _studentContext.Students.FirstOrDefaultAsync(x => x.IndexNumber.Equals(indexNumber));
            if (studentExist == null)
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<StudentResponseModel>("Index number not found");
            }

            var updateStudent = _mapper.Map(studentUpdateModel, studentExist);
            _studentContext.Students.Update(updateStudent);
            var rows = await _studentContext.SaveChangesAsync();
            if (rows < 1)
            {
                CommonResponses.ErrorResponse.FailedDependencyErrorResponse<StudentUpdateModel>();
            }
            
            return CommonResponses.SuccessResponse.CreatedResponse(_mapper.Map<StudentResponseModel>(updateStudent));
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured updating student\n{studentUpdateModel}",JsonConvert.SerializeObject(studentUpdateModel,Formatting.Indented));

            return CommonResponses.ErrorResponse.InternalServerErrorResponse<StudentResponseModel>();
        }
    }

    public async Task<BaseResponse<EmptyResponse>> DeleteStudentAsync(string indexNumber)
    {
        try
        {
            var studentExist = await _studentContext.Students.FirstOrDefaultAsync(x => x.IndexNumber.Equals(indexNumber));
            if (studentExist == null)
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<EmptyResponse>("Index number not found");
            }

            _studentContext.Students.Remove(studentExist);
            var rows = await _studentContext.SaveChangesAsync();
            if (rows < 1)
            {
                return CommonResponses.ErrorResponse.FailedDependencyErrorResponse<EmptyResponse>();
            }

            await _redisService.DeleteStudentAsync(indexNumber);

            return CommonResponses.SuccessResponse.DeletedResponse();
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured deleting student by index number:{indexNumber}",indexNumber);

            return CommonResponses.ErrorResponse.InternalServerErrorResponse<EmptyResponse>();
        }
    }
}