using AutoMapper;
using JobVacanciesAPI.BAL.DTOs.Candidate;
using JobVacanciesAPI.BAL.DTOs.Recruiter;
using JobVacanciesAPI.BAL.DTOs.Vacancy;
using JobVacanciesAPI.BAL.DTOs.Tag;
using JobVacanciesAPI.DAL.Entity;
using JobVacanciesAPI.BAL.DTOs.Application;

namespace JobVacancies.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<VacancyDTO, Vacancy>().ReverseMap();
            CreateMap<CandidateDTO, Candidate>().ReverseMap();
            CreateMap<RecruiterDTO, Recruiter>().ReverseMap();
            CreateMap<TagDTO, Tag>().ReverseMap();
            CreateMap<ApplicationDTO, Application>().ReverseMap();
        }

    }
}
