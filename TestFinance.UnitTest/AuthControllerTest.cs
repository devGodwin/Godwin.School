using System.Net;
using AutoFixture;
using AutoMapper;
using Finance.Management.Api.Controllers;
using Finance.Management.Api.Data;
using Finance.Management.Api.Model.RequestModel;
using Finance.Management.Api.Model.Response;
using Finance.Management.Api.Model.ResponseModel;
using Finance.Management.Api.Services.AuthServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TestFinance.UnitTest.TestSetup;
using Xunit;

namespace TestStudent.UnitTest;

public class AuthControllerTest:IClassFixture<TestFixture>
{
    private readonly AuthController _authController;
    private readonly Mock<IAuthServices> _authServiceMock = new Mock<IAuthServices>();
    private readonly Fixture _fixture = new Fixture();

    public AuthControllerTest(TestFixture fixture)
    {
        var logger = fixture.ServiceProvider.GetService<ILogger<AuthController>>();
        var mapper = fixture.ServiceProvider.GetService<IMapper>();
        _authController = new AuthController(_authServiceMock.Object);
    }
    
    [Fact]
    public async Task Create_New_Account_Statement_If_Exist_Return_Conflict()
    {
        // Arrange
        var statementOfAccount = _fixture.Create<StatementOfAccount>();
        _authServiceMock.Setup(repos => repos.CreateStatementOfAccountAsync(It.IsAny<StatementOfAccountRequestModel>()))
            .ReturnsAsync(new BaseResponse<StatementOfAccountResponseModel>()
            {
                Code = (int)HttpStatusCode.Conflict,
                Data = new StatementOfAccountResponseModel(),
                Message = It.IsAny<string>()
            });

        // Act
        var result = await _authController.CreateStatementOfAccount(new StatementOfAccountRequestModel()
        {
           AcademicYear = statementOfAccount.AcademicYear,
           AdminName = statementOfAccount.AdminName,
           AmountPaid = statementOfAccount.AmountPaid,
           Bank = statementOfAccount.Bank,
           Fees = statementOfAccount.Fees,
           Password = It.IsAny<string>(),
           ConfirmPassword = It.IsAny<string>(),
           IndexNumber = statementOfAccount.IndexNumber,
           Level = statementOfAccount.Level,
           PaymentType = statementOfAccount.PaymentType
        }) as ObjectResult;

        // Assert
        Assert.Equal(409, result?.StatusCode);
    }
    
    [Fact]
    public async Task Create_New_Account_Statement_Return_Ok()
    {
        // Arrange
        var statementOfAccount = _fixture.Create<StatementOfAccount>();

        _authServiceMock.Setup(repos => repos.CreateStatementOfAccountAsync(It.IsAny<StatementOfAccountRequestModel>()))
            .ReturnsAsync(new BaseResponse<StatementOfAccountResponseModel>()
            {
                Code = (int)HttpStatusCode.Created,
                Data = new StatementOfAccountResponseModel(),
                Message = It.IsAny<string>()

            });

        // Act
        var result = await _authController.CreateStatementOfAccount(new StatementOfAccountRequestModel()
        {
            AcademicYear = statementOfAccount.AcademicYear,
            AdminName = statementOfAccount.AdminName,
            AmountPaid = statementOfAccount.AmountPaid,
            Bank = statementOfAccount.Bank,
            Fees = statementOfAccount.Fees,
            Password = It.IsAny<string>(),
            ConfirmPassword = It.IsAny<string>(),
            IndexNumber = statementOfAccount.IndexNumber,
            Level = statementOfAccount.Level,
            PaymentType = statementOfAccount.PaymentType
        }) as ObjectResult;
  
        // Assert
        Assert.Equal(201,result?.StatusCode);
    }
    
    [Fact]
    public async Task Login_Account_Statement_Return_Ok()
    {
        // Arrange
        var statementOfAccount = _fixture.Create<StatementOfAccount>();
        
        _authServiceMock.Setup(repos => repos.LoginStatementOfAccountAsync(It.IsAny<StatementOfAccountLoginModel>()))
            .ReturnsAsync(new BaseResponse<TokenResponse>()
            {
                Code = (int)HttpStatusCode.OK,
                Data = new TokenResponse(),
                Message = It.IsAny<string>()
            });
        
        // Act
        var result = await _authController.LoginStatementOfAccount(new StatementOfAccountLoginModel()
        {
            AdminName = statementOfAccount.IndexNumber,
            Password = It.IsAny<string>()
        }) as ObjectResult;
  
        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
}