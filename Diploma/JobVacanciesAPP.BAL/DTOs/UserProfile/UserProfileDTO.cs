using JobVacanciesAPP.DAL.Models.Users;

namespace JobVacanciesAPP.BAL.DTOs.UserProfile
{
    public class UserProfileDTO
    {
        public User User { get; set; }
        public Candidate Candidate { get; set; }
        public Recruiter Recruiter { get; set; }
    }

    public enum UserRole
    {
        Candidate,
        Recruiter
    }
}
