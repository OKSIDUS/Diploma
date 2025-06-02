using JobVacanciesAPI.DAL.Entity;

namespace JobVacanciesAPI.DAL.Interfaces
{
    public interface IApplicationRepository
    {
        Task<List<Candidate>> GetCandidatesByVacancyIdAsync(int vacancyId);
    }
}
