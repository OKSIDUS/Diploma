using JobVacanciesAPP.DAL.Models.Vacancy;

namespace JobVacanciesAPP.DAL.Models.Users
{
    public class UserProfile
    {
        public User User { get; set; }
        public Candidate? Candidate { get; set; }
        public Recruiter? Recruiter { get; set; }
        public VacancyPage VacancyPage { get; set; }
    }
}
