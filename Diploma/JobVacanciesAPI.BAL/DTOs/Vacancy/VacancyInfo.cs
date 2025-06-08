namespace JobVacanciesAPI.BAL.DTOs.Vacancy
{
    public class VacancyInfo
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public string Location { get; set; }
        public decimal SalaryMin { get; set; }
        public decimal SalaryMax { get; set; }
    }
}
