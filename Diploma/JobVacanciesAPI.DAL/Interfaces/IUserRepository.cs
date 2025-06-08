using JobVacanciesAPI.DAL.Entity;

namespace JobVacanciesAPI.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task UpdateAsync(User user);

        Task<User?> GetByEmail(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<User?> GetById(int id);

        Task EditUserEmail(string email, int userId);
        Task<string> GetUserRole(int userId);
        Task<int> GetCandidateId (int userId);
    }

}
