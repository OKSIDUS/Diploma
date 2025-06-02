using JobVacanciesAPI.DAL.Entity;

namespace JobVacanciesAPI.DAL.Interfaces
{
    public interface IVacancyRepository
    {
        Task<List<Vacancy>> GetAllAsync();
        Task<Vacancy> GetByIdAsync(int id);
        Task CreateAsync(Vacancy vacancy);
        Task UpdateAsync(Vacancy vacancy);
        Task DeleteAsync(int id);
    }

}
