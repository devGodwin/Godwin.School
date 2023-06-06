using Finance.Management.Api.Model.RequestModel;
using Finance.Management.Api.Model.Response;
using Finance.Management.Api.Model.ResponseModel;

namespace Finance.Management.Api.Services.AuthServices;

public interface IAuthServices
{
    Task<BaseResponse<StatementOfAccountResponseModel >> CreateStatementOfAccountAsync(StatementOfAccountRequestModel statementRequestModel);
    Task<BaseResponse<TokenResponse>> LoginStatementOfAccountAsync(StatementOfAccountLoginModel statementLoginModel);
}