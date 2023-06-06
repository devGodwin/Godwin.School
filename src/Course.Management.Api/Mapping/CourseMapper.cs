using AutoMapper;
using Course.Management.Api.Model.RequestModel;
using Course.Management.Api.Model.ResponseModel;

namespace Course.Management.Api.Mapping;

    public class CourseMapper : Profile
    {
        public CourseMapper()
        {
            CreateMap<Data.Course, CourseRequestModel>().ReverseMap();
            CreateMap<Data.Course, CourseUpdateModel>().ReverseMap();
            CreateMap<Data.Course, CourseResponseModel>().ReverseMap();
        }
    }
