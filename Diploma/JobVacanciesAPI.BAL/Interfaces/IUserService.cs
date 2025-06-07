using JobVacanciesAPI.BAL.DTOs.User;

namespace JobVacanciesAPI.BAL.Interfaces
{
    public interface IUserService
    {
        public Task<UserProfile> GetUserProfileAsync(int userId);
        public Task EditRecruiterProfile(RecruiterEditDTO recruiterEditDTO);
        public Task EditCandidateProfile(CandidateEditDTO candidateEditDTO);
    }
}
