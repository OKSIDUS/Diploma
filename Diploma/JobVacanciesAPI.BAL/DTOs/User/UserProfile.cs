using JobVacanciesAPI.BAL.DTOs.Candidate;
using JobVacanciesAPI.BAL.DTOs.Recruiter;

namespace JobVacanciesAPI.BAL.DTOs.User
{
    public class UserProfile
    {
        public UserDTO User { get; set; }
        public CandidateDTO Candidate { get; set; }
        public RecruiterDTO Recruiter { get; set; }
    }
}
