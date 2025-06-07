using JobVacanciesAPP.BAL.DTOs.UserProfile;
using JobVacanciesAPP.BAL.Interfaces;
using JobVacanciesAPP.DAL.Interfaces;
using JobVacanciesAPP.DAL.Models.Users;
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

        public async Task EditCandidateProfile(CandidateEdit candidateEdit)
        {
            if (candidateEdit != null)
            {
                await _profileRepository.EditCandidateProfile(candidateEdit);
            }
        }

        public async Task EditRecruiterProfile(RecruiterEdit recruiterEdit)
        {
            if (recruiterEdit != null)
            {
                await _profileRepository.EditRecruiterProfile(recruiterEdit);
            }
        }

        public async Task<Skills> GetCandidateSkills(int userId)
        {
            if (userId > 0)
            {
                var skills = await _profileRepository.GetUserSkills(userId);
                return skills;
            }

            return null;
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
