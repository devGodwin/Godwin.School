using System.Net;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using Course.Management.Api.Data;
using Course.Management.Api.Model.Filter;
using Course.Management.Api.Model.RequestModel;
using Course.Management.Api.Model.Response;
using Course.Management.Api.Model.ResponseModel;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Course.Management.Api.Services;

public class CourseServices:ICourseServices
{
    private readonly CourseContext _courseContext;
    private readonly ILogger<CourseServices> _logger;
    private readonly IMapper _mapper;

    public CourseServices(CourseContext courseContext,ILogger<CourseServices>logger,IMapper mapper)
    {
        _courseContext = courseContext;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task<BaseResponse<CourseResponseModel>> CreateCourseAsync(CourseRequestModel courseRequestModel)
    {
        try
        {
            var courseExist = await _courseContext.Courses.AnyAsync(x => x.CourseName.Equals(courseRequestModel.CourseName));
            if (courseExist)
            {
                return CommonResponses.ErrorResponse.ConflictErrorResponse<CourseResponseModel>("Course is already added");
            }

            var newCourse = _mapper.Map<Data.Course>(courseRequestModel);

            await _courseContext.Courses.AddAsync(newCourse);
            var rows = await _courseContext.SaveChangesAsync();
            if (rows < 1)
            {
                CommonResponses.ErrorResponse.FailedDependencyErrorResponse<CourseResponseModel>();
            }

            return CommonResponses.SuccessResponse.CreatedResponse(_mapper.Map<CourseResponseModel>(newCourse));
        }
        catch (Exception e)
        {
           _logger.LogError(e,"An error occured adding course\n{courseRequestModel}",JsonConvert.SerializeObject(courseRequestModel,Formatting.Indented));

           return CommonResponses.ErrorResponse.InternalServerErrorResponse<CourseResponseModel>();
        }
    }

    public async Task<BaseResponse<PaginatedResponse<CourseResponseModel>>> GetCoursesAsync(CourseFilter courseFilter)
    {
        try
        {
            var courseQueryable = _courseContext.Courses.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(courseFilter.CourseId))
            {
                courseQueryable = courseQueryable.Where(x => x.CourseId.Equals(courseFilter.CourseId));
            }

            if (!string.IsNullOrEmpty(courseFilter.CourseName))
            {
                courseQueryable = courseQueryable.Where(x => x.CourseName.Equals(courseFilter.CourseName));
            }

            if (!string.IsNullOrEmpty(courseFilter.CourseDescription))
            {
                courseQueryable = courseQueryable.Where(x => x.CourseDescription.Equals(courseFilter.CourseDescription));
            }

            courseQueryable = "desc".Equals(courseFilter.OrderBy, StringComparison.OrdinalIgnoreCase)
                ? courseQueryable.OrderByDescending(x => x.CreatedAt)
                : courseQueryable.OrderBy(x=>x.CreatedAt);

            var paginatedResponse = await 
                courseQueryable.ToPagedListAsync(courseFilter.CurrentPage - 1, courseFilter.PageSize);

            return new BaseResponse<PaginatedResponse<CourseResponseModel>>()
            {
                Code = (int)HttpStatusCode.OK,
                Message = "Retrieved successfully",
                Data = new PaginatedResponse<CourseResponseModel>()
                {
                    CurrentPage = courseFilter.CurrentPage,
                    TotalPages = paginatedResponse.TotalPages,
                    PageSize = courseFilter.PageSize,
                    TotalRecords = paginatedResponse.TotalCount,
                    Data = paginatedResponse.Items.Select(x => _mapper.Map<CourseResponseModel>(x)).ToList()
                }
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured getting courses\n{courseFilter}",JsonConvert.SerializeObject(courseFilter,Formatting.Indented));
          
            return CommonResponses.ErrorResponse.FailedDependencyErrorResponse<PaginatedResponse<CourseResponseModel>>();
        }
    }

    public async Task<BaseResponse<CourseResponseModel>> GetCourseByIdAsync(string courseId)
    {
        try
        {
            var courseExist = await _courseContext.Courses.FirstOrDefaultAsync(x => x.CourseId.Equals(courseId));
            if (courseExist == null)
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<CourseResponseModel>("Course not found");
            }

            return CommonResponses.SuccessResponse.OkResponse(_mapper.Map<CourseResponseModel>(courseExist));
        }
        catch (Exception e)
        {
           _logger.LogError(e,"An error occured getting course by course Id:{courseId}",courseId);
          
           return CommonResponses.ErrorResponse.InternalServerErrorResponse<CourseResponseModel>();
        }
    }

    public async Task<BaseResponse<CourseResponseModel>> UpdateCourseAsync(string courseId,
        CourseUpdateModel courseUpdateModel)
    {
        try
        {
            var courseExist = await _courseContext.Courses.FirstOrDefaultAsync(x => x.CourseId.Equals(courseId));
            if (courseExist == null)
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<CourseResponseModel>("Course not found");
            }

            var updateCourse = _mapper.Map(courseUpdateModel, courseExist);
            _courseContext.Courses.Update(updateCourse);
            var rows = await _courseContext.SaveChangesAsync();
            if (rows < 1)
            {
                CommonResponses.ErrorResponse.FailedDependencyErrorResponse<CourseUpdateModel>();
            }

            return CommonResponses.SuccessResponse.CreatedResponse(_mapper.Map<CourseResponseModel>(updateCourse));
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured updating course\n{courseUpdateModel}",JsonConvert.SerializeObject(courseUpdateModel,Formatting.Indented));

            return CommonResponses.ErrorResponse.InternalServerErrorResponse<CourseResponseModel>();
        }
    }

    public async Task<BaseResponse<EmptyResponse>> DeleteCourseAsync(string courseId)
    {
        try
        {
            var courseExist = await _courseContext.Courses.FirstOrDefaultAsync(x => x.CourseId.Equals(courseId));
            if (courseExist == null)
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<EmptyResponse>("Course not found");
            }

            _courseContext.Courses.Remove(courseExist);
            var rows = await _courseContext.SaveChangesAsync();
            if (rows < 1)
            {
                return CommonResponses.ErrorResponse.FailedDependencyErrorResponse<EmptyResponse>();
            }

            return CommonResponses.SuccessResponse.DeletedResponse();
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured deleting course by course Id:{courseId}",courseId);

            return CommonResponses.ErrorResponse.InternalServerErrorResponse<EmptyResponse>();
        }
    }
}