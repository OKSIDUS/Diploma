using JobVacanciesAPP.DAL.Models.Users;
using JobVacanciesAPP.DAL.Models.Vacancy;

namespace JobVacanciesAPP.DAL.Interfaces
{
    public interface IVacancyRepository
    {
        Task CreateVacancy(CreateVacancy vacancy);
        Task<List<string>> GetAllSkills();
        Task<VacancyPage> GetVacanciesForRecruiter(int page, int pageSize, bool isRecommendation, int userId, string keyword);
        Task<VacancyPage> GetRecommendetVacancies(int page, int pageSize, bool isRecommendation, int userId, string keyword);
        Task VacancyApply(int vacancyId, int userId);
        Task<VacancyPage> GetAllVacancy(int page, int pageSize, bool isRecommendation, int userId, string keyword);
        Task<VacancyDTO> GetVacancy(int vacancyId);
        Task<VacancyRecruiterDTO> GetVacancyInfoForRecruiter(int vacancyId);

        Task ChangeStatus(int vacancyId, int userId, string status);
    }
}
