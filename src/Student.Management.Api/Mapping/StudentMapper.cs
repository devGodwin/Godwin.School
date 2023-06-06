
using AutoMapper;
using Student.Management.Api.Model;
using Student.Management.Api.Model.RequestModel;
using Student.Management.Api.Model.ResponseModel;

namespace Student.Management.Api.Mapping;

    public class StudentMapper : Profile
    {
        public StudentMapper()
        {
            CreateMap<Data.Student, StudentRequestModel>().ReverseMap();
            CreateMap<Data.Student, StudentUpdateModel>().ReverseMap();
            CreateMap<Data.Student, StudentResponseModel>().ReverseMap();
            CreateMap<Data.Student, CachedStudent>().ReverseMap();
        }
    }
