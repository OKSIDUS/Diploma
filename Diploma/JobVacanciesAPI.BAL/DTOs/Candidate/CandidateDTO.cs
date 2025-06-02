namespace JobVacanciesAPI.BAL.DTOs.Candidate
{
    public class CandidateDTO
    {
         public int Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public int Experience { get; set; }
        public string Skills { get; set; } = null!;
        public string ResumeFilePath { get; set; } = null!;
    }
}
