using AutoMapper;
using GradeAndAssessment.Api.Data;
using GradeAndAssessment.Api.Model;
using GradeAndAssessment.Api.Model.RequestModel;
using GradeAndAssessment.Api.Model.ResponseModel;

namespace GradeAndAssessment.Api.Mapping;

    public class StudentAssessmentMapper : Profile
    {
        public StudentAssessmentMapper()
        {
            CreateMap<StudentAssessment, StudentAssessmentRequestModel>().ReverseMap();
            CreateMap<StudentAssessment, StudentAssessmentUpdateModel>().ReverseMap();
            CreateMap<StudentAssessment, StudentAssessmentResponseModel>().ReverseMap();
            CreateMap<StudentAssessment, CachedStudentAssessment>().ReverseMap();
        }
    }
