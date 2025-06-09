namespace JobVacanciesAPI.BAL.DTOs.Candidate
{
    public class CandidateShort
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public int? Experience { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
    }
}
