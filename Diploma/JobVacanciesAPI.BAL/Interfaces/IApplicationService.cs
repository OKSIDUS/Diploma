using JobVacanciesAPI.BAL.DTOs.Application;
using JobVacanciesAPI.BAL.DTOs.Candidate;

namespace JobVacanciesAPI.BAL.Interfaces
{
    public interface IApplicationService
    {
        Task<List<CandidateDTO>> GetCandidatesByVacancyIdAsync(int vacancyId);
        Task<ApplicationDTO?> GetByIdAsync(int id);
        Task AddAsync(ApplicationDTO dto);
        Task UpdateStatusAsync(int id, string newStatus);
        Task ChangeStatus(int vacancyId,int userId, string newStatus);
    }
}
