using JobVacanciesAPP.DAL.Models.Users;

namespace JobVacanciesAPP.DAL.Models.Vacancy
{
    public class VacancyRecruiterDTO
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public string Location { get; set; }
        public decimal SalaryMin { get; set; }
        public decimal SalaryMax { get; set; }

        public List<CandidateDTO> Candidates { get; set; }
    }
}
