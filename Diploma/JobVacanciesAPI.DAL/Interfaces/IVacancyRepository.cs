using JobVacanciesAPI.DAL.Entity;

namespace JobVacanciesAPI.DAL.Interfaces
{
    public interface IVacancyRepository
    {
        Task<List<Vacancy>> GetAllAsync();
        Task<Vacancy?> GetByIdAsync(int id);
        Task<List<Vacancy>> GetByRecruiterIdAsync(int recruiterId);
        Task AddAsync(Vacancy vacancy);
        Task UpdateAsync(Vacancy vacancy);
        Task DeleteAsync(int id);
    }

}
