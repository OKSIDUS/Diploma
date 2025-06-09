using JobVacanciesAPP.DAL.Interfaces;
using JobVacanciesAPP.BAL.Interfaces;
using JobVacanciesAPP.DAL.Models.Vacancy;
using JobVacanciesAPP.DAL.Models.Users;

namespace JobVacanciesAPP.BAL.Services
{
    public class VacancyService : IVacancyService
    {
        private readonly IVacancyRepository _repository;

        public VacancyService(IVacancyRepository repository)
        {
            _repository = repository;
        }

        public async Task ChangeStatus(int vacancyId, int userId, string status)
        {
           await _repository.ChangeStatus(vacancyId, userId, status);
        }

        public async Task CreateVacancy(CreateVacancy vacancy)
        {
            if (vacancy != null)
            {
                await _repository.CreateVacancy(vacancy);
            }
        }

        public async Task<Skills> GetAllSkills()
        {
            var skills = new Skills();
            skills.AllAvailableTags = await _repository.GetAllSkills();
            skills.SelectedTags = new();

            return skills;
        }

        public async Task<VacancyDTO> GetVacancyInfo(int vacancyId)
        {
            return await _repository.GetVacancy(vacancyId);
        }

        public async Task<VacancyPage> GetVacancyPage(int page, int pageSize, int userId, bool isRecommendation, string keyword = "")
        {
            return await _repository.GetAllVacancy(page, pageSize, false, userId, keyword);
        }

        public async Task<VacancyRecruiterDTO> GetVacancyRecruiter(int vacancyId)
        {
            return await _repository.GetVacancyInfoForRecruiter(vacancyId);
        }

        public async Task VacancyApply(int vacancyId, int userId)
        {
            await _repository.VacancyApply(vacancyId, userId);
        }
    }
}
