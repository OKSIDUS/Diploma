namespace JobVacanciesAPI.BAL.DTOs.Vacancy
{
    public class VacancyDTO
    {
        public int Id { get; set; }
        public int RecruiterId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Requirements { get; set; } = null!;
        public decimal SalaryMin { get; set; }
        public decimal SalaryMax { get; set; }
        public string Location { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
