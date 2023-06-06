using AutoMapper;
using Finance.Management.Api.Data;
using Finance.Management.Api.Model.RequestModel;
using Finance.Management.Api.Model.ResponseModel;

namespace Finance.Management.Api.Mapping;

    public class StatementOfAccountMapper : Profile
    {
        public StatementOfAccountMapper()
        {
            CreateMap<StatementOfAccount, StatementOfAccountRequestModel>().ReverseMap();
            CreateMap<StatementOfAccount, StatementOfAccountUpdateModel>().ReverseMap();
            CreateMap<StatementOfAccount, StatementOfAccountResponseModel>().ReverseMap();
        }
    }
