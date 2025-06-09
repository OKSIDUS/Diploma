using JobVacanciesAPP.DAL.Models.Users;
using JobVacanciesAPP.DAL.Models.Vacancy;

namespace JobVacanciesAPP.BAL.Interfaces
{
    public interface IVacancyService
    {
        Task CreateVacancy(CreateVacancy vacancy);
        Task<Skills> GetAllSkills();
        Task VacancyApply(int vacancyId, int userId);
        Task<VacancyPage> GetVacancyPage(int page, int pageSize, int userId, bool isRecommendation, string keyword = "");
        Task<VacancyDTO> GetVacancyInfo(int vacancyId);
        Task<VacancyRecruiterDTO> GetVacancyRecruiter(int vacancyId);
        Task ChangeStatus(int vacancyId, int userId, string status);
    }
}
