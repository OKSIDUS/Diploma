namespace JobVacanciesAPP.DAL.Models.Vacancy
{
    public class CandidateApplications
    {
        public int VacancyId { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public DateTime AppliedDate { get; set; }
        public string Status { get; set; }
    }
}
