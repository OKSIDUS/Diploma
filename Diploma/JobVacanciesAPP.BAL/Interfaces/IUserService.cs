using JobVacanciesAPP.BAL.DTOs.UserProfile;
using JobVacanciesAPP.DAL.Models.Users;

namespace JobVacanciesAPP.BAL.Interfaces
{
    public interface IUserService
    {
        public Task<UserProfileDTO> GetUserProfileAsync(int userId);
        public Task EditRecruiterProfile(RecruiterEdit recruiterEdit);
        public Task EditCandidateProfile(CandidateEdit candidateEdit);
    }
}
