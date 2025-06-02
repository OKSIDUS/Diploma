using JobVacanciesAPI.BAL.DTOs.Recruiter;

namespace JobVacanciesAPI.BAL.Interfaces
{
    public interface IRecruiterService
    {
        Task<List<RecruiterDTO>> GetAllAsync();
        Task<RecruiterDTO?> GetByIdAsync(int id);
        Task AddAsync(RecruiterDTO dto);
        Task UpdateAsync(RecruiterDTO dto);
        Task DeleteAsync(int id);
    }
}
