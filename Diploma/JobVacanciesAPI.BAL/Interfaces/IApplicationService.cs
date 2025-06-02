using JobVacanciesAPI.BAL.DTOs.Candidate;

namespace JobVacanciesAPI.BAL.Interfaces
{
    public interface IApplicationService
    {
        Task<List<CandidateDTO>> GetCandidatesByVacancyIdAsync(int vacancyId);
    }
}
