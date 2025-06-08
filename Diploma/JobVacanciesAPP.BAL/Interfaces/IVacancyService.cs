using JobVacanciesAPP.DAL.Models.Users;
using JobVacanciesAPP.DAL.Models.Vacancy;

namespace JobVacanciesAPP.BAL.Interfaces
{
    public interface IVacancyService
    {
        Task CreateVacancy(CreateVacancy vacancy);
        Task<Skills> GetAllSkills();
    }
}
