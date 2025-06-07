using JobVacanciesAPP.BAL.DTOs.UserProfile;
using JobVacanciesAPP.BAL.Interfaces;
using JobVacanciesAPP.DAL.Interfaces;
using Microsoft.Identity.Client;

namespace JobVacanciesAPP.BAL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserProfileRepository _profileRepository;

        public UserService(IUserProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<UserProfileDTO> GetUserProfileAsync(int userId)
        {
            if (userId < 1)
            {
                return null;
            }

            var profile = await _profileRepository.GetUserProfileAsync(userId);

            return new UserProfileDTO
            {
                User = profile.User,
                Candidate = profile.Candidate,
                Recruiter = profile.Recruiter
            };
        }
    }
}
