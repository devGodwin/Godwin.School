using System.Net;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Teacher.Management.Api.Data;
using Teacher.Management.Api.Helper;
using Teacher.Management.Api.Model;
using Teacher.Management.Api.Model.Filter;
using Teacher.Management.Api.Model.RequestModel;
using Teacher.Management.Api.Model.Response;
using Teacher.Management.Api.Model.ResponseModel;
using Teacher.Management.Api.Redis;

namespace Teacher.Management.Api.Services;

public class TeacherServices:ITeacherServices
{
    private readonly TeacherContext _teacherContext;
    private readonly ILogger<TeacherServices> _logger;
    private readonly IMapper _mapper;
    private readonly IRedisService _redisService;

    public TeacherServices(TeacherContext teacherContext,ILogger<TeacherServices>logger,IMapper mapper,IRedisService redisService)
    {
        _teacherContext = teacherContext;
        _logger = logger;
        _mapper = mapper;
        _redisService = redisService;
    }
    public async Task<BaseResponse<TeacherResponseModel>> RegisterTeacherAsync(TeacherRequestModel teacherRequestModel)
    {
        try
        {
            var teacherExist = await _teacherContext.Teachers.AnyAsync(x => x.EmailAddress.Equals(teacherRequestModel.EmailAddress));
            if (teacherExist)
            {
                return CommonResponses.ErrorResponse.ConflictErrorResponse<TeacherResponseModel>("Teacher is already registered");
            }

            Authentication.CreatePasswordHash(teacherRequestModel.Password,out byte[] passwordHash, out byte[] passwordSalt);
            
            var newTeacher = _mapper.Map<Data.Teacher>(teacherRequestModel);
            newTeacher.PasswordHash = passwordHash;
            newTeacher.PasswordSalt = passwordSalt;
            
            await _teacherContext.Teachers.AddAsync(newTeacher);
            var rows = await _teacherContext.SaveChangesAsync();
            if (rows < 1)
            {
                CommonResponses.ErrorResponse.FailedDependencyErrorResponse<TeacherResponseModel>();
            }

            await _redisService.CacheNewTeacherAsync(_mapper.Map<CachedTeacher>(newTeacher));
            
            return CommonResponses.SuccessResponse.CreatedResponse(_mapper.Map<TeacherResponseModel>(newTeacher));
        }
        catch (Exception e)
        {
           _logger.LogError(e,"An error occured registering teacher\n{teacherRequestModel}",JsonConvert.SerializeObject(teacherRequestModel,Formatting.Indented));

           return CommonResponses.ErrorResponse.InternalServerErrorResponse<TeacherResponseModel>();
        }
    }

    public async Task<BaseResponse<TeacherResponseModel>> LoginTeacherAsync(TeacherLoginModel loginModel)
    {
        try
        {
            var teacherExist = await _teacherContext.Teachers.FirstOrDefaultAsync(x => x.TeacherId.Equals(loginModel.TeacherId));
            if (teacherExist == null)
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<TeacherResponseModel>("Teacher Id is incorrect");
            }

            if (!Authentication.VerifyPasswordHash(loginModel.Password, teacherExist.PasswordHash, teacherExist.PasswordSalt))
            {
                return CommonResponses.ErrorResponse.ConflictErrorResponse<TeacherResponseModel>(
                    "Password is incorrect");
            }
            
        
            return CommonResponses.SuccessResponse.OkResponse(_mapper.Map<TeacherResponseModel>(teacherExist),
                "Login successfully");
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured logging in teacher\n{loginModel}",JsonConvert.SerializeObject(loginModel,Formatting.Indented));

            return CommonResponses.ErrorResponse.InternalServerErrorResponse<TeacherResponseModel>();
        }
    }

    public async Task<BaseResponse<PaginatedResponse<TeacherResponseModel>>> GetTeachersAsync(TeacherFilter teacherFilter)
    {
        try
        {
            var teacherQueryable = _teacherContext.Teachers.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(teacherFilter.TeacherId))
            {
                teacherQueryable = teacherQueryable.Where(x => x.TeacherId.Equals(teacherFilter.TeacherId));
            }

            if (!string.IsNullOrEmpty(teacherFilter.FirstName))
            {
                teacherQueryable = teacherQueryable.Where(x => x.FirstName.Equals(teacherFilter.FirstName));
            }

            if (!string.IsNullOrEmpty(teacherFilter.LastName))
            {
                teacherQueryable = teacherQueryable.Where(x => x.LastName.Equals(teacherFilter.LastName));
            }

            if (!string.IsNullOrEmpty(teacherFilter.DateOfBirth))
            {
                teacherQueryable = teacherQueryable.Where(x => x.DateOfBirth.Equals(teacherFilter.DateOfBirth));
            }

            if (!string.IsNullOrEmpty(teacherFilter.Address))
            {
                teacherQueryable = teacherQueryable.Where(x => x.Address.Equals(teacherFilter.Address));
            }

            if (!string.IsNullOrEmpty(teacherFilter.ContactNumber))
            {
                teacherQueryable = teacherQueryable.Where(x => x.ContactNumber.Equals(teacherFilter.ContactNumber));
            }

            if (!string.IsNullOrEmpty(teacherFilter.EmailAddress))
            {
                teacherQueryable = teacherQueryable.Where(x => x.EmailAddress.Equals(teacherFilter.EmailAddress));
            }

            teacherQueryable = "desc".Equals(teacherFilter.OrderBy, StringComparison.OrdinalIgnoreCase)
                ? teacherQueryable.OrderByDescending(x => x.CreatedAt)
                : teacherQueryable.OrderBy(x=>x.CreatedAt);

            var paginatedResponse = await 
                teacherQueryable.ToPagedListAsync(teacherFilter.CurrentPage - 1, teacherFilter.PageSize);

            return new BaseResponse<PaginatedResponse<TeacherResponseModel>>()
            {
                Code = (int)HttpStatusCode.OK,
                Message = "Retrieved successfully",
                Data = new PaginatedResponse<TeacherResponseModel>()
                {
                    CurrentPage = teacherFilter.CurrentPage,
                    TotalPages = paginatedResponse.TotalPages,
                    PageSize = teacherFilter.PageSize,
                    TotalRecords = paginatedResponse.TotalCount,
                    Data = paginatedResponse.Items.Select(x => _mapper.Map<TeacherResponseModel>(x)).ToList()
                }
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured getting teachers\n{teacherFilter}",JsonConvert.SerializeObject(teacherFilter,Formatting.Indented));
          
            return CommonResponses.ErrorResponse.FailedDependencyErrorResponse<PaginatedResponse<TeacherResponseModel>>();
        }
    }

    public async Task<BaseResponse<TeacherResponseModel>> GetTeacherByIdAsync(string teacherId)
    {
        try
        {
            var teacherExist = await _teacherContext.Teachers.FirstOrDefaultAsync(x => x.TeacherId.Equals(teacherId));
            if (teacherExist == null)
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<TeacherResponseModel>("Teacher not found");
            }

            await _redisService.GetTeacherAsync(teacherId);
            
            return CommonResponses.SuccessResponse.OkResponse(_mapper.Map<TeacherResponseModel>(teacherExist));
        }
        catch (Exception e)
        {
           _logger.LogError(e,"An error occured getting Teacher by teacher Id:{teacherId}",teacherId);
          
           return CommonResponses.ErrorResponse.InternalServerErrorResponse<TeacherResponseModel>();
        }
    }

    public async Task<BaseResponse<TeacherResponseModel>> UpdateTeacherAsync(string teacherId,
        TeacherUpdateModel teacherUpdateModel)
    {
        try
        {
            var teacherExist = await _teacherContext.Teachers.FirstOrDefaultAsync(x => x.TeacherId.Equals(teacherId));
            if (teacherExist == null)
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<TeacherResponseModel>("Teacher not found");
            }

            var updateTeacher = _mapper.Map(teacherUpdateModel, teacherExist);
            _teacherContext.Teachers.Update(updateTeacher);
            var rows = await _teacherContext.SaveChangesAsync();
            if (rows < 1)
            {
                CommonResponses.ErrorResponse.FailedDependencyErrorResponse<TeacherUpdateModel>();
            }

            return CommonResponses.SuccessResponse.CreatedResponse(_mapper.Map<TeacherResponseModel>(updateTeacher));
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured updating teacher\n{teacherUpdateModel}",JsonConvert.SerializeObject(teacherUpdateModel,Formatting.Indented));

            return CommonResponses.ErrorResponse.InternalServerErrorResponse<TeacherResponseModel>();
        }
    }

    public async Task<BaseResponse<EmptyResponse>> DeleteTeacherAsync(string teacherId)
    {
        try
        {
            var teacherExist = await _teacherContext.Teachers.FirstOrDefaultAsync(x => x.TeacherId.Equals(teacherId));
            if (teacherExist == null)
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<EmptyResponse>("Teacher not found");
            }

            _teacherContext.Teachers.Remove(teacherExist);
            var rows = await _teacherContext.SaveChangesAsync();
            if (rows < 1)
            {
                return CommonResponses.ErrorResponse.FailedDependencyErrorResponse<EmptyResponse>();
            }

            await _redisService.DeleteTeacherAsync(teacherId);
            
            return CommonResponses.SuccessResponse.DeletedResponse();
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured deleting teacher by teacher Id:{teacherId}",teacherId);

            return CommonResponses.ErrorResponse.InternalServerErrorResponse<EmptyResponse>();
        }
    }
}