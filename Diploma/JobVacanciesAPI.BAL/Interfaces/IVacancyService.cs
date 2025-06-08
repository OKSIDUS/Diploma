using JobVacanciesAPI.BAL.DTOs.Vacancy;

namespace JobVacanciesAPI.BAL.Interfaces
{
    public interface IVacancyService
    {
        Task<List<VacancyDTO>> GetAllAsync();
        Task<VacancyDTO?> GetByIdAsync(int id);
        Task<List<VacancyDTO>> GetByRecruiterAsync(int recruiterId);
        Task AddAsync(VacancyDTO dto);
        Task UpdateAsync(VacancyDTO dto);
        Task DeleteAsync(int id);

        Task CreateVacancy(CreateVacancyDTO vacancyDTO);
    }
}
