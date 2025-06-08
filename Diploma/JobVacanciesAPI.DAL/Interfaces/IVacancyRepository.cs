using JobVacanciesAPI.DAL.Entity;

namespace JobVacanciesAPI.DAL.Interfaces
{
    public interface IVacancyRepository
    {
        Task<List<Vacancy>> GetAllAsync();
        Task<Vacancy?> GetByIdAsync(int id);
        Task<List<Vacancy>> GetByRecruiterIdAsync(int recruiterId);
        Task<int> AddAsync(Vacancy vacancy);
        Task UpdateAsync(Vacancy vacancy);
        Task DeleteAsync(int id);
        Task<List<Vacancy>> GetNewVacancies();
        Task<List<Vacancy>> GetVacanciesByIds(List<int> ids);

        Task VacancyApply(int vacancyId, int candidateId);
    }

}
