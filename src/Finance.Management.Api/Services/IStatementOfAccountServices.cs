using Finance.Management.Api.Model.Filter;
using Finance.Management.Api.Model.RequestModel;
using Finance.Management.Api.Model.Response;
using Finance.Management.Api.Model.ResponseModel;

namespace Finance.Management.Api.Services;

public interface IStatementOfAccountServices
{
    Task<BaseResponse<PaginatedResponse<StatementOfAccountResponseModel>>> GetStatementOfAccountsAsync(StatementOfAccountFilter statementFilter);
    Task<BaseResponse<StatementOfAccountResponseModel>> GetStatementOfAccountByIdAsync(string indexNumber);
    Task<BaseResponse<StatementOfAccountResponseModel>> UpdateStatementOfAccountAsync(string indexNumber, StatementOfAccountUpdateModel statementUpdateModel);
    Task<BaseResponse<EmptyResponse>> DeleteStatementOfAccountAsync(string indexNumber);

}