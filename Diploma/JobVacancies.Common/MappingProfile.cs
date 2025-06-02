using AutoMapper;
using JobVacanciesAPI.BAL.Entity;
using JobVacanciesAPI.DAL.Entity;

namespace JobVacancies.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<VacancyDTO, Vacancy>().ReverseMap();
            CreateMap<UserDTO, User>().ReverseMap();
        }

    }
}
