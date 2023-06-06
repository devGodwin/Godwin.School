using AutoMapper;
using Finance.Management.Api.Configuration;
using Finance.Management.Api.Data;
using Finance.Management.Api.Helper;
using Finance.Management.Api.Model.RequestModel;
using Finance.Management.Api.Model.Response;
using Finance.Management.Api.Model.ResponseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Finance.Management.Api.Services.AuthServices;

public class AuthServices:IAuthServices
{
    private readonly StatementOfAccountContext _statementOfAccountContext;
    private readonly ILogger<StatementOfAccountServices> _logger;
    private readonly IMapper _mapper;
    private readonly BearerTokenConfig _bearerTokenConfig;

    public AuthServices(StatementOfAccountContext statementOfAccountContext,ILogger<StatementOfAccountServices>logger,IMapper mapper,
        IOptions<BearerTokenConfig> bearerTokenConfig)
    {
        _statementOfAccountContext = statementOfAccountContext;
        _logger = logger;
        _mapper = mapper;
        _bearerTokenConfig = bearerTokenConfig.Value;
    }
     public async Task<BaseResponse<StatementOfAccountResponseModel>> CreateStatementOfAccountAsync(StatementOfAccountRequestModel statementRequestModel)
    {
        try
        {
            var statementOfAccountExist = await _statementOfAccountContext.StatementOfAccounts.AnyAsync(x => x.IndexNumber.Equals(statementRequestModel.IndexNumber));
            if (statementOfAccountExist)
            {
                return CommonResponses.ErrorResponse.ConflictErrorResponse<StatementOfAccountResponseModel>("Student statement Of account is already added");
            }

            Authentication.CreatePasswordHash(statementRequestModel.Password,out byte[] passwordHash,out byte[] passwordSalt);
            
            var newStatementOfAccount = _mapper.Map<StatementOfAccount>(statementRequestModel);

            newStatementOfAccount.Balance = newStatementOfAccount.AmountPaid - newStatementOfAccount.Fees;
            newStatementOfAccount.Arrear = newStatementOfAccount.Fees - newStatementOfAccount.AmountPaid;
                                             

            newStatementOfAccount.PasswordHash = passwordHash;
            newStatementOfAccount.PasswordSalt = passwordSalt;
            
            await _statementOfAccountContext.StatementOfAccounts.AddAsync(newStatementOfAccount);
            var rows = await _statementOfAccountContext.SaveChangesAsync();
            if (rows < 1)
            {
                CommonResponses.ErrorResponse.FailedDependencyErrorResponse<StatementOfAccountResponseModel>();
            }

            return CommonResponses.SuccessResponse.CreatedResponse(_mapper.Map<StatementOfAccountResponseModel>(newStatementOfAccount));
        }
        catch (Exception e)
        {
           _logger.LogError(e,"An error occured adding student statement of account\n{statementRequestModel}",JsonConvert.SerializeObject(statementRequestModel,Formatting.Indented));

           return CommonResponses.ErrorResponse.InternalServerErrorResponse<StatementOfAccountResponseModel>();
        }
    }

    public async Task<BaseResponse<TokenResponse>> LoginStatementOfAccountAsync(StatementOfAccountLoginModel statementLoginModel)
    {
        try
        {
            var statementOfAccountExist = await _statementOfAccountContext.StatementOfAccounts.FirstOrDefaultAsync(x => x.AdminName.Equals(statementLoginModel.AdminName));
            if (statementOfAccountExist == null)
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<TokenResponse>("Admin name is incorrect");
            }
            
            if (!Authentication.VerifyPasswordHash(statementLoginModel.Password,statementOfAccountExist.PasswordHash,statementOfAccountExist.PasswordSalt))
            {
                return CommonResponses.ErrorResponse.ConflictErrorResponse<TokenResponse>("Password is incorrect");
            }
            
            // return token
            StatementOfAccountResponseModel statementResponse = new StatementOfAccountResponseModel
            {
                AdminName = statementLoginModel.AdminName
            };
            
            var token = TokenGenerator.GenerateToken(statementResponse, _bearerTokenConfig);
            
            return CommonResponses.SuccessResponse.OkResponse(token,"Successful");
            
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured logging in student statement of account\n{statementLoginModel}",JsonConvert.SerializeObject(statementLoginModel,Formatting.Indented));

            return CommonResponses.ErrorResponse.InternalServerErrorResponse<TokenResponse>();
        }
    }
}