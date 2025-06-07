namespace JobVacanciesAPP.DAL.Models.Users
{
    public class Recruiter
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? CompanyName { get; set; } = null!;
        public string? Position { get; set; } = null!;
    }
}
