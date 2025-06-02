using JobVacanciesAPI.BAL.DTOs.Candidate;

namespace JobVacanciesAPI.BAL.Interfaces
{
    public interface ICandidateService
    {
        Task<List<CandidateDTO>> GetAllAsync();
        Task<CandidateDTO?> GetByIdAsync(int id);
        Task AddAsync(CandidateDTO dto);
        Task UpdateAsync(CandidateDTO dto);
        Task DeleteAsync(int id);
    }
}
