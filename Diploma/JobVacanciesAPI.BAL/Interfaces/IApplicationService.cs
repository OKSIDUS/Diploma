using JobVacanciesAPI.BAL.Entity;

namespace JobVacanciesAPI.BAL.Interfaces
{
    public interface IApplicationService
    {
        Task<List<CandidateDTO>> GetCandidatesByVacancyIdAsync(int vacancyId);
    }
}
