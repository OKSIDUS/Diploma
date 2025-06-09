namespace JobVacanciesAPI.BAL.DTOs.Vacancy
{
    public class VacancyLongInfo
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public string Location { get; set; }
        public decimal SalaryMin { get; set; }
        public decimal SalaryMax { get; set; }
    }
}
