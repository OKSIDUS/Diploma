using JobVacanciesAPI.DAL.Entity;

namespace JobVacanciesAPI.DAL.Interfaces
{
    public interface IRecruiterRepository
    {
        Task<List<Recruiter>> GetAllAsync();
        Task<Recruiter?> GetByIdAsync(int id);
        Task AddAsync(Recruiter recruiter);
        Task UpdateAsync(Recruiter recruiter);
        Task DeleteAsync(int id);
    }
}
