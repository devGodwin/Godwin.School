using System.Net;
using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Student.Management.Api.Controller;
using Student.Management.Api.Model.Filter;
using Student.Management.Api.Model.RequestModel;
using Student.Management.Api.Model.Response;
using Student.Management.Api.Model.ResponseModel;
using Student.Management.Api.Services;
using TestStudent.UnitTest.TestSetup;
using Xunit;

namespace TestStudent.UnitTest;

public class StudentControllerTest:IClassFixture<TestFixture>
{
    private readonly StudentController _studentController;
    private readonly Mock<IStudentServices> _studentServicesMock = new Mock<IStudentServices>();
    private readonly Fixture _fixture = new Fixture();

    public StudentControllerTest(TestFixture fixture)
    {
        var logger = fixture.ServiceProvider.GetService<ILogger<StudentController>>();
        var mapper = fixture.ServiceProvider.GetService<IMapper>();

        _studentController = new StudentController(_studentServicesMock.Object);
    }
    
    [Fact]
    public async Task Register_New_Student_If_Exist_Return_Conflict()
    {
        // Arrange
        var student = _fixture.Create<Student.Management.Api.Data.Student>();
        _studentServicesMock.Setup(repos => repos.RegisterStudentAsync(It.IsAny<StudentRequestModel>()))
            .ReturnsAsync(new BaseResponse<StudentResponseModel>()
            {
                Code = (int)HttpStatusCode.Conflict,
                Data = new StudentResponseModel(),
                Message = It.IsAny<string>()
            });
        
        // Act
        var result = await _studentController.RegisterStudent(new StudentRequestModel()
        {
            Address = student.Address,
            ContactNumber = student.ContactNumber,
            DateOfBirth = student.DateOfBirth,
            EmailAddress = student.EmailAddress,
            EmergencyContactName = student.EmergencyContactName,
            EmergencyContactNumber = student.EmergencyContactNumber,
            FirstName = student.FirstName,
            LastName = student.LastName
        }) as ObjectResult;
        
        // Assert
        Assert.Equal(409,result?.StatusCode);
    }
    
    [Fact]
    public async Task Register_Student_Return_Ok()
    {
        // Arrange
        var student = _fixture.Create<Student.Management.Api.Data.Student>();

        _studentServicesMock.Setup(repos => repos.RegisterStudentAsync(It.IsAny<StudentRequestModel>()))
            .ReturnsAsync(new BaseResponse<StudentResponseModel>()
            {
                Code = (int)HttpStatusCode.Created,
                Data = new StudentResponseModel(),
                Message = It.IsAny<string>()

            });

        // Act
        var result = await _studentController.RegisterStudent(new StudentRequestModel()
        {
            Address = student.Address,
            ContactNumber = student.ContactNumber,
            DateOfBirth = student.DateOfBirth,
            EmailAddress = student.EmailAddress,
            EmergencyContactName = student.EmergencyContactName,
            EmergencyContactNumber = student.EmergencyContactNumber,
            FirstName = student.FirstName,
            LastName = student.LastName
        }) as ObjectResult;
  
        // Assert
        Assert.Equal(201,result?.StatusCode);
    }
    
    [Fact]
    public async Task Login_Student_Return_Ok()
    {
        // Arrange
        var student = _fixture.Create<Student.Management.Api.Data.Student>();
        
        _studentServicesMock.Setup(repos => repos.LoginStudentAsync(It.IsAny<StudentLoginModel>()))
            .ReturnsAsync(new BaseResponse<StudentResponseModel>()
            {
                Code = (int)HttpStatusCode.OK,
                Data = new StudentResponseModel(),
                Message = It.IsAny<string>()
            });
        
        // Act
        var result = await _studentController.LoginStudent(new StudentLoginModel()
        {
            IndexNumber = student.IndexNumber,
            Password = It.IsAny<string>()
        }) as ObjectResult;
  
        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Filter_Students_Return_Ok()
    {
        // Arrange
        var student = _fixture.Create<PaginatedResponse<StudentResponseModel>>();
        _studentServicesMock.Setup(repos => repos.GetStudentsAsync(It.IsAny<StudentFilter>()))
            .ReturnsAsync(new BaseResponse<PaginatedResponse<StudentResponseModel>>()
            {
                Code  = (int)HttpStatusCode.OK,
                Message = It.IsAny<string>(),
                Data = new PaginatedResponse<StudentResponseModel>()
                {
                    CurrentPage = student.CurrentPage,
                    PageSize = student.PageSize,
                    TotalRecords = student.TotalRecords,
                    TotalPages = student.TotalPages,
                    Data = new List<StudentResponseModel>()
                }
            });

        // Act
        var result = await _studentController.GetStudents(It.IsAny<StudentFilter>()) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Get_Student_ById_Return_Ok()
    {
        // Arrange
        _studentServicesMock.Setup(repos => repos.GetStudentByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new BaseResponse<StudentResponseModel>()
            {
                Code = (int)HttpStatusCode.OK,
                Data = new StudentResponseModel(),
                Message = It.IsAny<string>()
            });

        // Act
        var result = await _studentController.GetStudentById(It.IsAny<string>()) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Update_Student_If_Null_Return_NotFound()
    {
        // Arrange
        var student = _fixture.Create<Student.Management.Api.Data.Student>();
       
        _studentServicesMock.Setup(repos =>
                repos.UpdateStudentAsync(It.IsAny<string>(), It.IsAny<StudentUpdateModel>()))
            .ReturnsAsync(new BaseResponse<StudentResponseModel>()
            {
                Code = (int)HttpStatusCode.NotFound,
                Message = It.IsAny<string>(),
                Data = new StudentResponseModel()
            });

        // Act
        var result = await _studentController.UpdateStudent(It.IsAny<string>(),new StudentUpdateModel()
        {
            Address = student.Address,
            ContactNumber = student.ContactNumber,
            DateOfBirth = student.DateOfBirth,
            EmailAddress = student.EmailAddress,
            EmergencyContactName = student.EmergencyContactName,
            EmergencyContactNumber = student.EmergencyContactNumber,
            FirstName = student.FirstName,
            LastName = student.LastName
        }) as ObjectResult;

        // Assert
        Assert.Equal(404,result?.StatusCode);
    }
    
    [Fact]
    public async Task Update_Student_Return_Ok()
    {
        // Arrange
        var student = _fixture.Create<Student.Management.Api.Data.Student>();

        _studentServicesMock.Setup(repos => repos.UpdateStudentAsync(It.IsAny<string>(), It.IsAny<StudentUpdateModel>()))
            .ReturnsAsync(new BaseResponse<StudentResponseModel>()
            {
                Code = (int)HttpStatusCode.OK,
                Message = It.IsAny<string>(),
                Data = new StudentResponseModel()
            });

        // Act
        var result = await _studentController.UpdateStudent( It.IsAny<string>(), new StudentUpdateModel()
        {
            Address = student.Address,
            ContactNumber = student.ContactNumber,
            DateOfBirth = student.DateOfBirth,
            EmailAddress = student.EmailAddress,
            EmergencyContactName = student.EmergencyContactName,
            EmergencyContactNumber = student.EmergencyContactNumber,
            FirstName = student.FirstName,
            LastName = student.LastName
        }) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Delete_Student_If_Null_Return_NotFound()
    {
        // Arrange
        var mockResponse = CommonResponses.ErrorResponse.NotFoundErrorResponse<EmptyResponse>(It.IsAny<string>());
        
        _studentServicesMock.Setup(repos => repos.DeleteStudentAsync(It.IsAny<string>()))
            .ReturnsAsync(mockResponse);

        // Act
        var result = await _studentController.DeleteStudent(It.IsAny<string>()) as ObjectResult;

        // Assert
        Assert.Equal(404,result?.StatusCode);
    }
    
    [Fact]
    public async Task Delete_Student_Return_Ok()
    {
        // Arrange
        var mockResponse = CommonResponses.SuccessResponse.DeletedResponse();
        
        _studentServicesMock.Setup(repos => repos.DeleteStudentAsync(It.IsAny<string>()))
            .ReturnsAsync(mockResponse);

        // Act
        var result = (ObjectResult) await _studentController.DeleteStudent(It.IsAny<string>());

        // Assert
        Assert.Equal(200,result.StatusCode);
    }

}