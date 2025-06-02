using JobVacanciesAPI.DAL.Entity;
namespace JobVacanciesAPI.DAL.Interfaces
{
    public interface ICandidateRepository
    {
        Task<List<Candidate>> GetAllAsync();
        Task<Candidate?> GetByIdAsync(int id);
        Task AddAsync(Candidate candidate);
        Task UpdateAsync(Candidate candidate);
        Task DeleteAsync(int id);
    }
}
