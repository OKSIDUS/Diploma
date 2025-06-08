using JobVacanciesAPP.DAL.Models.Vacancy;

namespace JobVacanciesAPP.DAL.Interfaces
{
    public interface IVacancyRepository
    {
        Task CreateVacancy(CreateVacancy vacancy);
        Task<List<string>> GetAllSkills();
    }
}
