namespace JobVacanciesAPI.BAL.DTOs.Recruiter
{
    public class RecruiterDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string Position { get; set; } = null!;
    }
}
