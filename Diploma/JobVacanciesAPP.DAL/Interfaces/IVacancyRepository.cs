using JobVacanciesAPP.DAL.Models.Vacancy;

namespace JobVacanciesAPP.DAL.Interfaces
{
    public interface IVacancyRepository
    {
        Task CreateVacancy(CreateVacancy vacancy);
        Task<List<string>> GetAllSkills();
        Task<VacancyPage> GetVacanciesForRecruiter(int page, int pageSize, bool isRecommendation, int userId);
        Task<VacancyPage> GetRecommendetVacancies(int page, int pageSize, bool isRecommendation, int userId);
        Task VacancyApply(int vacancyId, int userId);
    }
}
