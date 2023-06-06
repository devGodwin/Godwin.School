using System.Net;
using AutoFixture;
using AutoMapper;
using Finance.Management.Api.Controllers;
using Finance.Management.Api.Data;
using Finance.Management.Api.Model.Filter;
using Finance.Management.Api.Model.RequestModel;
using Finance.Management.Api.Model.Response;
using Finance.Management.Api.Model.ResponseModel;
using Finance.Management.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TestFinance.UnitTest.TestSetup;
using Xunit;

namespace TestFinance.UnitTest;

public class StatementOfAccountControllerTest:IClassFixture<TestFixture>
{
    private readonly StatementOfAccountController _statementOfAccountController;
    private readonly Mock<IStatementOfAccountServices> _statementOfAccountServicesMock = new Mock<IStatementOfAccountServices>();
    private readonly Fixture _fixture = new Fixture();

    public StatementOfAccountControllerTest(TestFixture fixture)
    {
        var logger = fixture.ServiceProvider.GetService<ILogger<StatementOfAccountController>>();
        var mapper = fixture.ServiceProvider.GetService<IMapper>();

        _statementOfAccountController = new StatementOfAccountController(_statementOfAccountServicesMock.Object);
    }

    [Fact]
    public async Task Filter_Account_Statement_Return_Ok()
    {
        // Arrange
        var statementOfAccount = _fixture.Create<PaginatedResponse<StatementOfAccount>>();
        _statementOfAccountServicesMock.Setup(repos => repos.GetStatementOfAccountsAsync(It.IsAny<StatementOfAccountFilter>()))
            .ReturnsAsync(new BaseResponse<PaginatedResponse<StatementOfAccountResponseModel>>()
            {
                Code  = (int)HttpStatusCode.OK,
                Message = It.IsAny<string>(),
                Data = new PaginatedResponse<StatementOfAccountResponseModel>()
                {
                    CurrentPage = statementOfAccount.CurrentPage,
                    PageSize = statementOfAccount.PageSize,
                    TotalRecords = statementOfAccount.TotalRecords,
                    TotalPages = statementOfAccount.TotalPages,
                    Data = new List<StatementOfAccountResponseModel>()
                }
            });

        // Act
        var result = await _statementOfAccountController.GetStatementOfAccounts(It.IsAny<StatementOfAccountFilter>()) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Get_Account_Statement_ById_Return_Ok()
    {
        // Arrange
        _statementOfAccountServicesMock.Setup(repos => repos.GetStatementOfAccountByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new BaseResponse<StatementOfAccountResponseModel>()
            {
                Code = (int)HttpStatusCode.OK,
                Data = new StatementOfAccountResponseModel(),
                Message = It.IsAny<string>()
            });

        // Act
        var result = await _statementOfAccountController.GetStatementOfAccountById(It.IsAny<string>()) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Update_Account_Statement_If_Null_Return_NotFound()
    {
        // Arrange
        var statementOfAccount = _fixture.Create<StatementOfAccount>();
       
        _statementOfAccountServicesMock.Setup(repos =>
                repos.UpdateStatementOfAccountAsync(It.IsAny<string>(), It.IsAny<StatementOfAccountUpdateModel>()))
            .ReturnsAsync(new BaseResponse<StatementOfAccountResponseModel>()
            {
                Code = (int)HttpStatusCode.NotFound,
                Message = It.IsAny<string>(),
                Data = new StatementOfAccountResponseModel()
            });

        // Act
        var result = await _statementOfAccountController.UpdateStatementOfAccount(It.IsAny<string>(),new StatementOfAccountUpdateModel()
        {
            AmountPaid = statementOfAccount.AmountPaid,
            AcademicYear = statementOfAccount.AcademicYear,
            Bank = statementOfAccount.Bank,
            Fees = statementOfAccount.Fees,
            Level = statementOfAccount.Level,
            PaymentType = statementOfAccount.PaymentType,
            PaymentDate = statementOfAccount.PaymentDate,
        }) as ObjectResult;

        // Assert
        Assert.Equal(404,result?.StatusCode);
    }
    
    [Fact]
    public async Task Update_Account_Statement_Return_Ok()
    {
        // Arrange
        var statementOfAccount = _fixture.Create<StatementOfAccount>();

        _statementOfAccountServicesMock.Setup(repos => repos.UpdateStatementOfAccountAsync(It.IsAny<string>(), 
                It.IsAny<StatementOfAccountUpdateModel>())).ReturnsAsync(new BaseResponse<StatementOfAccountResponseModel>()
            {
                Code = (int)HttpStatusCode.OK,
                Message = It.IsAny<string>(),
                Data = new StatementOfAccountResponseModel()
            });

        // Act
        var result = await _statementOfAccountController.UpdateStatementOfAccount( It.IsAny<string>(), new StatementOfAccountUpdateModel()
        {
            AmountPaid = statementOfAccount.AmountPaid,
            AcademicYear = statementOfAccount.AcademicYear,
            Bank = statementOfAccount.Bank,
            Fees = statementOfAccount.Fees,
            Level = statementOfAccount.Level,
            PaymentType = statementOfAccount.PaymentType,
            PaymentDate = statementOfAccount.PaymentDate,
        }) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Delete_Account_Statement_If_Null_Return_NotFound()
    {
        // Arrange
        var mockResponse = CommonResponses.ErrorResponse.NotFoundErrorResponse<EmptyResponse>(It.IsAny<string>());
        
        _statementOfAccountServicesMock.Setup(repos => repos.DeleteStatementOfAccountAsync(It.IsAny<string>()))
            .ReturnsAsync(mockResponse);

        // Act
        var result = await _statementOfAccountController.DeleteStudent(It.IsAny<string>()) as ObjectResult;

        // Assert
        Assert.Equal(404,result?.StatusCode);
    }
    
    [Fact]
    public async Task Delete_Account_Statement_Return_Ok()
    {
        // Arrange
        var mockResponse = CommonResponses.SuccessResponse.DeletedResponse();
        
        _statementOfAccountServicesMock.Setup(repos => repos.DeleteStatementOfAccountAsync(It.IsAny<string>()))
            .ReturnsAsync(mockResponse);

        // Act
        var result = (ObjectResult) await _statementOfAccountController.DeleteStudent(It.IsAny<string>());

        // Assert
        Assert.Equal(200,result.StatusCode);
    }

}