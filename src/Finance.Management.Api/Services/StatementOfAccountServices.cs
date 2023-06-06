using System.Net;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using Finance.Management.Api.Configuration;
using Finance.Management.Api.Data;
using Finance.Management.Api.Helper;
using Finance.Management.Api.Model.Filter;
using Finance.Management.Api.Model.RequestModel;
using Finance.Management.Api.Model.Response;
using Finance.Management.Api.Model.ResponseModel;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Finance.Management.Api.Services;

public class StatementOfAccountServices:IStatementOfAccountServices
{
    private readonly StatementOfAccountContext _statementOfAccountContext;
    private readonly ILogger<StatementOfAccountServices> _logger;
    private readonly IMapper _mapper;

    public StatementOfAccountServices(StatementOfAccountContext statementOfAccountContext,ILogger<StatementOfAccountServices>logger,IMapper mapper)
    {
        _statementOfAccountContext = statementOfAccountContext;
        _logger = logger;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<PaginatedResponse<StatementOfAccountResponseModel>>> GetStatementOfAccountsAsync(StatementOfAccountFilter statementFilter)
    {
        try
        {
            var statementOfAccountQueryable = _statementOfAccountContext.StatementOfAccounts.AsNoTracking().AsQueryable();
            
            if (!string.IsNullOrEmpty(statementFilter.StatementId))
            {
                statementOfAccountQueryable = statementOfAccountQueryable.Where(x => x.StatementId.Equals(statementFilter.StatementId));
            }
            
            if (!string.IsNullOrEmpty(statementFilter.IndexNumber))
            {
                statementOfAccountQueryable = statementOfAccountQueryable.Where(x => x.IndexNumber.Equals(statementFilter.IndexNumber));
            }

            if (!string.IsNullOrEmpty(statementFilter.AdminName))
            {
                statementOfAccountQueryable = statementOfAccountQueryable.Where(x => x.AdminName.Equals(statementFilter.AdminName));
            }
            
            if (!string.IsNullOrEmpty(statementFilter.AcademicYear))
            {
                statementOfAccountQueryable = statementOfAccountQueryable.Where(x => x.AcademicYear.Equals(statementFilter.AcademicYear));
            }
            if (!string.IsNullOrEmpty(statementFilter.Level))
            {
                statementOfAccountQueryable = statementOfAccountQueryable.Where(x => x.Level.Equals(statementFilter.Level));
            }

            statementOfAccountQueryable = "desc".Equals(statementFilter.OrderBy, StringComparison.OrdinalIgnoreCase)
                ? statementOfAccountQueryable.OrderByDescending(x => x.PaymentDate)
                : statementOfAccountQueryable.OrderBy(x=>x.PaymentDate);

            var paginatedResponse = await 
                statementOfAccountQueryable.ToPagedListAsync(statementFilter.CurrentPage - 1, statementFilter.PageSize);

            return new BaseResponse<PaginatedResponse<StatementOfAccountResponseModel>>()
            {
                Code = (int)HttpStatusCode.OK,
                Message = "Retrieved successfully",
                Data = new PaginatedResponse<StatementOfAccountResponseModel>()
                {
                    CurrentPage = statementFilter.CurrentPage,
                    TotalPages = paginatedResponse.TotalPages,
                    PageSize = statementFilter.PageSize,
                    TotalRecords = paginatedResponse.TotalCount,
                    Data = paginatedResponse.Items.Select(x => _mapper.Map<StatementOfAccountResponseModel>(x)).ToList()
                }
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured getting students statement of account\n{statementFilter}",JsonConvert.SerializeObject(statementFilter,Formatting.Indented));
          
            return CommonResponses.ErrorResponse.FailedDependencyErrorResponse<PaginatedResponse<StatementOfAccountResponseModel>>();
        }
    }

    public async Task<BaseResponse<StatementOfAccountResponseModel>> GetStatementOfAccountByIdAsync(string indexNumber)
    {
        try
        {
            var statementOfAccountExist = await _statementOfAccountContext.StatementOfAccounts.FirstOrDefaultAsync(x => x.IndexNumber.Equals(indexNumber));
            if (statementOfAccountExist == null)
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<StatementOfAccountResponseModel>("Student statement of account not found");
            }

            return CommonResponses.SuccessResponse.OkResponse(_mapper.Map<StatementOfAccountResponseModel>(statementOfAccountExist));
        }
        catch (Exception e)
        {
           _logger.LogError(e,"An error occured getting student statement of account by index number:{indexNumber}",indexNumber);
          
           return CommonResponses.ErrorResponse.InternalServerErrorResponse<StatementOfAccountResponseModel>();
        }
    }

    public async Task<BaseResponse<StatementOfAccountResponseModel>> UpdateStatementOfAccountAsync(string indexNumber,
        StatementOfAccountUpdateModel statementUpdateModel)
    {
        try
        {
            var statementOfAccountExist = await _statementOfAccountContext.StatementOfAccounts.FirstOrDefaultAsync(x => x.IndexNumber.Equals(indexNumber));
            if (statementOfAccountExist == null)
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<StatementOfAccountResponseModel>("Student statement of account not found");
            }

            var updateStatementOfAccount = _mapper.Map(statementUpdateModel, statementOfAccountExist);
            
            updateStatementOfAccount.Balance = updateStatementOfAccount.AmountPaid - updateStatementOfAccount.Fees;
            updateStatementOfAccount.Arrear = updateStatementOfAccount.Fees - updateStatementOfAccount.AmountPaid;


            _statementOfAccountContext.StatementOfAccounts.Update(updateStatementOfAccount);
            
            var rows = await _statementOfAccountContext.SaveChangesAsync();
            if (rows < 1)
            {
                CommonResponses.ErrorResponse.FailedDependencyErrorResponse<StatementOfAccountResponseModel>();
            }

            return CommonResponses.SuccessResponse.CreatedResponse(_mapper.Map<StatementOfAccountResponseModel>(updateStatementOfAccount));
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured updating student statement of account\n{statementUpdateModel}",JsonConvert.SerializeObject(statementUpdateModel,Formatting.Indented));

            return CommonResponses.ErrorResponse.InternalServerErrorResponse<StatementOfAccountResponseModel>();
        }
    }

    public async Task<BaseResponse<EmptyResponse>> DeleteStatementOfAccountAsync(string indexNumber)
    {
        try
        {
            var statementOfAccountExist = await _statementOfAccountContext.StatementOfAccounts.FirstOrDefaultAsync(x => x.IndexNumber.Equals(indexNumber));
            if (statementOfAccountExist == null)
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<EmptyResponse>("Student statement of account not found");
            }

            _statementOfAccountContext.StatementOfAccounts.Remove(statementOfAccountExist);
        
            var rows = await _statementOfAccountContext.SaveChangesAsync();
            if (rows < 1)
            {
                return CommonResponses.ErrorResponse.FailedDependencyErrorResponse<EmptyResponse>();
            }

            return CommonResponses.SuccessResponse.DeletedResponse();
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured deleting student statement of account by index number\n{indexNumber}",indexNumber);

            return CommonResponses.ErrorResponse.InternalServerErrorResponse<EmptyResponse>();
        }
    }
}