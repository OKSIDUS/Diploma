using JobVacanciesAPI.DAL.Entity;

namespace JobVacanciesAPI.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task UpdateAsync(User user);

        Task<User?> GetByEmail(string email);
        Task<bool> EmailExistsAsync(string email);
    }

}
