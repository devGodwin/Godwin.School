using System.Net;
using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Teacher.Management.Api.Controller;
using Teacher.Management.Api.Model.Filter;
using Teacher.Management.Api.Model.RequestModel;
using Teacher.Management.Api.Model.Response;
using Teacher.Management.Api.Model.ResponseModel;
using Teacher.Management.Api.Services;
using TestTeacher.UnitTest.TestSetup;
using Xunit;

namespace TestTeacher.UnitTest;

public class TeacherControllerTest:IClassFixture<TestFixture>
{
    private readonly TeacherController _teacherController;
    private readonly Mock<ITeacherServices> _teacherServicesMock = new Mock<ITeacherServices>();
    private readonly Fixture _fixture = new Fixture();

    public TeacherControllerTest(TestFixture fixture)
    {
        var logger = fixture.ServiceProvider.GetService<ILogger<TeacherController>>();
        var mapper = fixture.ServiceProvider.GetService<IMapper>();

        _teacherController = new TeacherController(_teacherServicesMock.Object);
    }
    
    [Fact]
    public async Task Register_New_Teacher_If_Exist_Return_Conflict()
    {
        // Arrange
        var teacher = _fixture.Create<Teacher.Management.Api.Data.Teacher>();
        _teacherServicesMock.Setup(repos => repos.RegisterTeacherAsync(It.IsAny<TeacherRequestModel>()))
            .ReturnsAsync(new BaseResponse<TeacherResponseModel>()
            {
                Code = (int)HttpStatusCode.Conflict,
                Data = new TeacherResponseModel(),
                Message = It.IsAny<string>()
            });
        
        // Act
        var result = await _teacherController.RegisterTeacher(new TeacherRequestModel()
        {
            Address = teacher.Address,
            ContactNumber = teacher.ContactNumber,
            DateOfBirth = teacher.DateOfBirth,
            EmailAddress = teacher.EmailAddress,
            FirstName = teacher.FirstName,
            LastName = teacher.LastName
        }) as ObjectResult;
        
        // Assert
        Assert.Equal(409,result?.StatusCode);
    }
    
    [Fact]
    public async Task Register_Teacher_Return_Ok()
    {
        // Arrange
        var teacher = _fixture.Create<Teacher.Management.Api.Data.Teacher>();

        _teacherServicesMock.Setup(repos => repos.RegisterTeacherAsync(It.IsAny<TeacherRequestModel>()))
            .ReturnsAsync(new BaseResponse<TeacherResponseModel>()
            {
                Code = (int)HttpStatusCode.Created,
                Data = new TeacherResponseModel(),
                Message = It.IsAny<string>()

            });

        // Act
        var result = await _teacherController.RegisterTeacher(new TeacherRequestModel()
        {
            Address = teacher.Address,
            ContactNumber = teacher.ContactNumber,
            DateOfBirth = teacher.DateOfBirth,
            EmailAddress = teacher.EmailAddress,
            FirstName = teacher.FirstName,
            LastName = teacher.LastName
        }) as ObjectResult;
  
        // Assert
        Assert.Equal(201,result?.StatusCode);
    }
    
    [Fact]
    public async Task Login_Teacher_Return_Ok()
    {
        // Arrange
        var teacher = _fixture.Create<Teacher.Management.Api.Data.Teacher>();
        
        _teacherServicesMock.Setup(repos => repos.LoginTeacherAsync(It.IsAny<TeacherLoginModel>()))
            .ReturnsAsync(new BaseResponse<TeacherResponseModel>()
            {
                Code = (int)HttpStatusCode.OK,
                Data = new TeacherResponseModel(),
                Message = It.IsAny<string>()
            });
        
        // Act
        var result = await _teacherController.LoginTeacher(new TeacherLoginModel()
        {
            TeacherId = teacher.TeacherId,
            Password = It.IsAny<string>()
        }) as ObjectResult;
  
        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Filter_Teacher_Return_Ok()
    {
        // Arrange
        var teacher = _fixture.Create<PaginatedResponse<TeacherResponseModel>>();
        _teacherServicesMock.Setup(repos => repos.GetTeachersAsync(It.IsAny<TeacherFilter>()))
            .ReturnsAsync(new BaseResponse<PaginatedResponse<TeacherResponseModel>>()
            {
                Code  = (int)HttpStatusCode.OK,
                Message = It.IsAny<string>(),
                Data = new PaginatedResponse<TeacherResponseModel>()
                {
                    CurrentPage = teacher.CurrentPage,
                    PageSize = teacher.PageSize,
                    TotalRecords = teacher.TotalRecords,
                    TotalPages = teacher.TotalPages,
                    Data = new List<TeacherResponseModel>()
                }
            });

        // Act
        var result = await _teacherController.GetTeachers(It.IsAny<TeacherFilter>()) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Get_Teacher_ById_Return_Ok()
    {
        // Arrange
        _teacherServicesMock.Setup(repos => repos.GetTeacherByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new BaseResponse<TeacherResponseModel>()
            {
                Code = (int)HttpStatusCode.OK,
                Data = new TeacherResponseModel(),
                Message = It.IsAny<string>()
            });

        // Act
        var result = await _teacherController.GetTeacherById(It.IsAny<string>()) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Update_Teacher_If_Null_Return_NotFound()
    {
        // Arrange
        var teacher = _fixture.Create<Teacher.Management.Api.Data.Teacher>();
        var mockResponse = CommonResponses.ErrorResponse.NotFoundErrorResponse<TeacherResponseModel>(It.IsAny<string>());
        
        _teacherServicesMock.Setup(repos =>
                repos.UpdateTeacherAsync(It.IsAny<string>(), It.IsAny<TeacherUpdateModel>()))
            .ReturnsAsync(mockResponse);

        // Act
        var result = await _teacherController.UpdateTeacher(It.IsAny<string>(),new TeacherUpdateModel()
        {
            Address = teacher.Address,
            ContactNumber = teacher.ContactNumber,
            DateOfBirth = teacher.DateOfBirth,
            EmailAddress = teacher.EmailAddress,
            FirstName = teacher.FirstName,
            LastName = teacher.LastName,
            Qualification = teacher.Qualification
        }) as ObjectResult;

        // Assert
        Assert.Equal(404,result?.StatusCode);
    }
    
    [Fact]
    public async Task Update_Teacher_Return_Ok()
    {
        // Arrange
        var teacher = _fixture.Create<Teacher.Management.Api.Data.Teacher>();

        _teacherServicesMock.Setup(repos => repos.UpdateTeacherAsync(It.IsAny<string>(), It.IsAny<TeacherUpdateModel>()))
            .ReturnsAsync(new BaseResponse<TeacherResponseModel>()
            {
                Code = (int)HttpStatusCode.OK,
                Message = It.IsAny<string>(),
                Data = new TeacherResponseModel()
            });

        // Act
        var result = await _teacherController.UpdateTeacher( It.IsAny<string>(), new TeacherUpdateModel()
        {
            Address = teacher.Address,
            ContactNumber = teacher.ContactNumber,
            DateOfBirth = teacher.DateOfBirth,
            EmailAddress = teacher.EmailAddress,
            FirstName = teacher.FirstName,
            LastName = teacher.LastName,
            Qualification = teacher.Qualification
        }) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Delete_Teacher_If_Null_Return_NotFound()
    {
        // Arrange
        var mockResponse = CommonResponses.ErrorResponse.NotFoundErrorResponse<EmptyResponse>(It.IsAny<string>());
        
        _teacherServicesMock.Setup(repos => repos.DeleteTeacherAsync(It.IsAny<string>()))
            .ReturnsAsync(mockResponse);

        // Act
        var result = await _teacherController.DeleteTeacher(It.IsAny<string>()) as ObjectResult;

        // Assert
        Assert.Equal(404,result?.StatusCode);
    }
    
    [Fact]
    public async Task Delete_Teacher_Return_Ok()
    {
        // Arrange
        var mockResponse = CommonResponses.SuccessResponse.DeletedResponse();
        
        _teacherServicesMock.Setup(repos => repos.DeleteTeacherAsync(It.IsAny<string>()))
            .ReturnsAsync(mockResponse);

        // Act
        var result = (ObjectResult) await _teacherController.DeleteTeacher(It.IsAny<string>());

        // Assert
        Assert.Equal(200,result.StatusCode);
    }

}