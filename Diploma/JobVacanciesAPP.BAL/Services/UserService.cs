using JobVacanciesAPI.DAL.Entity;
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
        private readonly IVacancyRepository _vacancyRepository;

        public UserService(IUserProfileRepository profileRepository, IVacancyRepository vacancyRepository)
        {
            _profileRepository = profileRepository;
            _vacancyRepository = vacancyRepository;
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

            var vacancy = new DAL.Models.Vacancy.VacancyPage();

            var profile = await _profileRepository.GetUserProfileAsync(userId);
            if (profile.Candidate == null)
            {
                vacancy = await _vacancyRepository.GetVacanciesForRecruiter(1, 10, false, userId, "");
            }
            else
            {
                vacancy = await _vacancyRepository.GetRecommendetVacancies(1, 10, true, userId, "");
            }
             

            return new UserProfileDTO
            {
                User = profile.User,
                Candidate = profile.Candidate,
                Recruiter = profile.Recruiter,
                VacancyPage = vacancy,
                
            };
        }

        public async Task SaveCandidateSkills(UserSkills skills)
        {
            if(skills != null)
            {
                await _profileRepository.SaveCandidateSkills(skills);
            }
        }
    }
}
