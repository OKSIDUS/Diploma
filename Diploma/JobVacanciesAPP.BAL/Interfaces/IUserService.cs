using JobVacanciesAPP.BAL.DTOs.UserProfile;

namespace JobVacanciesAPP.BAL.Interfaces
{
    public interface IUserService
    {
        public Task<UserProfileDTO> GetUserProfileAsync(int userId);
    }
}
