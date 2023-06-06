using System.Net;
using AutoFixture;
using AutoMapper;
using GradeAndAssessment.Api.Controller;
using GradeAndAssessment.Api.Data;
using GradeAndAssessment.Api.Model.Filter;
using GradeAndAssessment.Api.Model.RequestModel;
using GradeAndAssessment.Api.Model.Response;
using GradeAndAssessment.Api.Model.ResponseModel;
using GradeAndAssessment.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TestGradeAndAssessment.UnitTest.TestSetup;
using Xunit;

namespace TestGradeAndAssessment.UnitTest;

public class StudentAssessmentControllerTest:IClassFixture<TestFixture>
{
    private readonly StudentAssessmentController _studentAssessmentController;
    private readonly Mock<IStudentAssessmentServices> _studentAssessmentServicesMock = new Mock<IStudentAssessmentServices>();
    private readonly Fixture _fixture = new Fixture();

    public StudentAssessmentControllerTest(TestFixture fixture)
    {
        var logger = fixture.ServiceProvider.GetService<ILogger<StudentAssessmentController>>();
        var mapper = fixture.ServiceProvider.GetService<IMapper>();

        _studentAssessmentController = new StudentAssessmentController(_studentAssessmentServicesMock.Object);
    }
    
    [Fact]
    public async Task Create_New_Student_Assessment_If_Exist_Return_Conflict()
    {
        // Arrange
        var studentAssessment = _fixture.Create<StudentAssessment>();
        _studentAssessmentServicesMock.Setup(repos => repos.CreateStudentAssessmentAsync(It.IsAny<StudentAssessmentRequestModel>()))
            .ReturnsAsync(new BaseResponse<StudentAssessmentResponseModel>()
            {
                Code = (int)HttpStatusCode.Conflict,
                Data = new StudentAssessmentResponseModel(),
                Message = It.IsAny<string>()
            });
        
        // Act
        var result = await _studentAssessmentController.CreateStudentAssessment(new StudentAssessmentRequestModel()
        {
           Assignment = studentAssessment.Assignment,
           CourseId = studentAssessment.CourseId,
           EndOfSemExam = studentAssessment.EndOfSemExam,
           IndexNumber = studentAssessment.IndexNumber,
           MidsemExam = studentAssessment.MidsemExam,
           Project = studentAssessment.Project,
           TeacherName = studentAssessment.TeacherName
        }) as ObjectResult;
        
        // Assert
        Assert.Equal(409,result?.StatusCode);
    }
    
    [Fact]
    public async Task Create_Student_Assessment_Return_Ok()
    {
        // Arrange
        var studentAssessment = _fixture.Create<StudentAssessment>();

        _studentAssessmentServicesMock.Setup(repos => repos.CreateStudentAssessmentAsync(It.IsAny<StudentAssessmentRequestModel>()))
            .ReturnsAsync(new BaseResponse<StudentAssessmentResponseModel>()
            {
                Code = (int)HttpStatusCode.Created,
                Data = new StudentAssessmentResponseModel(),
                Message = It.IsAny<string>()

            });

        // Act
        var result = await _studentAssessmentController.CreateStudentAssessment(new StudentAssessmentRequestModel()
        {
            Assignment = studentAssessment.Assignment,
            CourseId = studentAssessment.CourseId,
            EndOfSemExam = studentAssessment.EndOfSemExam,
            IndexNumber = studentAssessment.IndexNumber,
            MidsemExam = studentAssessment.MidsemExam,
            Project = studentAssessment.Project,
            TeacherName = studentAssessment.TeacherName
        }) as ObjectResult;
  
        // Assert
        Assert.Equal(201,result?.StatusCode);
    }
    
    [Fact]
    public async Task Login_Student_Assessment_Return_Ok()
    {
        // Arrange
        var studentAssessment = _fixture.Create<StudentAssessment>();
        
        _studentAssessmentServicesMock.Setup(repos => repos.LoginStudentAssessmentAsync(It.IsAny<StudentAssessmentLoginModel>()))
            .ReturnsAsync(new BaseResponse<StudentAssessmentResponseModel>()
            {
                Code = (int)HttpStatusCode.OK,
                Data = new StudentAssessmentResponseModel(),
                Message = It.IsAny<string>()
            });
        
        // Act
        var result = await _studentAssessmentController.LoginStudentAssessment(new StudentAssessmentLoginModel()
        {
            AssessmentId = studentAssessment.AssessmentId,
            Password = It.IsAny<string>()
        }) as ObjectResult;
  
        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Filter_Student_Assessments_Return_Ok()
    {
        // Arrange
        var studentAssessment = _fixture.Create<PaginatedResponse<StudentAssessment>>();
        _studentAssessmentServicesMock.Setup(repos => repos.GetStudentAssessmentsAsync(It.IsAny<StudentAssessmentFilter>()))
            .ReturnsAsync(new BaseResponse<PaginatedResponse<StudentAssessmentResponseModel>>()
            {
                Code  = (int)HttpStatusCode.OK,
                Message = It.IsAny<string>(),
                Data = new PaginatedResponse<StudentAssessmentResponseModel>()
                {
                    CurrentPage = studentAssessment.CurrentPage,
                    PageSize = studentAssessment.PageSize,
                    TotalRecords = studentAssessment.TotalRecords,
                    TotalPages = studentAssessment.TotalPages,
                    Data = new List<StudentAssessmentResponseModel>()
                }
            });

        // Act
        var result = await _studentAssessmentController.GetStudentAssessments(It.IsAny<StudentAssessmentFilter>()) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Get_Student_Assessment_ById_Return_Ok()
    {
        // Arrange
        _studentAssessmentServicesMock.Setup(repos => repos.GetStudentAssessmentByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new BaseResponse<StudentAssessmentResponseModel>()
            {
                Code = (int)HttpStatusCode.OK,
                Data = new StudentAssessmentResponseModel(),
                Message = It.IsAny<string>()
            });

        // Act
        var result = await _studentAssessmentController.GetStudentAssessmentById(It.IsAny<string>()) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Update_Student_Assessment_If_Null_Return_NotFound()
    {
        // Arrange
        var studentAssessment = _fixture.Create<StudentAssessment>();
       
        _studentAssessmentServicesMock.Setup(repos =>
                repos.UpdateStudentAssessmentAsync(It.IsAny<string>(), It.IsAny<StudentAssessmentUpdateModel>()))
            .ReturnsAsync(new BaseResponse<StudentAssessmentResponseModel>()
            {
                Code = (int)HttpStatusCode.NotFound,
                Message = It.IsAny<string>(),
                Data = new StudentAssessmentResponseModel()
            });

        // Act
        var result = await _studentAssessmentController.UpdateStudentAssessment(It.IsAny<string>(),new StudentAssessmentUpdateModel()
        {
            Assignment = studentAssessment.Assignment,
            CourseId = studentAssessment.CourseId,
            EndOfSemExam = studentAssessment.EndOfSemExam,
            MidsemExam = studentAssessment.MidsemExam,
            Project = studentAssessment.Project,
            TeacherName = studentAssessment.TeacherName,
        }) as ObjectResult;

        // Assert
        Assert.Equal(404,result?.StatusCode);
    }
    
    [Fact]
    public async Task Update_Student_Assessment_Return_Ok()
    {
        // Arrange
        var studentAssessment = _fixture.Create<StudentAssessment>();

        _studentAssessmentServicesMock.Setup(repos => repos.UpdateStudentAssessmentAsync(It.IsAny<string>(), 
                It.IsAny<StudentAssessmentUpdateModel>())).ReturnsAsync(new BaseResponse<StudentAssessmentResponseModel>()
            {
                Code = (int)HttpStatusCode.OK,
                Message = It.IsAny<string>(),
                Data = new StudentAssessmentResponseModel()
            });

        // Act
        var result = await _studentAssessmentController.UpdateStudentAssessment( It.IsAny<string>(), new StudentAssessmentUpdateModel()
        {
            Assignment = studentAssessment.Assignment,
            CourseId = studentAssessment.CourseId,
            EndOfSemExam = studentAssessment.EndOfSemExam,
            MidsemExam = studentAssessment.MidsemExam,
            Project = studentAssessment.Project,
            TeacherName = studentAssessment.TeacherName,
        }) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Delete_Student_Assessment_If_Null_Return_NotFound()
    {
        // Arrange
        var mockResponse = CommonResponses.ErrorResponse.NotFoundErrorResponse<EmptyResponse>(It.IsAny<string>());
        
        _studentAssessmentServicesMock.Setup(repos => repos.DeleteStudentAssessmentAsync(It.IsAny<string>()))
            .ReturnsAsync(mockResponse);

        // Act
        var result = await _studentAssessmentController.DeleteStudentAssessment(It.IsAny<string>()) as ObjectResult;

        // Assert
        Assert.Equal(404,result?.StatusCode);
    }
    
    [Fact]
    public async Task Delete_Student_Assessment_Return_Ok()
    {
        // Arrange
        var mockResponse = CommonResponses.SuccessResponse.DeletedResponse();
        
        _studentAssessmentServicesMock.Setup(repos => repos.DeleteStudentAssessmentAsync(It.IsAny<string>()))
            .ReturnsAsync(mockResponse);

        // Act
        var result = (ObjectResult) await _studentAssessmentController.DeleteStudentAssessment(It.IsAny<string>());

        // Assert
        Assert.Equal(200,result.StatusCode);
    }

}