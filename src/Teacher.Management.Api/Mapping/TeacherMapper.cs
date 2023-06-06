
using AutoMapper;
using Teacher.Management.Api.Model;
using Teacher.Management.Api.Model.RequestModel;
using Teacher.Management.Api.Model.ResponseModel;

namespace Teacher.Management.Api.Mapping;

    public class TeacherMapper : Profile
    {
        public TeacherMapper()
        {
            CreateMap<Data.Teacher, TeacherRequestModel>().ReverseMap();
            CreateMap<Data.Teacher, TeacherUpdateModel>().ReverseMap();
            CreateMap<Data.Teacher, TeacherResponseModel>().ReverseMap();
            CreateMap<Data.Teacher, CachedTeacher>().ReverseMap();
        }
    }
