using System.Net;
using AutoFixture;
using AutoMapper;
using Course.Management.Api.Controller;
using Course.Management.Api.Model.Filter;
using Course.Management.Api.Model.RequestModel;
using Course.Management.Api.Model.Response;
using Course.Management.Api.Model.ResponseModel;
using Course.Management.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TestCourse.UnitTest.TestSetup;
using Xunit;

namespace TestCourse.UnitTest;

public class CourseControllerTest:IClassFixture<TestFixture>
{
    private readonly CourseController _courseController;
    private readonly Mock<ICourseServices> _courseServicesMock = new Mock<ICourseServices>();
    private readonly Fixture _fixture = new Fixture();

    public CourseControllerTest(TestFixture fixture)
    {
        var logger = fixture.ServiceProvider.GetService<ILogger<CourseController>>();
        var mapper = fixture.ServiceProvider.GetService<IMapper>();

        _courseController = new CourseController(_courseServicesMock.Object);
    }
    
    [Fact]
    public async Task Create_New_Course_If_Exist_Return_Conflict()
    {
        // Arrange
        var course = _fixture.Create<Course.Management.Api.Data.Course>();
        _courseServicesMock.Setup(repos => repos.CreateCourseAsync(It.IsAny<CourseRequestModel>()))
            .ReturnsAsync(new BaseResponse<CourseResponseModel>()
            {
                Code = (int)HttpStatusCode.Conflict,
                Data = new CourseResponseModel(),
                Message = It.IsAny<string>()
            });
        
        // Act
        var result = await _courseController.CreateCourse(new CourseRequestModel()
        {
            CourseDescription = course.CourseDescription,
            CourseName = course.CourseName,
        }) as ObjectResult;
        
        // Assert
        Assert.Equal(409,result?.StatusCode);
    }
    
    [Fact]
    public async Task Create_Course_Return_Ok()
    {
        // Arrange
        var course = _fixture.Create<Course.Management.Api.Data.Course>();

        _courseServicesMock.Setup(repos => repos.CreateCourseAsync(It.IsAny<CourseRequestModel>()))
            .ReturnsAsync(new BaseResponse<CourseResponseModel>()
            {
                Code = (int)HttpStatusCode.Created,
                Data = new CourseResponseModel(),
                Message = It.IsAny<string>()

            });

        // Act
        var result = await _courseController.CreateCourse(new CourseRequestModel()
        {
            CourseDescription = course.CourseDescription,
            CourseName = course.CourseName,
        }) as ObjectResult;
  
        // Assert
        Assert.Equal(201,result?.StatusCode);
    }

    [Fact]
    public async Task Filter_Courses_Return_Ok()
    {
        // Arrange
        var course = _fixture.Create<PaginatedResponse<CourseResponseModel>>();
        _courseServicesMock.Setup(repos => repos.GetCoursesAsync(It.IsAny<CourseFilter>()))
            .ReturnsAsync(new BaseResponse<PaginatedResponse<CourseResponseModel>>()
            {
                Code  = (int)HttpStatusCode.OK,
                Message = It.IsAny<string>(),
                Data = new PaginatedResponse<CourseResponseModel>()
                {
                    CurrentPage = course.CurrentPage,
                    PageSize = course.PageSize,
                    TotalRecords = course.TotalRecords,
                    TotalPages = course.TotalPages,
                    Data = new List<CourseResponseModel>()
                }
            });

        // Act
        var result = await _courseController.GetCourses(It.IsAny<CourseFilter>()) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Get_Course_ById_Return_Ok()
    {
        // Arrange
        _courseServicesMock.Setup(repos => repos.GetCourseByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new BaseResponse<CourseResponseModel>()
            {
                Code = (int)HttpStatusCode.OK,
                Data = new CourseResponseModel(),
                Message = It.IsAny<string>()
            });

        // Act
        var result = await _courseController.GetCourseById(It.IsAny<string>()) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Update_Course_If_Null_Return_NotFound()
    {
        // Arrange
        var course = _fixture.Create<Course.Management.Api.Data.Course>();
       
        _courseServicesMock.Setup(repos =>
                repos.UpdateCourseAsync(It.IsAny<string>(), It.IsAny<CourseUpdateModel>()))
            .ReturnsAsync(new BaseResponse<CourseResponseModel>()
            {
                Code = (int)HttpStatusCode.NotFound,
                Message = It.IsAny<string>(),
                Data = new CourseResponseModel()
            });

        // Act
        var result = await _courseController.UpdateCourse(It.IsAny<string>(),new CourseUpdateModel()
        {
            CourseDescription = course.CourseDescription,
            CourseName = course.CourseName
        }) as ObjectResult;

        // Assert
        Assert.Equal(404,result?.StatusCode);
    }
    
    [Fact]
    public async Task Update_Course_Return_Ok()
    {
        // Arrange
        var course = _fixture.Create<Course.Management.Api.Data.Course>();

        _courseServicesMock.Setup(repos => repos.UpdateCourseAsync(It.IsAny<string>(), It.IsAny<CourseUpdateModel>()))
            .ReturnsAsync(new BaseResponse<CourseResponseModel>()
            {
                Code = (int)HttpStatusCode.OK,
                Message = It.IsAny<string>(),
                Data = new CourseResponseModel()
            });

        // Act
        var result = await _courseController.UpdateCourse( It.IsAny<string>(), new CourseUpdateModel()
        {
            CourseDescription = course.CourseDescription,
            CourseName = course.CourseName,
        }) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Delete_Course_If_Null_Return_NotFound()
    {
        // Arrange
        var mockResponse = CommonResponses.ErrorResponse.NotFoundErrorResponse<EmptyResponse>(It.IsAny<string>());
        
        _courseServicesMock.Setup(repos => repos.DeleteCourseAsync(It.IsAny<string>()))
            .ReturnsAsync(mockResponse);

        // Act
        var result = await _courseController.DeleteCourse(It.IsAny<string>()) as ObjectResult;

        // Assert
        Assert.Equal(404,result?.StatusCode);
    }
    
    [Fact]
    public async Task Delete_Course_Return_Ok()
    {
        // Arrange
        var mockResponse = CommonResponses.SuccessResponse.DeletedResponse();
        
        _courseServicesMock.Setup(repos => repos.DeleteCourseAsync(It.IsAny<string>()))
            .ReturnsAsync(mockResponse);

        // Act
        var result = (ObjectResult) await _courseController.DeleteCourse(It.IsAny<string>());

        // Assert
        Assert.Equal(200,result.StatusCode);
    }

}