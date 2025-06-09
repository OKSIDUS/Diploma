namespace JobVacanciesAPP.DAL.Models.Users
{
    public class Candidate
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; } = null!;
        public int? Experience { get; set; } = null!;
        public string? Skills { get; set; } = null!;
        public string ResumeFilePath { get; set; } = null!;
    }
}
