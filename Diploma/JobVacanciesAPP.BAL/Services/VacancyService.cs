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
    }
}
