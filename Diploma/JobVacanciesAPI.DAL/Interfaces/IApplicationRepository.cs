using JobVacanciesAPI.DAL.Entity;

namespace JobVacanciesAPI.DAL.Interfaces
{
    public interface IApplicationRepository
    {
        Task<List<Candidate>> GetCandidatesByVacancyIdAsync(int vacancyId);
        Task<Application?> GetByIdAsync(int id);
        Task AddAsync(Application application);
        Task UpdateAsync(Application application);
        Task<string> GetStatus(int userID, int vacancyId);
        Task ChangeStatus(int vacancyId, int candidateId, string status);
        Task<List<Application>> GetApplicationsByUserId(int candidateId);
    }
}
