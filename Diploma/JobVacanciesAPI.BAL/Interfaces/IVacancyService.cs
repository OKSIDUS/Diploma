using JobVacanciesAPI.BAL.Entity;

namespace JobVacanciesAPI.BAL.Interfaces
{
    public interface IVacancyService
    {
        Task<List<VacancyDTO>> GetAllVacanciesAsync();
        Task<VacancyDTO> GetVacancyAsync(int id);
        Task CreateVacancyAsync(VacancyDTO vacancy);
        Task UpdateVacancyAsync(VacancyDTO vacancy);
        Task DeleteVacancyAsync(int id);
    }
}
