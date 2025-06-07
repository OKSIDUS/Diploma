using JobVacanciesAPP.DAL.Models.Users;

namespace JobVacanciesAPP.DAL.Interfaces
{
    public interface IUserProfileRepository
    {
        Task<UserProfile> GetUserProfileAsync(int userId);
    }
}
